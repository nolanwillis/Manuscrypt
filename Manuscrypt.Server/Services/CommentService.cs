using Manuscrypt.Server.Data;
using Manuscrypt.Server.Data.DTOs.Comment;
using Manuscrypt.Server.Data.Models;
using Manuscrypt.Server.Data.Repositories;
using Manuscrypt.Server.Services.Exceptions;

namespace Manuscrypt.Server.Services
{
    public class CommentService
    {
        private readonly ManuscryptContext _context;
        private readonly CommentRepo _commentRepo;

        public CommentService(ManuscryptContext context, CommentRepo commentRepo)
        {
            _context = context;
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
        public virtual async Task<IEnumerable<GetCommentDTO>> GetCommentsAsync()
        {
            var comments = await _commentRepo.GetAllAsync();

            var commentDTOs = comments.Select(comment => new GetCommentDTO
            {
                Id = comment.Id,
                PostId = comment.PostId,
                UserId = comment.UserId,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt
            }).ToList();

            return commentDTOs;
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
            await _context.SaveChangesAsync();

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
            _commentRepo.Update(comment);
            _context.SaveChanges();
        }

        public virtual async Task DeleteCommentAsync(int commentId)
        {
            var comment = await _commentRepo.GetAsync(commentId);
            if (comment == null)
            {
                throw new CommentDoesNotExistException(commentId);
            }

            _commentRepo.Delete(comment);
            _context.SaveChanges();
        }
    }
}
