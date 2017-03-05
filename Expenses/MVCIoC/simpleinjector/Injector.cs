using System.Web.Mvc;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Integration.Web;
using Expenses.Services;
using Expenses.Data;

namespace Expenses.MVCIoC.simpleinjector
{
   public  class Injector
    {
        public static Container Initialise()
        {
            //Ioc S I M P L E   I N J E C T O R
            // 1. Create a new Simple Injector container
            var container = new Container();

            // 2. Configure the container (register)
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            container.Register<IExpensesServices, ExpensesServices>(Lifestyle.Transient);
            container.Register<ExpensesDbContext>(Lifestyle.Scoped);
            // 3. Optionally verify the container's configuration.
            container.Verify();

            // 4. Store the container for use by the application

            DependencyResolver.SetResolver(
                new SimpleInjectorDependencyResolver(container));
            return container;
        }
    }
}
