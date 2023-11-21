using AutoMapper;
using BrainWave.Core.Entities;
using BrainWave.Infrastructure.Data;
using BrainWave.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace BrainWave.WebUI.Controllers
{
    public class PaymentController : Controller
    {
        private readonly BrainWaveDbContext _dbContext;
        private readonly IMapper _mapper;
        public PaymentController(BrainWaveDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        [Authorize]
        [Route("payment/{articleId:int}")]
        public ActionResult Index(int articleId)
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
            var article = _dbContext.Articles.Where(a=>a.IsAvailable)
                .Include(a=>a.User).FirstOrDefault(a=>a.Id == articleId);
            if (article == null)
            {
                return NotFound();
            }
            if (article.User == user)
            {
                return RedirectToAction("Index", "Profile");
            }
            if (article.Price == 0)
            {
                return BadRequest();
            }
            var isPaid = _dbContext.Payments.Any(p => p.UserId == user.Id && p.ArticleId == articleId);
            if (isPaid)
            {
                return RedirectToAction("Wallet", "Profile");
            }
            int offset = Math.Min(100, article.Text.Length);
            article.Text = article.Text.Substring(0, offset) + "...";
            var articleView = _mapper.Map<ArticleViewModel>(article);
            articleView.User = article.User;
            return View(articleView);
        }


        [Route("payment/{articleId}")]
        [HttpPost]
        public IActionResult Pay(int articleId)
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
            var article = _dbContext.Articles.Where(a=>a.IsAvailable).Include(a => a.User).SingleOrDefault(a => a.Id == articleId);
            if (article == null)
            {
                return NotFound();
            }
            if (article.Price == 0)
            {
                return BadRequest();
            }
            var payment = new Payment
            {
                ArticleId = article.Id,
                UserId = user.Id,
                Cost = article.Price,
                IsSuccess = true
            };
            article.User.EarnedMoney += article.Price;
            _dbContext.Payments.Add(payment);
            _dbContext.SaveChanges();
            return RedirectToAction("Wallet", "Profile");
        }
    }
}
