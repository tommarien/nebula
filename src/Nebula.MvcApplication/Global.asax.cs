using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using Castle.Core.Logging;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Nebula.Bootstrapper;
using Nebula.MvcApplication.Helpers;
using Nebula.MvcApplication.Modules;
using log4net;

namespace Nebula.MvcApplication
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication, IContainerAccessor
    {
        private static IWindsorContainer WindsorContainer;
        private static ILogger Logger;

        #region IContainerAccessor Members

        public IWindsorContainer Container
        {
            get { return WindsorContainer; }
        }

        #endregion

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterWindsor();

            SetupLog4Net();
        }

        private static void SetupLog4Net()
        {
            Logger = WindsorContainer.Resolve<ILoggerFactory>().Create("Nebula.Application");
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

        protected void Application_PostAcquireRequestState(object sender, EventArgs e)
        {
            if (Context.Handler is IRequiresSessionState)
            {
                ThreadContext.Properties["sessionId"] = new SessionIdProvider().ToString();
                ThreadContext.Properties["UserName"] = new UserNameProvider().ToString();
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError().GetBaseException();
            Logger.Error(exception.Message, exception);
        }
    }
}