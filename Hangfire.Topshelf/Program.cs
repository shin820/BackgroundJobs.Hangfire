using System;
using Topshelf;

namespace Hangfire.Topshelf
{
    class Program
    {
        static void Main(string[] args)
        {
            var rc = HostFactory.Run(x =>
              {
                  x.Service<Bootstrap>();

                  x.RunAsLocalSystem();

                  x.SetServiceName(HangfireSettings.Instance.ServiceName);
                  x.SetDisplayName(HangfireSettings.Instance.ServiceDisplayName);
                  x.SetDescription(HangfireSettings.Instance.ServiceDescription);

                  x.SetStartTimeout(TimeSpan.FromMinutes(5));
                  //https://github.com/Topshelf/Topshelf/issues/165
                  x.SetStopTimeout(TimeSpan.FromMinutes(35));

                  x.EnableServiceRecovery(r => { r.RestartService(1); });
              });

            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode;
        }
    }
}
