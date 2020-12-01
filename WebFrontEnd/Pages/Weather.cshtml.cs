using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;

namespace WebFrontEnd.Pages
{
    public class WeatherModel : PageModel
    {
        
        public async Task OnGet()
        {
            ViewData["Message"] = "Hello from webfrontend";

            using (var client = new System.Net.Http.HttpClient())
            {
                var webapi = Environment.GetEnvironmentVariable("webapi");

               var url= String.Format("http://{0}/WeatherForecast",webapi?? "mywebapi");
                // Call *mywebapi*, and display its response in the page
                var request = new System.Net.Http.HttpRequestMessage();
                request.RequestUri = new Uri(url);
               
                var response = await client.SendAsync(request);
                ViewData["Message"] += await response.Content.ReadAsStringAsync();
            }
        }
    }
}