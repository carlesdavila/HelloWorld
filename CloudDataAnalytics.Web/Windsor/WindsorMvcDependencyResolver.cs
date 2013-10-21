using System.Web.Mvc;
using Castle.Windsor;

namespace CloudDataAnalytics.Web.Windsor
{
    internal class WindsorMvcDependencyResolver : WindsorDependencyScope, IDependencyResolver
    {
        private readonly IWindsorContainer _container;

        public WindsorMvcDependencyResolver(IWindsorContainer container)
            : base(container)
        {
            this._container = container;
        }

    }
}