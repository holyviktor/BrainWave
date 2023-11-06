using Azure;
using BrainWave.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace BrainWave.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BrainWaveDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, BrainWaveDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

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