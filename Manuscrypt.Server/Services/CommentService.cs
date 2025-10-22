using Manuscrypt.Server.Data;
using Manuscrypt.Server.Data.DTOs.Comment;
using Manuscrypt.Server.Data.Models;
using Manuscrypt.Server.Data.Repositories;
using Manuscrypt.Server.Services.Exceptions;

namespace Manuscrypt.Server.Services
{
    public class CommentService
    {
        private readonly CommentRepo _commentRepo;

        public CommentService(CommentRepo commentRepo)
        {
            _commentRepo = commentRepo;
        }

        public virtual async Task<GetCommentDTO> GetCommentAsync(int commentId)
        {
            var comment = await _commentRepo.GetAsync(commentId);
            if (comment == null)
            {
                throw new CommentDoesNotExistException(commentId);
            }

            var commentDTO = new GetCommentDTO
            {
                Id = comment.Id,
                PostId = comment.PostId,
                UserId = comment.UserId,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
            };

            return commentDTO;
        } 

        public virtual async Task<int> CreateCommentAsync(CreateCommentDTO createCommentDTO)
        {
            // Add a new Comment to the DB.
            var comment = new Comment
            {
                PostId = createCommentDTO.PostId,
                UserId = createCommentDTO.UserId,
                Content = createCommentDTO.Content,
                CreatedAt = createCommentDTO.CreatedAt
            };

            await _commentRepo.AddAsync(comment);

            return comment.Id;
        }

        public virtual async Task UpdateCommentAsync(UpdateCommentDTO updateCommentDTO)
        {
            var comment = await _commentRepo.GetAsync(updateCommentDTO.Id);
            if (comment == null)
            {
                throw new CommentDoesNotExistException(updateCommentDTO.Id);
            }

            comment.Content = updateCommentDTO.Content;

            await _commentRepo.UpdateAsync(comment);
        }

        public virtual async Task DeleteCommentAsync(int commentId) => await _commentRepo.DeleteAsync(commentId);
    }
}
