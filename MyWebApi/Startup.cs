using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace MyWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ProductsDBContext>(
           options => options.UseSqlServer(Configuration.GetConnectionString("SqlConnection")));

            services.AddMassTransit(x =>
            {
                x.AddConsumer<ProductConsumer>();
                x.UsingRabbitMq((context,cfg)=> {
                    cfg.Host("rabbitmq://rab01");
                    cfg.ReceiveEndpoint("event-listener", e =>
                    {
                        e.ConfigureConsumer<ProductConsumer>(context);
                    });

                });
               

               
            });

            services.AddMassTransitHostedService();

            services.AddControllers();
            services.AddSwaggerDocument();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            Console.WriteLine($"Service MyWebApi getting started at: {DateTime.Now}");
            
            Console.WriteLine($"Connection string is {Configuration.GetConnectionString("SqlConnection")}");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

          // CreateDb();

            app.UseRouting();

            app.UseAuthorization();

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            try
            {
                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetService<ProductsDBContext>();
                    context.Database.Migrate();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"MyWebApi migration not working");
                Console.Write(e.Message);
                Console.Write(e.InnerException);
                Console.Write(e.StackTrace);
            }
            Console.WriteLine($"Service MyWebApi started at: {DateTime.Now}");

        }

        
            
       
    }
}
