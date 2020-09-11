using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            services.AddControllers();
            services.AddSwaggerDocument();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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


            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ProductsDBContext>();
                context.Database.Migrate();
            }

            Console.WriteLine($"Service MyWebApi started at: {DateTime.Now}");

        }

        
            //private void CreateDb()
            //{
            //    string cs = @"server=db;userid=root;password=root;database=new_base";

            //    using var con = new MySqlConnection(cs);
            //    con.Open();

            //    using var cmd = new MySqlCommand();
            //    cmd.Connection = con;

            //    cmd.CommandText = "DROP TABLE IF EXISTS weather";
            //    cmd.ExecuteNonQuery();

            //    cmd.CommandText = @"CREATE TABLE weather(id INTEGER PRIMARY KEY AUTO_INCREMENT,
            //        weatherdesc TEXT)";
            //    cmd.ExecuteNonQuery();

            //for (int i = 0; i < 10; i++)
            //{
            //    cmd.CommandText = $"INSERT INTO weather(weatherdesc) VALUES({i})";
            //    cmd.ExecuteNonQuery();
            //}
            //}
       
    }
}
