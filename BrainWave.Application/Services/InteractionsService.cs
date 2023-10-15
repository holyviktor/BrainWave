using BrainWave.Core;
using BrainWave.Core.Entities;
using BrainWave.Infrastructure.Data;

namespace BrainWave.Application.Services
{
    public class InteractionsService
    {
        private readonly BrainWaveDbContext _dbContext;
        public InteractionsService(BrainWaveDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool EditLike(bool status, int idArticle, int idUser)
        {
            bool statusSuccess = false;
            int likesCount = 0;
            var authorisedUser = _dbContext.Users.FirstOrDefault(m => m.Id == idUser);
            var articleCurrent = _dbContext.Articles.FirstOrDefault(m => m.Id == idArticle);
                
            if (authorisedUser != null && articleCurrent != null)
            {
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
    }
}