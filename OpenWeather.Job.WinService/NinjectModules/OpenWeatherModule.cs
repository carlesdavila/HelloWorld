using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CloudDataAnalytics.Shared.DomainEvents;
using Ninject.Modules;
using OpenWeather.Job.WinService.Components;
using OpenWeather.Job.WinService.Events;
using OpenWeather.Job.WinService.Handler;

namespace OpenWeather.Job.WinService.NinjectModules
{
    public class OpenWeatherModule : NinjectModule
    {
        public override void Load()
        {
            var cfg = ConfigurationManager.GetSection("ServiceProviderConfiguration") as OpenWeatherConfiguration;

            //add ninject bindings:
            //Bind<IOpenWeatherConfiguration>().To<OpenWeatherConfiguration>();            
            Bind<IOpenWeatherConfiguration>().ToConstant(cfg);
            //Bind<IDomainEventHandler<GetWeatherInfoTriggered>>().To<GetWeatherInfoTriggeredHandler>();            

        }
    }
}
