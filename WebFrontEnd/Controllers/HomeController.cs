using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebFrontEnd.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            //using (var client = new HttpClient())
            //{
            //    var result = await client.GetAsync("http://192.168.0.106:5100");
            //    Console.WriteLine(result.StatusCode);
            //}
            return View();
        }

        [Authorize]
        public IActionResult Restricted()
        {
            return View();
        }

        public IActionResult Logout()
        {
            // return new SignOutResult(new[] { "oidc", "Cookies" });
            return SignOut("Cookies", "oidc");
        }
    }
}