using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwinConsole
{
    public class Webserver
    {
        IDisposable _webApp;
        public void Start()
        {
            _webApp = WebApp.Start<Startup>("http://localhost:8085");
        }
        public void Stop()
        {
            _webApp?.Dispose();
        }
    }
}
