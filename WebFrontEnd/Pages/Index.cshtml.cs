using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebFrontEnd.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGet()
        {
            ViewData["Message"] = "Hello from webfrontend";

            using (var client = new System.Net.Http.HttpClient())
            {
                // Call *mywebapi*, and display its response in the page
                var request = new System.Net.Http.HttpRequestMessage();
                var webapi = Environment.GetEnvironmentVariable("webapi");

                var url = String.Format("http://{0}/api/Products/GetProduct", webapi ?? "mywebapi");
                Console.WriteLine("Url is: "+url);
                request.RequestUri = new Uri(url); // ASP.NET 3 
                var response = await client.SendAsync(request);
                var result= await response.Content.ReadAsStringAsync();
                Console.WriteLine("Result is: " + result);
                ViewData["Message"] += " and " + result;
            }
        }
    }
}
