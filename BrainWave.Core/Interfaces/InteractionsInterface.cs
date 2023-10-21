using BrainWave.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainWave.Core.Interfaces
{
    public interface InteractionsInterface
    {
        public bool EditLike(bool status, int idArticle, int idUser);
        public bool EditSaving(bool status, int idArticle, int idUser);
        public Comment AddComment(int idArticle, int idUser, string comment);
        public bool DeleteComment(int idArticle, int idUser, int idComment);
    }
}
