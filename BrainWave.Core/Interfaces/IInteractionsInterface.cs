using BrainWave.Core.Entities;

namespace BrainWave.Core.Interfaces
{
    public interface IInteractionsInterface
    {
        public bool EditLike(bool status, int idArticle, int idUser);
        public bool EditSaving(bool status, int idArticle, int idUser);
        public Comment AddComment(int idArticle, int idUser, string comment);
        public bool DeleteComment(int idArticle, int idUser, int idComment);
    }
}
