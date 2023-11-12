using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainWave.Core.DTOs
{
    public class DeleteCommentDTO
    {
        public int idArticle;
        public int idUser;
        public int idComment;
        public DeleteCommentDTO(int idArticle, int isUser, int idComment)
        {
            this.idArticle = idArticle;
            this.idUser = isUser;
            this.idComment = idComment;
        }
    }
}
