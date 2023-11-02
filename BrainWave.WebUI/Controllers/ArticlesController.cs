using AutoMapper;
using BrainWave.Application.Services;
using BrainWave.Core.Entities;
using BrainWave.Infrastructure.Data;
using BrainWave.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Xml.Linq;

namespace BrainWave.WebUI.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly ILogger<ArticlesController> _logger;
        private readonly BrainWaveDbContext _dbContext;
        private readonly IStringLocalizer<ArticlesController> _stringLocalizer;
        private readonly FiltersService _filtersService;
        private readonly IMapper _mapper;

        public ArticlesController(ILogger<ArticlesController> logger, BrainWaveDbContext dbContext, IStringLocalizer<ArticlesController> stringLocalizer, IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _stringLocalizer = stringLocalizer;
            _filtersService = new FiltersService(dbContext);
            _mapper = mapper;
        }

        public ActionResult Index(ArticlesViewModel? articlesViewModel)
        {
            List<Article> articles;
            FilterViewModel filterViewModel = new FilterViewModel
            {
                Categories = _dbContext.Categories.ToList(),
            };
            int userId = 2;
            List<ArticleViewModel> articleViewModels = new();
            var user = _dbContext.Users.Find(userId);
            bool isAuthorised = user != null;

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
                    .Include(c=>c.Likes)
                    .Include(c=>c.Savings)
                    .ToList();
            }
            
            foreach (Article article in articles)
            {
                var isLiked = article.Likes.Any(x => x.UserId == userId);
                var isSaved = article.Savings.Any(x => x.UserId == userId);

                var articleViewModel = _mapper.Map<ArticleViewModel>(article);
                articleViewModel.LikesCount = article.Likes.Count(x => x.ArticleId == article.Id);
                articleViewModel.IsLiked = isAuthorised && isLiked;
                articleViewModel.Comments = article.Comments?.OrderByDescending(c => c.Date).ToList();
                articleViewModel.SavingsCount = article.Savings.Count(x => x.ArticleId == article.Id);
                articleViewModel.IsSaved = isAuthorised && isSaved;
                articleViewModels.Add(articleViewModel);

            }
            articlesViewModel = new ArticlesViewModel
            {
                Articles = articleViewModels,
                Filter = filterViewModel,
                IsUserAuthorised = isAuthorised
            };
            return View(articlesViewModel);
        }
        [Authorize]
        public ActionResult Create()
        {
            var categories = _dbContext.Categories.ToList();
            ViewBag.Categories = categories;
            return View();
        }
        [HttpPost]
        [Authorize]
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
                var articleNew = _mapper.Map<Article>(articleInputViewModel);
                articleNew.Date = DateTime.Now;
                articleNew.UserId= userId;
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
