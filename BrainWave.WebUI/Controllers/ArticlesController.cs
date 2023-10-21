using BrainWave.Application.Services;
using BrainWave.Core.Entities;
using BrainWave.Infrastructure.Data;
using BrainWave.WebUI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

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

        // GET: ArticlesController
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
                    .Include(c => c.Likes)
                    .Include(c => c.Savings)
                    .ToList();
            }
            int userId = 2;
            List<ArticleViewModel> articleViewModels = new List<ArticleViewModel>();
            var user = _dbContext.Users.Find(userId);
            bool isAuthorised = false;
            if (user != null)
            {
                isAuthorised = true;
            }
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
                    LikesCount = article.Likes.Count,
                    IsLiked = isAuthorised? isLiked: false,
                    Comments = article.Comments,
                    SavingsCount = article.Savings.Count,
                    IsSaved = isAuthorised ? isSaved: false,
                });

            }
            articlesViewModel = new ArticlesViewModel
            {
                Articles = articleViewModels,
                Filter = filterViewModel,
                IsUserAuthorised = isAuthorised
            };
            Console.WriteLine(articlesViewModel);

            return View(articlesViewModel);
        }

        public ActionResult Create()
        {
            var categories = _dbContext.Categories.ToList();
            /*ViewBag.Categories = new SelectList { 
                Text = categories.Select(m => m.Name),

            };*/

            ViewBag.Categories = categories;
            /* (categories, "Id", "Name");*/
            Console.WriteLine(ViewBag.Categories);
            return View();
        }
        [HttpPost]
        public IActionResult Create(ArticleInputViewModel articleInputViewModel)
        {
            Console.WriteLine(articleInputViewModel.CategoryId);
            if (!ModelState.IsValid)
            {
                throw new ArgumentException();
            }
            var userId = 2;
            var authorisedUser = _dbContext.Users.Find(userId);
            var categorySearched = _dbContext.Categories.FirstOrDefault(m => m.Id == articleInputViewModel.CategoryId);
            if (categorySearched != null)
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
            return RedirectToAction("Index", "Profile");
        }
    }
}
