using Microsoft.AspNetCore.Mvc;
using Producao_MVC.Models;
using Producao_MVC.Services;
using System.Diagnostics;

namespace Producao_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProducaoAPI _ducaoAPI;

        public HomeController(ILogger<HomeController> logger, ProducaoAPI api)
        {
            _logger = logger;
            _ducaoAPI = api;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
