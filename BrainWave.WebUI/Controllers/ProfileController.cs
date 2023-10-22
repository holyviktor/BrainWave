using BrainWave.Core.Entities;
using BrainWave.Infrastructure.Data;
using BrainWave.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrainWave.WebUI.Controllers
{
    public class ProfileController : Controller
    {
        private readonly BrainWaveDbContext _dbContext;

        public ProfileController(BrainWaveDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public ActionResult Index()
        {
            var userId = 2;
            var user = _dbContext.Users.Find(userId);
            ProfileViewModel profileViewModel;
            if (user != null) {
                var articles = _dbContext.Articles.Where(x => x.UserId == user.Id)
                    .Include(c => c.Comments)
                    .Include(c => c.Category)
                    .Include(c => c.User).ToList();
                var followers = _dbContext.Followings.Count(x => x.FollowingUserId == user.Id);
                var followings = _dbContext.Followings.Count(x => x.UserId == user.Id);
                List<ArticleViewModel> articleViewModels = new();
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
                        IsLiked = isLiked,
                        Comments = article.Comments,
                        SavingsCount = _dbContext.Savings.Count(x=>x.ArticleId == article.Id),
                        IsSaved = isSaved,
                    });

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
            var userId = 2;
            var user = _dbContext.Users.Find(userId);
            if (user == null)
            {
                return NotFound();
            }
            ProfileInputViewModel profileInputViewModel = new ProfileInputViewModel
            {
                Name = user.Name,
                Surname= user.Surname,
                Tag= user.Tag,
                Description = user.Description,
                Photo = user.Photo,
                Password = user.Password,

            };
            return View(profileInputViewModel);
        }
        [HttpPost]
        public IActionResult EditUser(ProfileInputViewModel profileInputViewModel)
        {
            Console.WriteLine( "here");
            var userId = 2;
            var user = _dbContext.Users.Find(userId);
            if (user == null) { 
                throw new ArgumentException();
            }
            if (!ModelState.IsValid)
            {
                ProfileInputViewModel profileModel = new ProfileInputViewModel
                {
                    Name = user.Name,
                    Surname = user.Surname,
                    Tag = user.Tag,
                    Description = user.Description,
                    Photo = user.Photo,
                    Password = user.Password,

                };
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
    }
}
