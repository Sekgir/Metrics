using AutoMapper;
using Dapper;
using MetricsAgent.DAL;
using MetricsAgent.DAL.Interfaces;
using MetricsAgent.DAL.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        }

        private void ConfigureSqlLiteConnection(IServiceCollection services)
        {
            const string connectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";
            SQLiteConnectionManager connectionManager = new SQLiteConnectionManager(connectionString);
            services.AddSingleton<IConnectionManager, SQLiteConnectionManager>(item => connectionManager);
            PrepareSchema(connectionManager.CreateOpenedConnection());
        }

        private void PrepareSchema(IDbConnection connection)
        {
            connection.Execute(@"CREATE TABLE IF NOT EXISTS CpuMetrics(id INTEGER PRIMARY KEY, value INT, time INT)");
            connection.Execute(@"CREATE TABLE IF NOT EXISTS DotNetMetrics(id INTEGER PRIMARY KEY, value INT, time INT)");
            connection.Execute(@"CREATE TABLE IF NOT EXISTS HddMetrics(id INTEGER PRIMARY KEY, value INT, time INT)");
            connection.Execute(@"CREATE TABLE IF NOT EXISTS NetworkMetrics(id INTEGER PRIMARY KEY, value INT, time INT)");
            connection.Execute(@"CREATE TABLE IF NOT EXISTS RamMetrics(id INTEGER PRIMARY KEY, value INT, time INT)");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
