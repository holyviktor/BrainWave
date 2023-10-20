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
        [HttpPatch ("/articles/likes")]
        public InteractionsOutputViewModel Likes(InteractionsViewModel interactionsViewModel)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidOperationException("input is not valid.");
            }
            var userId = 2;
            var statusSuccess = _interactionsService.EditLike(interactionsViewModel.Status, interactionsViewModel.ArticleId, userId);
            var likesCount = _dbContext.Likes.Count(m => m.ArticleId == interactionsViewModel.ArticleId);
            var interactionsLikes = new InteractionsOutputViewModel
            {
                StatusSuccess = statusSuccess,
                CountInteractions = likesCount
            };
            Console.WriteLine(likesCount.ToString()+"count likes");

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
            var savingsCount = _dbContext.Savings.Count(m => m.ArticleId == interactionsViewModel.ArticleId);
            var interactionsSavings = new InteractionsOutputViewModel
            {
                StatusSuccess = statusSuccess,
                CountInteractions = savingsCount
            };
            Console.WriteLine(savingsCount.ToString() + "count savings");

            return interactionsSavings;
        }

        [HttpPatch("/articles/comment")]
        public CommentsViewModel Comment(CommentInputViewModel commentViewModel)
        {
            Console.WriteLine("here");
            if (!ModelState.IsValid)
            {
                throw new InvalidOperationException("input is not valid.");
            }
            var userId = 2;
            var statusSuccess = _interactionsService.AddComment(commentViewModel.ArticleId, userId, commentViewModel.Comment);
            var commentsAll = _dbContext.Comments.Where(m => m.ArticleId == commentViewModel.ArticleId).OrderBy(m=>m.Date).ToList();
            var authorisedUser = _dbContext.Users.FirstOrDefault(m => m.Id == userId);
            List<CommentViewModel> commentView = new List<CommentViewModel>();
            if (authorisedUser == null) {
                throw new InvalidOperationException("not authorised");
            }
            var user = new UserViewModel {
                Id= userId,
                Name = authorisedUser.Name,
                Surname= authorisedUser.Surname,
                Photo = authorisedUser.Photo,
            };
            foreach (var comment in commentsAll) {
                commentView.Add(new CommentViewModel
                {
                    Id = comment.Id,
                    Text= comment.Text,
                    User = user
                });

            }
            var commentsView = new CommentsViewModel
            {
                Comments = commentView,
                CommentsCount= commentsAll.Count,
                Status = statusSuccess,
            };
            Console.WriteLine(commentsAll);
            Console.WriteLine(commentsAll.Count);
            Console.WriteLine(statusSuccess);

            return commentsView;

        }

        [HttpDelete ("/articles/comment/delete")]
        public CommentsViewModel CommentDelete(CommentDeleteViewModel commentViewModel)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidOperationException("input is not valid.");
            }
            var userId = 2;
            var statusSuccess = _interactionsService.DeleteComment(commentViewModel.ArticleId, userId, commentViewModel.CommentId);
            var commentsAll = _dbContext.Comments.Where(m => m.ArticleId == commentViewModel.ArticleId).OrderBy(m => m.Date).ToList();
            var authorisedUser = _dbContext.Users.FirstOrDefault(m => m.Id == userId);
            List<CommentViewModel> commentView = new List<CommentViewModel>();
            if (authorisedUser == null)
            {
                throw new InvalidOperationException("not authorised");
            }
            var user = new UserViewModel
            {
                Id = userId,
                Name = authorisedUser.Name,
                Surname = authorisedUser.Surname,
                Photo = authorisedUser.Photo,
            };
            foreach (var comment in commentsAll)
            {
                commentView.Add(new CommentViewModel
                {
                    Id = comment.Id,
                    Text = comment.Text,
                    User = user
                });

            }
            var commentsView = new CommentsViewModel
            {
                Comments = commentView,
                CommentsCount = commentsAll.Count,
                Status = statusSuccess,
            };

            return commentsView;

        }
    }
}
