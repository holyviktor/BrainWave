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
            var complaints = _dbContext.Complaints
                .Include(c => c.Article)
                .Include(c => c.Status)
                .Include(c => c.Reason)
                .ToList();

            var complaintsView = new ComplaintsStatusViewModel
            {
                Complaints = _mapper.Map<List<ComplaintViewModel>>(complaints)
            };
            return View(complaintsView);
        }

        public IActionResult ChangeStatus(ComplaintsStatusViewModel complaintsStatusViewModel)
        {
            var complaint = _dbContext.Complaints
                .Include(c=>c.Article)
                .FirstOrDefault(c => c.Id == complaintsStatusViewModel.ComplaintId);
            var status = _dbContext.StatusComplaints.FirstOrDefault(c=>c.Id== complaintsStatusViewModel.StatusId);
            if (complaint == null || status == null) {
                throw new InvalidOperationException();
            }
            if (status.Id == 2)
            {
                complaint.Article.IsAvailable = false;
            }else if (status.Id == 3)
            {
                complaint.Article.IsAvailable = true;
            }
            complaint.StatusId = status.Id;
            _dbContext.SaveChanges();
            return RedirectToAction("Complaints");
        }
    }
}
