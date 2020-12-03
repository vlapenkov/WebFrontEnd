using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApi2.Contracts.Intrerfaces;

namespace WebFrontEnd.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IProductService _service;

        public IndexModel(ILogger<IndexModel> logger, IProductService service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task OnGet()
        {
            ViewData["Message"] = "Hello from webfrontend";
           var result = await _service.GetProducts();
            Console.WriteLine("Result is: " + result);
                ViewData["Message"] += " and " + JsonConvert.SerializeObject(result);

            //using (var client = new System.Net.Http.HttpClient())
            //{
            //    // Call *mywebapi*, and display its response in the page
            //    var request = new System.Net.Http.HttpRequestMessage();
            //    var webapi = "webapi2";
            //   // var webapi = Environment.GetEnvironmentVariable("webapi");

            //    var url = String.Format("http://{0}/api/Products/GetProduct", webapi);
            //    Console.WriteLine("Url is: "+url);
            //    request.RequestUri = new Uri(url); // ASP.NET 3 
            //    var response = await client.SendAsync(request);
            //    var result= await response.Content.ReadAsStringAsync();
            //    Console.WriteLine("Result is: " + result);
            //    ViewData["Message"] += " and " + result;
            //}
        }
    }
}
