using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using AutoMapper;
using BrainWave.Application.Services;
using BrainWave.Core.DTOs;
using BrainWave.Infrastructure.Data;
using IdentityApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace IdentityApi.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

        [HttpPatch("/articles/likes")]
        public InteractionsOutputViewModel Likes(InteractionsViewModel interactionsViewModel)
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
                throw new InvalidOperationException("input is not valid.");
            }
            var statusSuccess = _interactionsService.EditLike(new LikesSavingsDTO(interactionsViewModel.Status, interactionsViewModel.ArticleId, user.Id));
            if (!statusSuccess)
            {
                throw new System.Web.Http.HttpResponseException(HttpStatusCode.NotFound);
            }
            var likesCount = _dbContext.Likes.Count(m => m.ArticleId == interactionsViewModel.ArticleId);
            var interactionsLikes = new InteractionsOutputViewModel
            {
                CountInteractions = likesCount
            };

            return interactionsLikes;
        }
        [HttpPatch("/articles/savings")]
        public InteractionsOutputViewModel Savings(InteractionsViewModel interactionsViewModel)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidOperationException("input is not valid.");
            }
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
            var statusSuccess = _interactionsService.EditSaving(new LikesSavingsDTO(interactionsViewModel.Status, interactionsViewModel.ArticleId, user.Id));
            if (!statusSuccess)
            {
                throw new System.Web.Http.HttpResponseException(HttpStatusCode.NotFound);
            }
            var savingsCount = _dbContext.Savings.Count(m => m.ArticleId == interactionsViewModel.ArticleId);
            var interactionsSavings = new InteractionsOutputViewModel
            {
                CountInteractions = savingsCount
            };
            return interactionsSavings;
        }

        [HttpPatch("/articles/comment")]
        public CommentViewModel Comment(CommentInputViewModel commentViewModel)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidOperationException("input is not valid.");
            }
            var userTag = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("Name")?.Value;
            if (userTag == null)
            {
                throw new ArgumentException();
            }
            var authorisedUser = _dbContext.Users.FirstOrDefault(x => x.Tag == userTag);
            if (authorisedUser == null)
            {
                throw new InvalidOperationException();
            }
            var comment = _interactionsService.AddComment(new AddCommentDTO(commentViewModel.ArticleId, authorisedUser.Id, commentViewModel.Comment));
            var user = _mapper.Map<UserViewModel>(authorisedUser);

            var addedComment = new CommentViewModel
            {
                Id = comment.Id,
                Text = comment.Text,
                User = user
            };
            return addedComment;
        }

        [HttpDelete("/articles/comment/delete")]
        public CommentDeleteViewModel CommentDelete(CommentDeleteViewModel commentViewModel)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidOperationException("input is not valid.");
            }
            var userTag = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("Name")?.Value;
            if (userTag == null)
            {
                throw new ArgumentException();
            }
            var authorisedUser = _dbContext.Users.FirstOrDefault(x => x.Tag == userTag);
            if (authorisedUser == null)
            {
                throw new InvalidOperationException();
            }
            var statusSuccess = _interactionsService.DeleteComment(new DeleteCommentDTO(commentViewModel.ArticleId, authorisedUser.Id, commentViewModel.CommentId));
            if (!statusSuccess)
            {
                throw new System.Web.Http.HttpResponseException(HttpStatusCode.NotFound);
            }
            return commentViewModel;

        }
    }
}
