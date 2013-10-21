using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CloudDataAnalytics.Domain;
using ServMan.Domain;
using ServMan.Domain.Interfaces;

namespace ServMan.Web.Windsor.Installers
{
    public class DomainInstaller:IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container
                .Register(Classes
                              .FromAssemblyContaining(typeof (IUserService))
                              .Pick().WithService
                              .FirstInterface()
                              .LifestylePerWebRequest());
            // repos
            RegisterComponents.Register(container, LifestyleType.PerWebRequest);
        }
    }
}