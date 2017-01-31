using Expenses.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            return View(_service.GetExpenses());
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View(new Data.Models.Expenses());
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
    }
}