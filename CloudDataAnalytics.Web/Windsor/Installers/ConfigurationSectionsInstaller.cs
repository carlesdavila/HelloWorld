using System.Configuration;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CloudDataAnalytics.Web.Components;

namespace CloudDataAnalytics.Web.Windsor.Installers
{
    public class ConfigurationSectionsInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var cfg = ConfigurationManager.GetSection("ServiceProviderConfiguration") as ServiceProviderConfiguration;
            container.Register(Component.For<IServiceProviderConfiguration>().Instance(cfg));
        }
    }
}