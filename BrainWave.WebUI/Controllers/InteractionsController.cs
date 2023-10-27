using System.Net;
using System.Net.Mail;
using System.Text.Json;
using AutoMapper;
/*using System.Web.Http;*/
using BrainWave.Application.Services;
using BrainWave.Core.DTOs;
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
        private readonly IMapper _mapper;

        public InteractionsController(BrainWaveDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _interactionsService = new InteractionsService(dbContext);
            _mapper = mapper;
        }

        [HttpPatch ("/articles/likes")]
        public InteractionsOutputViewModel Likes(InteractionsViewModel interactionsViewModel)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidOperationException("input is not valid.");
            }
            var userId = 2;
            var statusSuccess = _interactionsService.EditLike(new LikesSavingsDTO(interactionsViewModel.Status, interactionsViewModel.ArticleId, userId));
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
            var statusSuccess = _interactionsService.EditSaving(new LikesSavingsDTO(interactionsViewModel.Status, interactionsViewModel.ArticleId, userId));
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
            var comment = _interactionsService.AddComment(new AddCommentDTO(commentViewModel.ArticleId, userId, commentViewModel.Comment));
            var user = _mapper.Map<UserViewModel>(authorisedUser);

            var addedComment = new CommentViewModel
            {
                Id = comment.Id,
                Text = comment.Text,
                User = user
            };
            return addedComment;
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
            var statusSuccess = _interactionsService.DeleteComment(new DeleteCommentDTO(commentViewModel.ArticleId, userId, commentViewModel.CommentId));
            if (!statusSuccess)
            {
                throw new System.Web.Http.HttpResponseException(HttpStatusCode.NotFound);
            }
            var commentsAll = _dbContext.Comments.Where(m => m.ArticleId == commentViewModel.ArticleId).OrderBy(m => m.Date).ToList();
            
            return commentViewModel;

        }
    }
}
