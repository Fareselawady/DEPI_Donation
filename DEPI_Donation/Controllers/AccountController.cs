using DEPI_Donation.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace DEPI_Donation.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbcontext db;

        public AccountController(AppDbcontext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

 
    }
}
