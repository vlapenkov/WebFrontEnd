using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApi2.Contracts.Intrerfaces;
using Refit;
using Microsoft.AspNetCore.Http;

namespace WebFrontEnd
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private void CheckSameSite(HttpContext httpContext, CookieOptions options)
        {
            if (options.SameSite == SameSiteMode.None)
            {
                //var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
                //if (MyUserAgentDetectionLib.DisallowsSameSiteNone(userAgent))
                {
                    options.SameSite = SameSiteMode.Unspecified;
                }
            }
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // нужен для нормальной работы OpenIdConnect в Chrome 8 без https.
            //Также нужен код в IS4 
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
                options.OnAppendCookie = cookieContext =>
                    CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
                options.OnDeleteCookie = cookieContext =>
                    CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
            });

           ;
            services.AddHttpContextAccessor();

            services.AddTransient<MyAccessTokenHandler>();


            #region identity
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    // чтобы User.Identity.Name работало
                    options.TokenValidationParameters.NameClaimType = "name";
                    options.SignInScheme = "Cookies";

                    options.Authority = Configuration["Services:IS4"];
                    options.RequireHttpsMetadata = false;

                    options.ClientId = "mvc";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code id_token";

                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.Scope.Add("Api1.Scope");
                    options.Scope.Add("Api2.Scope");

                    options.Scope.Add("phone");
                    options.Scope.Add("custom.profile");

                    options.Scope.Add("offline_access");
                });

            #endregion
            // services.AddRazorPages();
            services.AddControllersWithViews();

            var baseAddress = new Uri(Configuration["Services:WebApi2"]);
            services.AddRefitClient<IProductService>()
                .ConfigureHttpClient(c => c.BaseAddress = baseAddress)
                .AddHttpMessageHandler<MyAccessTokenHandler>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCookiePolicy();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapRazorPages();
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            Console.WriteLine($"Service FrontEnd started at: {DateTime.Now}");
        }

        
    }
}
