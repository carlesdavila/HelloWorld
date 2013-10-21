using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace CloudDataAnalytics.Domain
{
    public static class RegisterComponents
    {
        public static IWindsorContainer Register(IWindsorContainer container, LifestyleType lifeStyle)
        {
            return
                container.Register(
                    //Component.For(typeof (IRepository<>))
                    //         .ImplementedBy(typeof (Repository<>))
                    //         .LifeStyle.Is(lifeStyle)
                    
                    );
        }
    }
}
