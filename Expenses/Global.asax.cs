using Expenses.MVCIoC.simpleinjector;
using System.Web.Mvc;
using System.Web.Routing;
using Expenses.Services;
using System.Data.Entity;
using Expenses.Data;
namespace Expenses
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //Ioc S I M P L E   I N J E C T O R
            Injector.Initialise(); //if you want to user Unity,coment this line and discoment the lines below

            //If you want to use Unity discoment this line .Initialize IoC container/Unity
            // Bootstrapper.Initialise();
            //Register our custom controller factory
            // ControllerBuilder.Current.SetControllerFactory(typeof(ControllerFactory));
        }
    }
}
