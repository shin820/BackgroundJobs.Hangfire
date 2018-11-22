using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Windsor;
using Hangfire.Common;
using Hangfire.Console;
using Hangfire.Dashboard;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;
using Microsoft.AspNetCore.Builder;
using Topshelf;
using Topshelf.HostConfigurators;

namespace Hangfire.Topshelf.Core
{
    public static class HangfireExtensions
    {
        public static IApplicationBuilder UseHangfireFilters(this IApplicationBuilder app, params JobFilterAttribute[] filters)
        {
            if (filters == null) throw new ArgumentNullException(nameof(filters));

            foreach (var filter in filters)
            {
                GlobalConfiguration.Configuration.UseFilter(filter);
            }

            return app;
        }

        public static IApplicationBuilder UseDashboardMetric(this IApplicationBuilder app)
        {
            GlobalConfiguration.Configuration
                .UseDashboardMetric(DashboardMetrics.ServerCount)
                //.UseDashboardMetric(SqlServerStorage.ActiveConnections)
                //.UseDashboardMetric(SqlServerStorage.TotalConnections)
                .UseDashboardMetric(DashboardMetrics.RecurringJobCount)
                .UseDashboardMetric(DashboardMetrics.RetriesCount)
                .UseDashboardMetric(DashboardMetrics.AwaitingCount)
                .UseDashboardMetric(DashboardMetrics.EnqueuedAndQueueCount)
                .UseDashboardMetric(DashboardMetrics.ScheduledCount)
                .UseDashboardMetric(DashboardMetrics.ProcessingCount)
                .UseDashboardMetric(DashboardMetrics.SucceededCount)
                .UseDashboardMetric(DashboardMetrics.FailedCount)
                .UseDashboardMetric(DashboardMetrics.DeletedCount);

            return app;
        }

        //public static IGlobalConfiguration UseRecurringJob(this IGlobalConfiguration configuration, IWindsorContainer container)
        //{
        //    if (container == null) throw new ArgumentNullException(nameof(container));

        //    //var interfaceTypes = container.ComponentRegistry
        //    //    .RegistrationsFor(new TypedService(typeof(IDependency)))
        //    //    .Select(x => x.Activator)
        //    //    .OfType<ReflectionActivator>()
        //    //    .Select(x => x.LimitType.GetInterface($"I{x.LimitType.Name}"));

        //    return GlobalConfiguration.Configuration.UseRecurringJob(() => interfaceTypes);
        //}

        //public static IApplicationBuilder UseRecurringJob(this IApplicationBuilder app, IWindsorContainer container)
        //{
        //    if (container == null) throw new ArgumentNullException(nameof(container));

        //    GlobalConfiguration.Configuration.UseRecurringJob(container);

        //    return app;
        //}
        public static IApplicationBuilder UseRecurringJob(this IApplicationBuilder app, params Type[] types)
        {
            if (types == null) throw new ArgumentNullException(nameof(types));

            GlobalConfiguration.Configuration.UseRecurringJob(types);

            return app;
        }
        public static IApplicationBuilder UseRecurringJob(this IApplicationBuilder app, Func<IEnumerable<Type>> typesProvider)
        {
            if (typesProvider == null) throw new ArgumentNullException(nameof(typesProvider));

            GlobalConfiguration.Configuration.UseRecurringJob(typesProvider);

            return app;
        }

        public static IApplicationBuilder UseRecurringJob(this IApplicationBuilder app, string jsonFile)
        {
            if (string.IsNullOrEmpty(jsonFile)) throw new ArgumentNullException(nameof(jsonFile));

            GlobalConfiguration.Configuration.UseRecurringJob(jsonFile);

            return app;
        }
    }
}
