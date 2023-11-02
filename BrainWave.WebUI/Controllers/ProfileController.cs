using AutoMapper;
using BrainWave.Core.Entities;
using BrainWave.Infrastructure.Data;
using BrainWave.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrainWave.WebUI.Controllers
{
    public class ProfileController : Controller
    {
        private readonly BrainWaveDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProfileController(BrainWaveDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        [Authorize]
        public ActionResult Index()
        {
            var userId = 2;
            var user = _dbContext.Users.Find(userId);
            ProfileViewModel profileViewModel;
            if (user != null)
            {
                var articles = _dbContext.Articles
                    .Where(x => x.UserId == user.Id)
                    .Include(c => c.Comments)
                    .Include(c => c.User)
                    .OrderByDescending(x => x.Date).ToList()
                    .Select(c => new
                    {
                        Article = c,
                        LikesCount = c.Likes?.Count(),
                        SavingsCount = c.Savings?.Count(),
                        IsLiked = c.Likes?.Any(like => like.UserId == userId),
                        IsSaved = c.Savings?.Any(saving => saving.UserId == userId)
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
        [Authorize]
        public ActionResult Edit()
        {
            var userId = 2;
            var user = _dbContext.Users.Find(userId);
            if (user == null)
            {
                return NotFound();
            }
            ProfileInputViewModel profileInputViewModel = _mapper.Map<ProfileInputViewModel>(user);
            return View(profileInputViewModel);
        }
        [HttpPost]
        [Authorize]
        public IActionResult EditUser(ProfileInputViewModel profileInputViewModel)
        {
            Console.WriteLine("here");
            var userId = 2;
            var user = _dbContext.Users.Find(userId);
            if (user == null)
            {
                throw new ArgumentException();
            }
            if (!ModelState.IsValid)
            {
                ProfileInputViewModel profileModel = _mapper.Map<ProfileInputViewModel>(user);
                return View("Edit", profileModel);
            }
            else
            {
                user.Name = profileInputViewModel.Name;
                user.Surname = profileInputViewModel.Surname;
                user.Tag = profileInputViewModel.Tag;
                user.Description = profileInputViewModel.Description;
                user.Photo = profileInputViewModel.Photo;
                user.Password = profileInputViewModel.Password;
                _dbContext.Update(user);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index", "Profile");
        }
        [Authorize]
        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }
    }
}
