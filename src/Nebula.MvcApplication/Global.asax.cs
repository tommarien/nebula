using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Nebula.Bootstrapper;
using Nebula.MvcApplication.Modules;

namespace Nebula.MvcApplication
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        private static IWindsorContainer WindsorContainer;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterWindsor();
        }

        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new {favicon = @"(.*/)?favicon.ico(/.*)?"});

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new {controller = "Home", action = "Index", id = UrlParameter.Optional} // Parameter defaults
                );
        }

        private static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        private static void RegisterWindsor()
        {
            WindsorContainer = Boot.Container()
                .Install(FromAssembly.This());

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(WindsorContainer.Kernel));
        }
    }
}