
using System.Data.Entity;
namespace Expenses.Data
{
    public class ExpensesDbContext : DbContext
    {
        public ExpensesDbContext() : base("name=MyContext")
        {
        }
        public virtual DbSet<Models.Expenses> Expenses { get; set; }
    }
}
