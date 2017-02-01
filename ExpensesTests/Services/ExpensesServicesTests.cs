using Microsoft.VisualStudio.TestTools.UnitTesting;
using Expenses.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Moq;
using Expenses.Data;

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
            var mockSet = new Mock<DbSet<Data.Models.Expenses>>();

            var mockContext = new Mock<ExpensesDbContext>();
            mockContext.Setup(m => m.Expenses).Returns(mockSet.Object);

            var service = new ExpensesServices(mockContext.Object);
            service.AddItem(ExpensesItem);

            // the test verifies that the service added a new Expense and called SaveChanges on the context.
            mockSet.Verify(m => m.Add(It.IsAny<Data.Models.Expenses>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
           
        }


        [TestMethod()]
        public void GetExpensesByFiscalYearTest()
        {
            // arrange 

            //add more items if you want.
            var data = new List<Data.Models.Expenses>
            {
                new Data.Models.Expenses { Description = "AAAAA",Amount= 1000, Date=new DateTime(2016,02,14) },
                new Data.Models.Expenses { Description = "bbbbbb" ,Amount= 2000,Date=new DateTime(2017,02,14) },
                new Data.Models.Expenses { Description = "cccccc",Amount= 3000,Date=new DateTime(2018,02,14) },
                new Data.Models.Expenses { Description = "DDDDDD",Amount= 4000,Date=new DateTime(2018,02,20) },
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Data.Models.Expenses>>();
            mockSet.As<IQueryable<Data.Models.Expenses>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Data.Models.Expenses>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Data.Models.Expenses>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Data.Models.Expenses>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<ExpensesDbContext>();
            mockContext.Setup(c => c.Expenses).Returns(mockSet.Object);

            var service = new ExpensesServices(mockContext.Object);
            // act 

            var item = service.GetExpensesByFiscalYear(2017);// if the fiscal year is 2017, the list should be all the records where the date between Aplil 1, 2017 and  March 31 2018
            // assert  
            Assert.AreEqual(2, item.Count);// if the test is correct with the initialized data,we expect 2 rows
            Assert.AreEqual( 7000, item.Sum(x => x.Amount)); //the amount sum should be 7000(with the initialized data)
        }
    }
}