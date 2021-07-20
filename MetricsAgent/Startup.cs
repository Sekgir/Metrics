using AutoMapper;
using Dapper;
using FluentMigrator.Runner;
using MetricsAgent.DAL;
using MetricsAgent.DAL.Interfaces;
using MetricsAgent.DAL.Repositories;
using MetricsAgent.DTO;
using MetricsAgent.Jobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System.Data;
using System.Data.SQLite;

namespace MetricsAgent
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            ConfigureSqlLiteConnection(services);
            services.AddSingleton<ICpuMetricsRepository, CpuMetricsRepository>();
            services.AddSingleton<IDotNetMetricsRepository, DotNetMetricsRepository>();
            services.AddSingleton<IHddMetricsRepository, HddMetricsRepository>();
            services.AddSingleton<INetworkMetricsRepository, NetworkMetricsRepository>();
            services.AddSingleton<IRamMetricsRepository, RamMetricsRepository>();
            var mapperConfiguration = new MapperConfiguration(mapperProfile => mapperProfile.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);
            ConfigureJobFactory(services);
        }

        private void ConfigureSqlLiteConnection(IServiceCollection services)
        {
            const string connectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";
            SQLiteConnectionManager connectionManager = new SQLiteConnectionManager(connectionString);
            services.AddSingleton<IConnectionManager, SQLiteConnectionManager>(item => connectionManager);
            services.AddFluentMigratorCore()
                .ConfigureRunner(runnerBuilder => runnerBuilder
                    .AddSQLite()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(Startup).Assembly).For.Migrations()
                ).AddLogging(logger => logger
                    .AddFluentMigratorConsole());
        }

        private void ConfigureJobFactory(IServiceCollection services)
        {
            services.AddHostedService<QuartzHostedService>();
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            // добавляем нашу задачу
            services.AddSingleton<CpuMetricJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(CpuMetricJob),
                cronExpression: "0/5 * * * * ?"));
            services.AddSingleton<DotNetMetricJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(DotNetMetricJob),
                cronExpression: "0/5 * * * * ?"));
            services.AddSingleton<HddMetricJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(HddMetricJob),
                cronExpression: "0/5 * * * * ?"));
            services.AddSingleton<NetworkMetricJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(NetworkMetricJob),
                cronExpression: "0/5 * * * * ?"));
            services.AddSingleton<RamMetricJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(RamMetricJob),
                cronExpression: "0/5 * * * * ?"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMigrationRunner migrationRunner)
        {
            migrationRunner.MigrateUp();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
