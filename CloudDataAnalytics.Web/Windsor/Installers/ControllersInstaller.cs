using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Mvc;

namespace CloudDataAnalytics.Web.Windsor.Installers
{
	public class ControllersInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
		    container.Register(Classes.FromThisAssembly()
		                              .BasedOn<IController>()
                                      .OrBasedOn(typeof(ApiController))
		                              .LifestylePerWebRequest());
            container.Register(Component.For<ODataMetadataController>().LifestylePerWebRequest());
		}
	}
}