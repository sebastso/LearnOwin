using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace OwinConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<Webserver>(s =>
                {
                    s.ConstructUsing(name => new Webserver());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("This is a demonstration of a Windows Service using Topshelf.");
                x.SetDisplayName("Self Host Web API Demo"); 
                x.SetServiceName("AspNetSelfHostDemo");
            });
        }
    }
}
