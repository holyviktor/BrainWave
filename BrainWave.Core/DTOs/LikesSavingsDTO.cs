using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainWave.Core.DTOs
{
    public class LikesSavingsDTO
    {
        public bool status;
        public int idArticle;
        public int idUser;
        public LikesSavingsDTO(bool status, int idArticle, int idUser)
        {
            this.status = status;
            this.idArticle = idArticle;
            this.idUser = idUser;
        }
    }
}
