using System.Diagnostics;
using DEPI_Donation.Models;
using Microsoft.AspNetCore.Mvc;

namespace DEPI_Donation.Controllers
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

        public IActionResult Privacy()
        {
            return View();
        }

     
    }
}
