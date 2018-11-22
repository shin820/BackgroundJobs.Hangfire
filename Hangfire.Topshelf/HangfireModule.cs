using Abp.Hangfire.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Runtime.Caching.Redis;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.Topshelf
{
    [DependsOn()]
    public class HangfireModule : AbpModule
    {
        private readonly IHostingEnvironment _env;

        public HangfireModule(IHostingEnvironment env)
        {
            _env = env;
        }

        public override void PreInitialize()
        {
            Configuration.BackgroundJobs.UseHangfire();
            Configuration.Caching.UseRedis(options =>
            {
                options.ConnectionString = HangfireSettings.Instance.HangfireRedisConnectionString;
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(HangfireModule).GetAssembly());
        }
    }
}
