using Microsoft.VisualStudio.TestTools.UnitTesting;
using Expenses.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Services.Tests
{
    [TestClass()]
    public class ExpensesServicesTests
    {
            [TestMethod()]
            public void AddItemTest()
            {
                // arrange  
                var ExpensesItem = new Data.Models.Expenses
                {
                    Amount = 963,
                    Description = "This a Unit test for insert record on January 30",
                    Date = System.DateTime.Now,

                };

                // act  
                var service = new ExpensesServices(new Data.ExpensesDbContext()).AddItem(ExpensesItem);
                // assert  
                Assert.IsTrue(service.Id > 0);
            }

            [TestMethod()]
            public void EditItemTest()
            {
                // arrange  
                var ExpensesItem = new Data.Models.Expenses
                {
                    Id = 24,
                    Amount = 1000,
                    Description = "This a Unit test for insert record",
                    Date = System.DateTime.Now,

                };

                // act  
                var EditedItem = new ExpensesServices(new Data.ExpensesDbContext()).EditItem(ExpensesItem);
                // assert  
                Assert.IsTrue(EditedItem == 1);

            }

            [TestMethod()]
            public void DeleteItemTest()
            {
                // arrange  
                var Id = 17;

                // act 

                var Deleted = new ExpensesServices(new Data.ExpensesDbContext()).DeleteItem(Id);
                // assert  
                Assert.IsTrue(Deleted == 1);
            }
        }
    }