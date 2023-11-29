using AutoMapper;
using BrainWave.Application.Services;
using BrainWave.Infrastructure.Data;
using BrainWave.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BrainWave.WebUI.Controllers
{
    [Authorize(Roles = "Moderator")]
    public class ModeratorController : Controller
    {
        private readonly BrainWaveDbContext _dbContext;
        private readonly IMapper _mapper;

        public ModeratorController(BrainWaveDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            return View();
        }

        public IActionResult Complaints()
        {
            var statuses = _dbContext.StatusComplaints.ToList();
            ViewBag.Statuses = statuses;
            var articleComplaints = _dbContext.ArticleComplaints
                .Include(c => c.Complaints)
                .ThenInclude(c => c.Reason)
                .Include(c => c.Article)
                .ToList();
            var complaints = _mapper.Map<List<ArticleComplaintsViewModel>>(articleComplaints);
            for (int i = 0;i<articleComplaints.Count; i++)
            {
                complaints[i].Complaints = _mapper.Map<List<ComplaintViewModel>>(articleComplaints[i].Complaints);
            }
            var complaintsView = new ComplaintsStatusViewModel
            {
                ArticleComplaints = complaints
            };

            return View(complaintsView);
        }

        public IActionResult ChangeStatus(ComplaintsStatusViewModel complaintsStatusViewModel)
        {
            var articleComplaint = _dbContext.ArticleComplaints
                .Include(c => c.Article)
                .FirstOrDefault(c => c.ArticleId == complaintsStatusViewModel.ArticleComplaintId);
            var status = _dbContext.StatusComplaints.FirstOrDefault(c => c.Id == complaintsStatusViewModel.StatusId);

            if (articleComplaint == null || status == null)
            {
                throw new InvalidOperationException();
            }
            if (status.Id == 2)
            {
                articleComplaint.Article.IsAvailable = false;
            }
            else if (status.Id == 3)
            {
                articleComplaint.Article.IsAvailable = true;
            }
            articleComplaint.StatusId = status.Id;
            _dbContext.SaveChanges();
            return RedirectToAction("Complaints");
        }
    }
}
