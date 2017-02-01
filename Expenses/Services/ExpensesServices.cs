using Expenses.Data;
using System.Collections.Generic;
using System.Linq;
using Expenses = Expenses.Data.Models.Expenses;
using Expenses.Data.Models;
using System;

namespace Expenses.Services
{
    public interface IExpensesServices
    {
        Data.Models.Expenses AddItem(Data.Models.Expenses model);
        Data.Models.Expenses GetItem(long Id);
        int EditItem(Data.Models.Expenses model);
        int DeleteItem(long Id);
        List<Data.Models.Expenses> GetExpenses();
        List<Data.Models.Expenses> GetExpensesByFiscalYear(int Year);
    }
    public class ExpensesServices : IExpensesServices
    {
        private readonly ExpensesDbContext _db;
        public ExpensesServices(ExpensesDbContext db)
        {
            this._db = db;
        }
        public List<Data.Models.Expenses> GetExpenses()
        {
            return _db.Expenses.ToList();
        }

        public Data.Models.Expenses AddItem(Data.Models.Expenses model)
        {
            //string date = "04/01/" + FiscalYear.ToString();
            //DateTime dtInitial = Convert.ToDateTime(date);
            //DateTime dtEnd = Convert.ToDateTime(date).AddYears(1);
            //if (model.Date >= dtInitial && model.Date < dtEnd)
            //{

            //}
            long Id = 0;
            var FiscalYear = model.Date.Year;
            if (FiscalYear > 0)
            {
                _db.Expenses.Add(model);
                _db.SaveChanges();//this generates the Id for customer
                Id = model.Id;
                return model;
            }
            else
            {
                throw new System.ArgumentException("verify your Information", "Save");
            }
            return new Data.Models.Expenses();
        }

        public Data.Models.Expenses GetItem(long Id)
        {
            return _db.Expenses.Find(Id);
        }

        public int EditItem(Data.Models.Expenses model)
        {
            // find the record to update
            var entityToUpdate = _db.Expenses.Find(model.Id);
            if (entityToUpdate != null)
            {
                //set the new values 
                _db.Entry(entityToUpdate).CurrentValues.SetValues(model);
                return _db.SaveChanges();
            }
            return 0;
        }

        public int DeleteItem(long Id)
        { // find the record to delete
            var entityToDelete = _db.Expenses.Find(Id);
            if (entityToDelete != null)
            {
                _db.Expenses.Remove(entityToDelete);
                return _db.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public List<Data.Models.Expenses> GetExpensesByFiscalYear(int Year)
        {
            if (Year > 0)
            {
                string date = "04/01/" + Year.ToString(); // the fiscal year Start on April 1 of the given year
                DateTime dtInitial = Convert.ToDateTime(date);
                DateTime dtEnd = Convert.ToDateTime(date).AddYears(1);
                return  _db.Expenses.Where(x => x.Date >= dtInitial).Where(x => x.Date < dtEnd).ToList();
            }
            return _db.Expenses.ToList();
        }
    }
}