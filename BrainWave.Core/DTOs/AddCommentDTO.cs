using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainWave.Core.DTOs
{
    public class AddCommentDTO
    {
        public int idArticle;
        public int idUser;
        public string comment;
        public AddCommentDTO(int idArticle, int isUser, string comment) {
            this.idArticle = idArticle;
            this.idUser = isUser;
            this.comment = comment;
        }
    }
}
