using System;
using Castle.Facilities.Logging;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CloudDataAnalytics.Shared.DomainEvents;
using OpenWeather.Job.WinService.Events;
using OpenWeather.Job.WinService.Handler;
using OpenWeather.Job.WinService.NinjectModules;
using Quartz;
using Topshelf;
using Topshelf.Ninject;
using Topshelf.Quartz;
using Topshelf.Quartz.Ninject;


namespace OpenWeather.Job.WinService
{
    class Program
    {

        static void Main(string[] args)
        {
            IWindsorContainer container = new WindsorContainer();

            container.Register(Component.For<IDomainEventHandler<GetWeatherInfoTriggered>>().ImplementedBy<GetWeatherInfoTriggeredHandler>());
            container.AddFacility<LoggingFacility>(f => f.LogUsing(LoggerImplementation.NLog).WithConfig("NLog.config"));

            DomainEventDispatcher.SetContainer(container);           

            HostFactory.Run(c =>
            {
                // Topshelf.Ninject (Optional) - Initiates Ninject and consumes Modules
                c.UseNinject(new OpenWeatherModule());

                c.Service<OpenWeatherCollectorService>(s =>
                {
                    //Topshelf.Ninject (Optional) - Construct service using Ninject
                    s.ConstructUsingNinject();

                    s.WhenStarted((service, control) => service.Start());
                    s.WhenStopped((service, control) => service.Stop());

                    // Topshelf.Quartz.Ninject (Optional) - Construct IJob instance with Ninject
                    s.UseQuartzNinject();

                    // Schedule a job to run in the background every 5 seconds.
                    // The full Quartz Builder framework is available here.
                    s.ScheduleQuartzJob(q =>
                        q.WithJob(() =>
                           // JobBuilder.Create<SampleJob>().Build())
                            JobBuilder.Create<DomainEventLauncherJob<GetWeatherInfoTriggered>>().Build())
                        .AddTrigger(() =>
                            TriggerBuilder.Create()
                                .WithSimpleSchedule(builder => builder
                                    .WithIntervalInSeconds(20)
                                    .RepeatForever())
                                .Build())
                        );
                });
            });
        }
    }



    public class OpenWeatherCollectorService
    {
        public bool Start()
        {
            Console.WriteLine("OpenWeather Collector Service Started...");
            return true;
        }

        public bool Stop()
        {
            Console.WriteLine("OpenWeather Collector Service stoped!");
            return true;
        }
    }

    public class SampleJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("The current time is: {0}", DateTime.Now);
        }
    }

}
