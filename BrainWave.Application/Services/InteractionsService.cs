using BrainWave.Core;
using BrainWave.Core.Entities;
using BrainWave.Core.Interfaces;
using BrainWave.Infrastructure.Data;

namespace BrainWave.Application.Services
{
    public class InteractionsService:InteractionsInterface
    {
        private readonly BrainWaveDbContext _dbContext;
        public InteractionsService(BrainWaveDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool EditLike(bool status, int idArticle, int idUser)
        {
            bool statusSuccess = false;
            var authorisedUser = _dbContext.Users.FirstOrDefault(m => m.Id == idUser);
            var articleCurrent = _dbContext.Articles.FirstOrDefault(m => m.Id == idArticle);

            if (authorisedUser == null || articleCurrent == null)
            { 
                throw new ArgumentException(); 
            }else {
                var likeCurrent = _dbContext.Likes
                    .Where(m => m.UserId == authorisedUser.Id)
                    .FirstOrDefault(m => m.ArticleId == articleCurrent.Id);
                if (status)
                {
                    if (likeCurrent == null){
                        var likeNew = new Like
                        {
                            User = authorisedUser,
                            Article = articleCurrent
                        };
                        _dbContext.Add(likeNew);
                        statusSuccess = true;
                    }
                }
                else
                {
                    if (likeCurrent != null)
                    {
                        _dbContext.Remove(likeCurrent);
                        statusSuccess = true;
                    }
                }
                _dbContext.SaveChanges();

            }

            return statusSuccess;
        }

        public bool EditSaving(bool status, int idArticle, int idUser)
        {
            var authorisedUser = _dbContext.Users.FirstOrDefault(m => m.Id == idUser);
            var articleCurrent = _dbContext.Articles.FirstOrDefault(m => m.Id == idArticle);
            bool statusSuccess = false;
            if (authorisedUser != null && articleCurrent != null)
            {
                var savingCurrent = _dbContext.Savings
                    .Where(m => m.UserId == authorisedUser.Id)
                    .FirstOrDefault(m => m.ArticleId == articleCurrent.Id);
                if (status)
                {
                    if (savingCurrent is null)
                    {
                        var savingNew = new Saving
                        {
                            User = authorisedUser,
                            Article = articleCurrent
                        };
                        _dbContext.Add(savingNew);
                        statusSuccess = true;
                    }

                }
                else
                {
                    if (savingCurrent != null)
                    {
                        _dbContext.Remove(savingCurrent);
                        statusSuccess = true;
                    }

                }
                _dbContext.SaveChanges();
            }

            return statusSuccess;
        }

        public bool AddComment(int idArticle, int idUser, string comment)
        {

            bool statusSuccess = false;
            var authorisedUser = _dbContext.Users.FirstOrDefault(m => m.Id == idUser);
            var articleCurrent = _dbContext.Articles.FirstOrDefault(m => m.Id == idArticle);
            if (authorisedUser != null && articleCurrent != null)
            {
                var commentNew = new Comment
                {
                    User = authorisedUser,
                    Article = articleCurrent,
                    Text = comment,
                    Date = DateTime.Now
                };
                _dbContext.Add(commentNew);
                _dbContext.SaveChanges();
                statusSuccess = true;
            }
            return statusSuccess;
        }

        public bool DeleteComment(int idArticle, int idUser, int idComment)
        {
            bool statusSuccess = false;
            var authorisedUser = _dbContext.Users.FirstOrDefault(m => m.Id == idUser);
            var articleCurrent = _dbContext.Articles.FirstOrDefault(m => m.Id == idArticle);
            if (authorisedUser != null && articleCurrent != null)
            {
                
                var commentCurrent = _dbContext.Comments.Where(m => m.Id == idComment).FirstOrDefault(m=>m.UserId==idUser || authorisedUser.Articles.Contains(m.Article));
                if (commentCurrent != null)
                {
                    _dbContext.Remove(commentCurrent);
                    _dbContext.SaveChanges();
                    statusSuccess = true;
                }
            }
            return statusSuccess;
        }

    }
}