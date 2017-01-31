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
    }
}