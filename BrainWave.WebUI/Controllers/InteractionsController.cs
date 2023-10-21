using System.Net;
using System.Net.Mail;
using System.Text.Json;
/*using System.Web.Http;*/
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

        [HttpPatch ("/articles/likes")]
        public InteractionsOutputViewModel Likes(InteractionsViewModel interactionsViewModel)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidOperationException("input is not valid.");
            }
            var userId = 2;
            var statusSuccess = _interactionsService.EditLike(interactionsViewModel.Status, interactionsViewModel.ArticleId, userId);
            if (!statusSuccess) {
                throw new System.Web.Http.HttpResponseException(HttpStatusCode.NotFound);
            }
            var likesCount = _dbContext.Likes.Count(m => m.ArticleId == interactionsViewModel.ArticleId);
            var interactionsLikes = new InteractionsOutputViewModel
            {
                CountInteractions = likesCount
            };

            return (interactionsLikes);
        }
        [HttpPatch("/articles/savings")]
        public InteractionsOutputViewModel Savings(InteractionsViewModel interactionsViewModel)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidOperationException("input is not valid.");
            }
            var userId = 2;
            var statusSuccess = _interactionsService.EditSaving(interactionsViewModel.Status, interactionsViewModel.ArticleId, userId);
            if (!statusSuccess)
            {
                throw new System.Web.Http.HttpResponseException(HttpStatusCode.NotFound);
            }
            var savingsCount = _dbContext.Savings.Count(m => m.ArticleId == interactionsViewModel.ArticleId);
            var interactionsSavings = new InteractionsOutputViewModel
            {
                CountInteractions = savingsCount
            };
            Console.WriteLine(savingsCount.ToString() + "count savings");

            return interactionsSavings;
        }

        [HttpPatch("/articles/comment")]
        public CommentViewModel Comment(CommentInputViewModel commentViewModel)
        {
            Console.WriteLine("here");
            if (!ModelState.IsValid)
            {
                throw new InvalidOperationException("input is not valid.");
            }
            var userId = 2;
            var authorisedUser = _dbContext.Users.FirstOrDefault(m => m.Id == userId);
            if (authorisedUser == null)
            {
                throw new InvalidOperationException("not authorised");
            }
            var comment = _interactionsService.AddComment(commentViewModel.ArticleId, userId, commentViewModel.Comment);
            var commentsAll = _dbContext.Comments.Where(m => m.ArticleId == commentViewModel.ArticleId).OrderBy(m=>m.Date).ToList();
                      
            var user = new UserViewModel {
                Id= userId,
                Name = authorisedUser.Name,
                Surname= authorisedUser.Surname,
                Photo = authorisedUser.Photo,
            };

            var commentViewModelNew = new CommentViewModel
            {
                Id = comment.Id,
                Text = comment.Text,
                User = user
            };
            

            return commentViewModelNew;
        }

        [HttpDelete ("/articles/comment/delete")]
        public CommentDeleteViewModel CommentDelete(CommentDeleteViewModel commentViewModel)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidOperationException("input is not valid.");
            }
            var userId = 2;
            var authorisedUser = _dbContext.Users.FirstOrDefault(m => m.Id == userId);
            if (authorisedUser == null)
            {
                throw new InvalidOperationException("not authorised");
            }
            var statusSuccess = _interactionsService.DeleteComment(commentViewModel.ArticleId, userId, commentViewModel.CommentId);
            if (!statusSuccess)
            {
                throw new System.Web.Http.HttpResponseException(HttpStatusCode.NotFound);
            }
            var commentsAll = _dbContext.Comments.Where(m => m.ArticleId == commentViewModel.ArticleId).OrderBy(m => m.Date).ToList();
            
            return commentViewModel;

        }
    }
}
