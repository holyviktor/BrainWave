using BrainWave.Application.Services;
using BrainWave.Core.Entities;
using BrainWave.Infrastructure.Data;
using BrainWave.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BrainWave.WebUI.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly ILogger<ArticlesController> _logger;
        private readonly BrainWaveDbContext _dbContext;
        private readonly IStringLocalizer<ArticlesController> _stringLocalizer;
        private readonly FiltersService _filtersService;

        public ArticlesController(ILogger<ArticlesController> logger, BrainWaveDbContext dbContext, IStringLocalizer<ArticlesController> stringLocalizer)
        {
            _logger = logger;
            _dbContext = dbContext;
            _stringLocalizer = stringLocalizer;
            _filtersService = new FiltersService(dbContext);
        }

        public ActionResult Index(ArticlesViewModel? articlesViewModel)
        {
            List<Article> articles;
            FilterViewModel filterViewModel = new FilterViewModel
            {
                Categories = _dbContext.Categories.ToList(),
            };

            if (articlesViewModel != null && articlesViewModel.FilterInput != null)
            {
                var filters = articlesViewModel.FilterInput;
                articles = _filtersService.ApplyFilters(filters.Search, filters.Category, filters.Sort, filters.SortOrder);
            }
            else
            {
                articles = _dbContext.Articles
                    .Include(c => c.Comments)
                    .Include(c => c.User)
                    .ToList();
            }
            int userId = 2;
            List<ArticleViewModel> articleViewModels = new();
            var user = _dbContext.Users.Find(userId);
            bool isAuthorised = user != null;
            foreach (Article article in articles)
            {
                var isLiked = _dbContext.Likes.Where(x => x.ArticleId == article.Id).FirstOrDefault(x => x.UserId == userId) != null;
                var isSaved = _dbContext.Savings.Where(x => x.ArticleId == article.Id).FirstOrDefault(x => x.UserId == userId) != null;

                articleViewModels.Add(new ArticleViewModel
                {
                    Id = article.Id,
                    Title = article.Title,
                    CategoryName = article.Category.Name,
                    Date = article.Date,
                    Price = article.Price,
                    Text = article.Text,
                    User = article.User,
                    LikesCount = _dbContext.Likes.Count(x=>x.ArticleId == article.Id),
                    IsLiked = isAuthorised && isLiked,
                    Comments = article.Comments,
                    SavingsCount = _dbContext.Savings.Count(x=>x.ArticleId == article.Id),
                    IsSaved = isAuthorised && isSaved,
                });

            }
            articlesViewModel = new ArticlesViewModel
            {
                Articles = articleViewModels,
                Filter = filterViewModel,
                IsUserAuthorised = isAuthorised
            };
            return View(articlesViewModel);
        }

        public ActionResult Create()
        {
            var categories = _dbContext.Categories.ToList();
            ViewBag.Categories = categories;
            return View();
        }
        [HttpPost]
        public IActionResult Create(ArticleInputViewModel articleInputViewModel)
        {
            if (!ModelState.IsValid)
            {
                var categories = _dbContext.Categories.ToList();
                ViewBag.Categories = categories;
                return View("Create");
            }
            var userId = 2;
            var authorisedUser = _dbContext.Users.Find(userId);
            var categorySearched = _dbContext.Categories.FirstOrDefault(m => m.Id == articleInputViewModel.CategoryId);
            if (categorySearched != null && authorisedUser !=null)
            {
                var articleNew = new Article
                {
                    Title = articleInputViewModel.Title,
                    Text = articleInputViewModel.Text,
                    CategoryId = articleInputViewModel.CategoryId,
                    Price = articleInputViewModel.Price,
                    UserId = userId,
                    Date = DateTime.Now,
                };
                _dbContext.Add(articleNew);
                _dbContext.SaveChanges();
            }
            else
            {
                throw new ArgumentException();
            }
            
            return RedirectToAction("Index", "Profile");
        }
    }
}
