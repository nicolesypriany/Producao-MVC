using Microsoft.AspNetCore.Mvc;
using Producao_MVC.Models;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Producao_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error(Exception exception)
        {
            return View(new ErrorViewModel
            {
                Message = exception.InnerException.Message
            });
        }
    }
}
