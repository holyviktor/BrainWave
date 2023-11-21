using AutoMapper;
using BrainWave.Core.Entities;
using BrainWave.Infrastructure.Data;
using BrainWave.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BrainWave.WebUI.Controllers
{
    public class ComplaintsController : Controller
    {
        private readonly BrainWaveDbContext _dbContext;
        private readonly IMapper _mapper;
        public ComplaintsController(BrainWaveDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        

        [Authorize(Roles = "User")]
        [Route("/complaints/create/{articleId:int}")]
        public ActionResult Create(int articleId)
        {
            var article = _dbContext.Articles.Where(a=>a.IsAvailable).Include(a=>a.Category).SingleOrDefault(a=>a.Id == articleId);
            if (article == null)
            {
                throw new InvalidOperationException();
            }
            var reasons = _dbContext.ReasonComplaints.ToList();
            ViewBag.Reasons = reasons;
            ViewBag.Article = article;
            return View();
        }
        [Authorize(Roles = "User")]
        public ActionResult Add(ComplaintInputViewModel complaintInputViewModel)
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
                throw new InvalidOperationException();
            }
            var article = _dbContext.Articles.Where(a => a.IsAvailable).FirstOrDefault(a=>a.Id == complaintInputViewModel.ArticleId);
            if (article == null)
            {
                throw new InvalidOperationException();
            }
            var status = _dbContext.StatusComplaints.Find(1);
            if (status == null)
            {
                throw new InvalidOperationException();
            }

            var complaint = _mapper.Map<Complaint>(complaintInputViewModel);
            complaint.UserId = user.Id;
            complaint.StatusId = status.Id;
                /*new Complaint
            {
                ArticleId = complaintInputViewModel.ArticleId,
                ReasonId = complaintInputViewModel.ReasonId,
                UserId = user.Id,
                Text = complaintInputViewModel.Text,
                StatusId = status.Id
            };*/
            _dbContext.Complaints.Add(complaint);
            _dbContext.SaveChanges();
            return RedirectToAction("Complaints", "Profile");
        }
    }
}
