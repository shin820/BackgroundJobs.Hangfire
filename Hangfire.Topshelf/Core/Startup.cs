using Abp.AspNetCore;
using Abp.Castle.Logging.Log4Net;
using Castle.Core.Logging;
using Castle.Facilities.Logging;
using Hangfire.Dashboard;
using Hangfire.Topshelf.Jobs;
using Hangfire.Windsor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Hangfire.Topshelf.Core
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(configuration =>
            {
                configuration.UseRedisStorage(HangfireSettings.Instance.HangfireRedisConnectionString);
            });

            //Configure Abp and Dependency Injection
            return services.AddAbp<HangfireModule>(options =>
            {
                //Configure Log4Net logging
                options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                );

                // 让Hangfire能够从容器中构造出实列。
                JobActivator.Current = new WindsorJobActivator(options.IocManager.IocContainer.Kernel);
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // 默认不自动重试
            app.UseHangfireFilters(new AutomaticRetryAttribute { Attempts = 0 });

            // 启用Hangfire作业服务器
            var queues = new[] { "default", "apis", "jobs" };
            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                ShutdownTimeout = TimeSpan.FromMinutes(30),
                Queues = queues,
                WorkerCount = Math.Max(Environment.ProcessorCount, 20)
                //HeartbeatInterval = new TimeSpan(0, 1, 0),
                //ServerCheckInterval = new TimeSpan(0, 1, 0),
                //SchedulePollingInterval = new TimeSpan(0, 1, 0)
            });

            // 启用Hangfire作业管理页面
            app.UseHangfireDashboard("");

            app.UseDashboardMetric();

            app.UseRecurringJob(typeof(RecurringJobService));

            //app.UseRecurringJob(container);

            app.UseRecurringJob("recurringjob.json");
        }
    }
}
