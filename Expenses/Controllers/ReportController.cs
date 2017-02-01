using Expenses.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Expenses.Controllers
{
    public class ReportController : Controller
    {
        private readonly IExpensesServices _service;
        public ReportController(IExpensesServices service)
        {
            this._service = service;
        }

        // GET: Report
        [HttpGet]
        public ActionResult Index(Data.Models.ExpenseReport model)
        {
            // The generated report will show all records inserted between April 1 of the given year and April 1 of the next year.
            if (model.FiscalYear > 0)
            {
                var report = _service.GetExpensesByFiscalYear(model.FiscalYear);
                var ItemList = new Data.Models.ExpenseReport();
                foreach (var item in report)
                {
                    ItemList.Report.Add(new Data.Models.Expenses
                    {
                        Date = item.Date,
                        Description = item.Description,
                        Amount = item.Amount,
                        Id = item.Id
                    });
                }
                return View(ItemList);
            }
            else
            {
                return View(new Data.Models.ExpenseReport());
            }
        }

    }
}