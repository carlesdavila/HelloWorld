using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irisys.Domain;
using Topshelf;

namespace Irisys.WinService
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 5000;

            HostFactory.Run(x =>                                
            {
                x.Service<BlackfinServer>(s =>                       
                {
                    s.ConstructUsing(name => new BlackfinServer());
                    s.WhenStarted(tc => tc.StartServer(port));             
                    s.WhenStopped(tc => tc.ShutdownServer());              
                });
                x.RunAsLocalSystem();

                x.SetDescription("Irisys server collector");       
                x.SetDisplayName("IrisyServer");                       
                x.SetServiceName("IrisysService");                      
            });                                          
        }
    }
}
