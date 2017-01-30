using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;
using System.Linq;
using System.Web.Mvc;
namespace Expenses.MVCIoC
{
    public class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            return container;
        }
        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            // container.RegisterType<IExpensesServices, ExpensesServices>();
            MvcUnityContainer.Container = container;
            return container;
        }
    }
}
