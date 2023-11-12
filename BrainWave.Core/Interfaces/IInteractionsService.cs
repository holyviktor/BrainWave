using BrainWave.Core.DTOs;
using BrainWave.Core.Entities;

namespace BrainWave.Core.Interfaces
{
    public interface IInteractionsService
    {
        public bool EditLike(LikesSavingsDTO likesSavingsDTO);
        public bool EditSaving(LikesSavingsDTO likesSavingsDTO);
        public Comment AddComment(AddCommentDTO commentDTO);
        public bool DeleteComment(DeleteCommentDTO commentDTO);
    }
}
