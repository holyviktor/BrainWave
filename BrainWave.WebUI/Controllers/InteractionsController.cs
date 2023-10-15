using System.Text.Json;
using BrainWave.Application.Services;
using BrainWave.Core.Entities;
using BrainWave.Infrastructure.Data;
using BrainWave.WebUI.Models;
using Microsoft.AspNetCore.Mvc;


namespace BrainWave.WebUI.Controllers
{
    [ApiController]
    
    public class InteractionsController : Controller
    {
        private readonly BrainWaveDbContext _dbContext;
        private readonly InteractionsService _interactionsService;

        public InteractionsController(BrainWaveDbContext dbContext)
        {
            _dbContext = dbContext;
            _interactionsService = new InteractionsService(dbContext);
        }
        // GET: InteractionsController
        [HttpPatch ("/articles/likes/{idArticle}")]
        public InteractionsLikesOutputViewModel Likes(int idArticle, InteractionsViewModel interactionsViewModel)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidOperationException("status is not valid.");
            }
            var idUser = 2;
            var statusSuccess = _interactionsService.EditLike(interactionsViewModel.Status, idArticle, idUser);
            var likesCount = _dbContext.Likes.Count(m => m.ArticleId == idArticle);
            var interactionsLikes = new InteractionsLikesOutputViewModel
            {
                StatusSuccess = statusSuccess,
                CountLikes = likesCount
            };

            return (interactionsLikes);
        }
    }
}
