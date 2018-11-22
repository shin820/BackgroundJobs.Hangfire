using Hangfire.Topshelf.Core;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Diagnostics;
using Topshelf;

namespace Hangfire.Topshelf
{
    public class Bootstrap : ServiceControl
    {
        private IWebHost _webHost;
        public string Address { get; set; }

        public bool Start(HostControl hostControl)
        {
            //try
            //{
                _webHost = WebHost.CreateDefaultBuilder()
                .UseUrls(HangfireSettings.Instance.ServiceAddress)
                .UseStartup<Startup>()
                .Build();
                _webHost.Run();
                return true;
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.Error("Topshelf starting occured errors.", ex);
            //    return false;
            //}
        }

        public bool Stop(HostControl hostControl)
        {
            //try
            //{
                _webHost.Dispose();
                return true;
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.Error("Topshelf starting occured errors.", ex);
            //    return false;
            //}
        }
    }
}
