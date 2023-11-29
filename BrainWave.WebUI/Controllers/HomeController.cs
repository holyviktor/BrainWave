using BrainWave.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace BrainWave.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}