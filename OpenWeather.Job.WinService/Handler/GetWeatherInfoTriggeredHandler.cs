using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using CloudDataAnalytics.Shared.DomainEvents;
using NLog;
using OpenWeather.Job.WinService.Events;

namespace OpenWeather.Job.WinService.Handler
{
    public class GetWeatherInfoTriggeredHandler : IDomainEventHandler<GetWeatherInfoTriggered>
    {

        private ILogger _logger = NullLogger.Instance;

        public void Handle(GetWeatherInfoTriggered msg)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:9000/");
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));



            
            Logger.Info("FromHandler: The current time is:"+ DateTime.Now);
        }

        public ILogger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }
    }
}
