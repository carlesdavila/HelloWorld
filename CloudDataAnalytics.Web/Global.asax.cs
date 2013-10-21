using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.Windsor;
using Castle.Windsor.Installer;
using CloudDataAnalytics.Shared.DomainEvents;
using CloudDataAnalytics.Web.Windsor;

namespace CloudDataAnalytics.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        private IWindsorContainer container;

        protected void Application_Start()
        {
            this.container = new WindsorContainer().Install(FromAssembly.This());

            DomainEventDispatcher.SetContainer(container);

            //AreaRegistration.RegisterAllAreas();
            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            CertificateConfig.RegisterCertificates(Application);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            GlobalConfiguration.Configuration.Services.Replace( typeof(IHttpControllerActivator),new WindsorCompositionRoot(this.container));

            //mvc
            DependencyResolver.SetResolver(new WindsorMvcDependencyResolver(container));
        }

        protected void Application_End()
        {
            if (container != null)
                container.Dispose();
        }
    }
}