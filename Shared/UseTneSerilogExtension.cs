using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Reflection;

namespace Shared
{
    public static partial class SerilogExtensions
    {
        /// <summary>
        /// Подключение Serilog с преднастроенными параметрами для логирвания.
        /// В качестве URL сервиса ElasticSearch - используется параметр 'Services:Elasticsearch:BaseUri' из appsettings.json
        /// В качестве applicationName - используется EntryAssemblyName[1] Имя DLL должно быть формата TNE.ServiceName.Other
        /// </summary>
        /// <param name="hostBuilder">IWebHostBuilder</param>
        /// <returns>IWebHostBuilder</returns>
        public static IWebHostBuilder UseTneSerilog(this IWebHostBuilder hostBuilder)
        {
           

            string uriPath = "Services:Elasticsearch:BaseUri";
            string[] arrAppName = Assembly.GetEntryAssembly().GetName().Name.Split('.');
            string appName = arrAppName.Length > 1 ? arrAppName[1] : arrAppName[0];

            hostBuilder.UseTneSerilog(uriPath, appName);

            return hostBuilder;
        }

        /// <summary>
        /// Подключение Serilog с преднастроенными параметрами для логирвания.
        /// </summary>
        /// <param name="hostBuilder">IWebHostBuilder</param>
        /// <param name="url">Url сервиса ElasticSearch</param>
        /// <param name="applicationName">Имя приложения/сервиса которое используется Serilog как имя логфайла и для логирования в параметре ServiceName</param>
        /// <returns>IWebHostBuilder</returns>
        public static IWebHostBuilder UseTneSerilog(this IWebHostBuilder hostBuilder, string url, string applicationName)
        {
            string uriPath = url;
            string date = DateTime.Now.ToString("yyyyMMdd");

            hostBuilder.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                .ReadFrom.Configuration(hostingContext.Configuration) // на 10.01.2020 берем только MinimumLevel, остальное определяем в коде. 
                .Enrich.FromLogContext()
                .Enrich.WithMachineName() //todo: sntr: на никсах почемуто имя компа не записывается в лог. надо разбираться.
                
                .Enrich.WithProperty("ServiceName", applicationName)
             //   .Enrich.WithProperty("UserName", "Anonymous")
             //   .Enrich.WithProperty("EventLevel", "Application")
             //   .Enrich.WithProperty("EventOperation", "")
                .WriteTo.Console()
                .WriteTo.File($"Logs/{applicationName}-{date}.txt",
                Serilog.Events.LogEventLevel.Verbose,
                "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{ServiceName}] [{Level}] {Message} {UserName} {ActionName} {TestUser}  {NewLine} {Exception}")
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://es01:9200"/*hostingContext.Configuration[uriPath]*/))
                {
                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
                  //  IndexDecider = SerilogExtensions.GetElasticsearchIndexName
                })
            );

            return hostBuilder;
        }
    }
}
