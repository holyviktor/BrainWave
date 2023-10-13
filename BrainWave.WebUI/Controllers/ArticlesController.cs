using BrainWave.Infrastructure.Data;
using BrainWave.WebUI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BrainWave.WebUI.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly ILogger<ArticlesController> _logger;
        private readonly BrainWaveDbContext _dbContext;

        public ArticlesController(ILogger<ArticlesController> logger, BrainWaveDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        // GET: ArticlesController
        public ActionResult Index()
        {
            List<string> listSort = new List<string>{"first", "second"};
            FilterViewModel filterViewModel = new FilterViewModel
            {
                Sort = listSort,
                Filter = null,
                Categories = _dbContext.Categoris.ToList()
            };
            ArticlesViewModel articlesViewModel = new ArticlesViewModel
            {
                Articles = _dbContext.Articles.ToList(),
                Filter = filterViewModel,
                FilterInput = null
            };
            return View(articlesViewModel);
        }

        public ArticlesViewModel ApplyFilters(ArticlesViewModel articlesViewModel)
        {
            return articlesViewModel;
        }

        // GET: ArticlesController/Details/5
        
    }
}
