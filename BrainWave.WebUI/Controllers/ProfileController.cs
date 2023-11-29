using AutoMapper;
using BrainWave.Core.Entities;
using BrainWave.Infrastructure.Data;
using BrainWave.WebUI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BrainWave.WebUI.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly BrainWaveDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProfileController(BrainWaveDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ActionResult> Index()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token") ?? "";
            HttpContext.Response.Cookies.Append("token", accessToken);
            var userTag = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("Name")?.Value;
            if (userTag == null)
            {
                throw new ArgumentException();
            }

            var user = _dbContext.Users.FirstOrDefault(x => x.Tag == userTag);
            ProfileViewModel profileViewModel;
            if (user != null)
            {
                var articles = _dbContext.Articles
                    .Where(x => x.UserId == user.Id)
                    .Where(a => a.IsAvailable)
                    .Include(c => c.Comments)
                    .ThenInclude(c => c.User)
                    .Include(c => c.User)
                    .Include(c => c.Likes)
                    .Include(c => c.Savings)
                    .OrderByDescending(x => x.Date).ToList()
                    .Select(c => new
                    {
                        Article = c,
                        LikesCount = c.Likes?.Count(),
                        SavingsCount = c.Savings?.Count(),
                        IsLiked = c.Likes?.Any(like => like.UserId == user.Id),
                        IsSaved = c.Savings?.Any(saving => saving.UserId == user.Id)
                    });
                var followers = _dbContext.Followings.Count(x => x.FollowingUserId == user.Id);
                var followings = _dbContext.Followings.Count(x => x.UserId == user.Id);
                List<ArticleViewModel> articleViewModels = new();
                foreach (var article in articles)
                {
                    var articleViewModel = _mapper.Map<ArticleViewModel>(article.Article);
                    articleViewModel.LikesCount = article.LikesCount ?? 0;
                    articleViewModel.IsLiked = article.IsLiked ?? false;
                    articleViewModel.Comments = article.Article.Comments?.OrderByDescending(c => c.Date).ToList();
                    articleViewModel.SavingsCount = article.SavingsCount ?? 0;
                    articleViewModel.IsSaved = article.IsSaved ?? false;
                    articleViewModels.Add(articleViewModel);
                }

                profileViewModel = new ProfileViewModel
                {
                    User = user,
                    Followers = followers,
                    Followings = followings,
                    Articles = articleViewModels
                };
            }
            else
            {
                throw new ArgumentException();
            }

            return View(profileViewModel);
        }

        public ActionResult Edit()
        {
            var userTag = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("Name")?.Value;
            if (userTag == null)
            {
                throw new ArgumentException();
            }

            var user = _dbContext.Users.FirstOrDefault(x => x.Tag == userTag);
            if (user == null)
            {
                return NotFound();
            }

            ProfileInputViewModel profileInputViewModel = _mapper.Map<ProfileInputViewModel>(user);
            return View(profileInputViewModel);
        }

        [HttpPost]
        public IActionResult EditUser(ProfileInputViewModel profileInputViewModel)
        {
            var userTag = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("Name")?.Value;
            if (userTag == null)
            {
                throw new ArgumentException();
            }

            var user = _dbContext.Users.FirstOrDefault(x => x.Tag == userTag);
            if (user == null)
            {
                throw new ArgumentException();
            }
            if (!ModelState.IsValid)
            {
                ProfileInputViewModel profileModel = _mapper.Map<ProfileInputViewModel>(user);
                return View("Edit", profileModel);
            }
            user.Name = profileInputViewModel.Name;
            user.Surname = profileInputViewModel.Surname;
            user.Description = profileInputViewModel.Description;
            _dbContext.Update(user);
            _dbContext.SaveChanges();
            return RedirectToAction("Index", "Profile");
        }

        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }

        public async Task<IActionResult> Liked()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token") ?? "";
            HttpContext.Response.Cookies.Append("token", accessToken);
            var userTag = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("Name")?.Value;
            if (userTag == null)
            {
                throw new ArgumentException();
            }

            var user = _dbContext.Users.FirstOrDefault(x => x.Tag == userTag);
            if (user == null)
            {
                throw new ArgumentException();
            }

            var likes = _dbContext.Likes
                .Include(l => l.Article)
                .ThenInclude(a => a.Comments)
                .ThenInclude(c => c.User)
                .Include(l => l.Article)
                .ThenInclude(a => a.Likes)
                .Include(l => l.Article)
                .ThenInclude(a => a.Savings)
                .Where(l => l.Article.IsAvailable)
                .Where(l => l.UserId == user.Id);
            List<ArticleViewModel> articleViewModels = new();
            foreach (var like in likes)
            {
                if (like.Article.Price > 0)
                {
                    int offset = Math.Min(10, like.Article.Text.Length);
                    like.Article.Text = like.Article.Text.Substring(0, offset) + "...";
                }

                var isSaved = like.Article.Savings.Any(x => x.UserId == user.Id);
                var likedArticle = _mapper.Map<ArticleViewModel>(like.Article);
                likedArticle.IsLiked = true;
                likedArticle.IsSaved = isSaved;
                likedArticle.LikesCount = like.Article.Likes.Count(x => x.ArticleId == like.Article.Id);
                likedArticle.Comments = like.Article.Comments.OrderByDescending(c => c.Date).ToList();
                likedArticle.SavingsCount = like.Article.Savings.Count(x => x.ArticleId == like.Article.Id);

                articleViewModels.Add(likedArticle);
            }

            return View(articleViewModels);
        }

        public async Task<IActionResult> Saved()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token") ?? "";
            HttpContext.Response.Cookies.Append("token", accessToken);
            var userTag = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("Name")?.Value;
            if (userTag == null)
            {
                throw new ArgumentException();
            }

            var user = _dbContext.Users.FirstOrDefault(x => x.Tag == userTag);
            if (user == null)
            {
                throw new ArgumentException();
            }

            var savings = _dbContext.Savings
                .Include(l => l.Article)
                .ThenInclude(a => a.Comments)
                .ThenInclude(c => c.User)
                .Include(l => l.Article)
                .ThenInclude(a => a.Likes)
                .Include(l => l.Article)
                .ThenInclude(a => a.Savings)
                .Where(l => l.Article.IsAvailable)
                .Where(l => l.UserId == user.Id);
            List<ArticleViewModel> articleViewModels = new();
            foreach (var saving in savings)
            {
                if (saving.Article.Price > 0)
                {
                    int offset = Math.Min(10, saving.Article.Text.Length);
                    saving.Article.Text = saving.Article.Text.Substring(0, offset) + "...";
                }

                var isSaved = true;
                var likedArticle = _mapper.Map<ArticleViewModel>(saving.Article);
                likedArticle.IsLiked = saving.Article.Likes.Any(x => x.UserId == user.Id);
                likedArticle.IsSaved = isSaved;
                likedArticle.LikesCount = saving.Article.Likes.Count(x => x.ArticleId == saving.Article.Id);
                likedArticle.Comments = saving.Article.Comments.OrderByDescending(c => c.Date).ToList();
                likedArticle.SavingsCount = saving.Article.Savings.Count(x => x.ArticleId == saving.Article.Id);

                articleViewModels.Add(likedArticle);
            }

            return View(articleViewModels);
        }

        public async Task<ActionResult> Wallet()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token") ?? "";
            HttpContext.Response.Cookies.Append("token", accessToken);
            FilterViewModel filterViewModel = new FilterViewModel
            {
                Categories = _dbContext.Categories.ToList(),
            };
            var userTag = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("Name")?.Value;
            if (userTag == null)
            {
                throw new ArgumentException();
            }

            var user = _dbContext.Users.FirstOrDefault(x => x.Tag == userTag);
            List<ArticleViewModel> articleViewModels = new();
            bool isAuthorised = user != null;

            var payments = _dbContext.Payments.Where(x => x.User == user)
                .Include(p => p.Article)
                .ThenInclude(c => c.Comments)
                .ThenInclude(c => c.User)
                .Include(p => p.Article)
                .ThenInclude(c => c.User)
                .Include(p => p.Article)
                .ThenInclude(c => c.Likes)
                .Include(p => p.Article)
                .ThenInclude(c => c.Savings)
                .ToList();

            foreach (Payment payment in payments)
            {
                var isLiked = payment.Article.Likes.Any(x => x.UserId == user.Id);
                var isSaved = payment.Article.Savings.Any(x => x.UserId == user.Id);

                var articleViewModel = _mapper.Map<ArticleViewModel>(payment.Article);
                articleViewModel.LikesCount = payment.Article.Likes.Count(x => x.ArticleId == payment.Article.Id);
                articleViewModel.IsLiked = isAuthorised && isLiked;
                articleViewModel.Comments = payment.Article.Comments?.OrderByDescending(c => c.Date).ToList();
                articleViewModel.SavingsCount = payment.Article.Savings.Count(x => x.ArticleId == payment.Article.Id);
                articleViewModel.IsSaved = isAuthorised && isSaved;
                articleViewModels.Add(articleViewModel);
            }

            var articlesViewModel = new ArticlesViewModel
            {
                Articles = articleViewModels,
                Filter = filterViewModel,
                IsUserAuthorised = isAuthorised
            };
            ViewBag.EarnedMoney = user.EarnedMoney;
            return View(articlesViewModel);
        }

        public ActionResult Complaints()
        {
            var userTag = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("Name")?.Value;
            if (userTag == null)
            {
                throw new ArgumentException();
            }

            var user = _dbContext.Users.FirstOrDefault(x => x.Tag == userTag);
            if (user == null)
            {
                throw new ArgumentException();
            }
            var articleComplaints = _dbContext.ArticleComplaints
                
                .Include(ac => ac.Complaints)
                .ThenInclude(c=>c.Reason)
                .Where(ac => ac.Complaints.Any(c => c.UserId == user.Id))
                .Include(ac => ac.Status)
                .Include(ac => ac.Article)
                .ToList();
            var articleComplaintsView = _mapper.Map<List<ArticleComplaintsViewModel>>(articleComplaints);
            for (int i=0; i < articleComplaintsView.Count; i++)
            {
                articleComplaintsView[i].Complaints = _mapper.Map<List<ComplaintViewModel>>(articleComplaints[i].Complaints);
            }
            return View(articleComplaintsView);
        }
    }
}