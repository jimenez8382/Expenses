using Expenses.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Expenses.Models;
using System.Web.Mvc;

namespace Expenses.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly IExpensesServices _service;
        public ExpensesController(IExpensesServices service)
        {
            this._service = service;
        }
        public ActionResult Index()
        {// return the complete Expense List
            return View();
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View(new Data.Models.Expenses { Date=DateTime.Now});
        }
        [HttpPost]
        public ActionResult Create(Data.Models.Expenses model)
        {
            try
            {
                var itemAdded = _service.AddItem(model);
                // Adding succesful message
                if (itemAdded.Id > 0)
                {
                    this.TempData["Changed"] = itemAdded.Id;
                    this.TempData["Notification"] = "Your record was created successfully.";
                    this.TempData["NotificationClass"] = "notificationbox notibox-success";
                }
            }
            catch (Exception ex)
            {
                //adding error message
                this.TempData["Notification"] = "We had a problem to save your Record,Verify your information and try again.";
                this.TempData["NotificationClass"] = "notificationbox notibox-error";
            }
            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(long Id)
        {
            // find the row we need to update and return new ExpenseItem.
            var item = _service.GetItem(Id);
            var editIt = new Data.Models.Expenses
            {
                Amount = item.Amount,
                Date = item.Date,
                Description = item.Description,
                Id = item.Id
            };
            return View(editIt);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Data.Models.Expenses model)
        {
            try
            { // Adding succesful message 
                var item = _service.EditItem(model);
                if (item > 0)
                {
                    this.TempData["Changed"] = model.Id;
                    this.TempData["Notification"] = "Your record was updated successfully.";
                    this.TempData["NotificationClass"] = "notificationbox notibox-success";
                }
            }
            catch (Exception ex)
            { //adding error message
                this.TempData["Notification"] = "We had a problem to Update your Record,Verify your information and try again.";
                this.TempData["NotificationClass"] = "notificationbox notibox-error";
            }
            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(long Id)
        {
            try
            {
                var item = _service.DeleteItem(Id);
                // Adding succesful message 
                this.TempData["Notification"] = "Your record was deleted successfully.";
                this.TempData["NotificationClass"] = "notificationbox notibox-success";
            }
            catch (Exception ex)
            { // Adding error message 
                this.TempData["Notification"] = "We had a problem to delete your Record,try again.";
                this.TempData["NotificationClass"] = "notificationbox notibox-error";
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Returns data by the criterion
        /// </summary>
        /// <param name="param">Request sent by DataTables plugin</param>
        /// <returns>JSON text used to display data
        /// <list type="">
        /// <item>sEcho - same value as in the input parameter</item>
        /// <item>iTotalRecords - Total number of unfiltered data. This value is used in the message: 
        /// "Showing *start* to *end* of *iTotalDisplayRecords* entries (filtered from *iTotalDisplayRecords* total entries)
        /// </item>
        /// <item>iTotalDisplayRecords - Total number of filtered data. This value is used in the message: 
        /// "Showing *start* to *end* of *iTotalDisplayRecords* entries (filtered from *iTotalDisplayRecords* total entries)
        /// </item>
        /// <item>aoData - Twodimensional array of values that will be displayed in table. 
        /// Number of columns must match the number of columns in table and number of rows is equal to the number of records that should be displayed in the table</item>
        /// </list>
        /// </returns>
        public ActionResult AjaxHandler(JQueryDataTableParamModel param)
        {
            var expensesList = _service.GetExpenses();
            IEnumerable<Data.Models.Expenses> FilteredExpensesList;
            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var descriptionFilter = Convert.ToString(Request["sSearch_1"]);
                var dateFilter = Convert.ToString(Request["sSearch_2"]);
                var amountFilter = Convert.ToString(Request["sSearch_3"]);

                //Optionally check whether the columns are searchable at all 
                var isDescriptionSearchable = Convert.ToBoolean(Request["bSearchable_1"]);
                var isDateSearchable = Convert.ToBoolean(Request["bSearchable_2"]);
                var isAmountSearchable = Convert.ToBoolean(Request["bSearchable_3"]);

                FilteredExpensesList = expensesList.Where(c => isDescriptionSearchable && c.Description.ToLower().Contains(param.sSearch.ToLower())
                || isDateSearchable && c.Date.ToShortDateString().ToLower().Contains(param.sSearch.ToLower())
                || isAmountSearchable && c.Amount.ToString("C").ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                FilteredExpensesList = expensesList;
            }

            var isDescriptionSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isDateSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isAmountSortable = Convert.ToBoolean(Request["bSortable_3"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Data.Models.Expenses, string> orderingFunction = (c => sortColumnIndex == 1 && isDescriptionSortable ? c.Description :
                                                           sortColumnIndex == 2 && isDateSortable ? c.Date.ToShortDateString() :
                                                           sortColumnIndex == 3 && isAmountSortable ? c.Amount.ToString("C") : "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                FilteredExpensesList = FilteredExpensesList.OrderBy(orderingFunction);
            else
                FilteredExpensesList = FilteredExpensesList.OrderByDescending(orderingFunction);

            var displayedItems = FilteredExpensesList.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = from c in displayedItems select new[] { c.Description, c.Date.ToShortDateString(), c.Amount.ToString("C"), Convert.ToString(c.Id)};
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = expensesList.Count(),
                iTotalDisplayRecords = FilteredExpensesList.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }
    }
}