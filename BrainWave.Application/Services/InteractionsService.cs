using BrainWave.Core;
using BrainWave.Core.DTOs;
using BrainWave.Core.Entities;
using BrainWave.Core.Interfaces;
using BrainWave.Infrastructure.Data;
using System.Net;
using System.Web.Http;

namespace BrainWave.Application.Services
{
    public class InteractionsService : IInteractionsService
    {
        private readonly BrainWaveDbContext _dbContext;

        public InteractionsService(BrainWaveDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool EditLike(LikesSavingsDTO likesSavingsDTO)
        {
            bool statusSuccess = false;
            var authorisedUser = _dbContext.Users.FirstOrDefault(m => m.Id == likesSavingsDTO.idUser);
            var currentArticle = _dbContext.Articles.FirstOrDefault(m => m.Id == likesSavingsDTO.idArticle);

            if (authorisedUser == null || currentArticle == null)
            {
                throw new ArgumentException();
            }

            var currentLike = _dbContext.Likes
                .Where(m => m.UserId == authorisedUser.Id)
                .FirstOrDefault(m => m.ArticleId == currentArticle.Id);
            if (likesSavingsDTO.status)
            {
                if (currentLike == null)
                {
                    var likeNew = new Like
                    {
                        User = authorisedUser,
                        Article = currentArticle
                    };
                    _dbContext.Add(likeNew);
                    statusSuccess = true;
                }
            }
            else
            {
                if (currentLike != null)
                {
                    _dbContext.Remove(currentLike);
                    statusSuccess = true;
                }
            }

            _dbContext.SaveChanges();

            return statusSuccess;
        }

        public bool EditSaving(LikesSavingsDTO likesSavingsDTO)
        {
            var authorisedUser = _dbContext.Users.FirstOrDefault(m => m.Id == likesSavingsDTO.idUser);
            var currentArticle = _dbContext.Articles.FirstOrDefault(m => m.Id == likesSavingsDTO.idArticle);
            bool statusSuccess = false;
            if (authorisedUser != null && currentArticle != null)
            {
                var currentSaving = _dbContext.Savings
                    .Where(m => m.UserId == authorisedUser.Id)
                    .FirstOrDefault(m => m.ArticleId == currentArticle.Id);
                if (likesSavingsDTO.status)
                {
                    if (currentSaving is null)
                    {
                        var savingNew = new Saving
                        {
                            User = authorisedUser,
                            Article = currentArticle
                        };
                        _dbContext.Add(savingNew);
                        statusSuccess = true;
                    }
                }
                else
                {
                    if (currentSaving != null)
                    {
                        _dbContext.Remove(currentSaving);
                        statusSuccess = true;
                    }
                }

                _dbContext.SaveChanges();
            }
            else
            {
                throw new ArgumentException();
            }

            return statusSuccess;
        }

        public Comment AddComment(AddCommentDTO commentDTO)
        {
            var authorisedUser = _dbContext.Users.FirstOrDefault(m => m.Id == commentDTO.idUser);
            var articleCurrent = _dbContext.Articles.FirstOrDefault(m => m.Id == commentDTO.idArticle);
            Comment commentNew;
            if (authorisedUser != null && articleCurrent != null)
            {
                commentNew = new Comment
                {
                    User = authorisedUser,
                    Article = articleCurrent,
                    Text = commentDTO.comment,
                    Date = DateTime.Now
                };
                _dbContext.Add(commentNew);
                _dbContext.SaveChanges();
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return commentNew;
        }

        public bool DeleteComment(DeleteCommentDTO commentDTO)
        {
            bool statusSuccess = false;
            var authorisedUser = _dbContext.Users.FirstOrDefault(m => m.Id == commentDTO.idUser);
            var currentArticle = _dbContext.Articles.FirstOrDefault(m => m.Id == commentDTO.idArticle);
            if (authorisedUser != null && currentArticle != null)
            {
                var currentComment = _dbContext.Comments.Where(m => m.Id == commentDTO.idComment)
                    .FirstOrDefault(m => m.UserId == commentDTO.idUser || authorisedUser.Articles.Contains(m.Article));
                if (currentComment != null)
                {
                    _dbContext.Remove(currentComment);
                    _dbContext.SaveChanges();
                    statusSuccess = true;
                }
            }

            return statusSuccess;
        }
    }
}