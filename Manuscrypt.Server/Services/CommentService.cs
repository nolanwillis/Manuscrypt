using Manuscrypt.Server.Data;
using Manuscrypt.Server.Data.DTOs;
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

        public async Task<CommentDTO> GetCommentAsync(int commentId)
        {
            var comment = await _commentRepo.GetAsync(commentId);
            if (comment == null)
            {
                throw new CommentDoesNotExistException(commentId);
            }

            var commentDTO = new CommentDTO
            {
                Id = comment.Id,
                PostId = comment.PostId,
                UserId = comment.UserId,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
            };

            return commentDTO;
        }
        public async Task<IEnumerable<CommentDTO>> GetCommentsAsync()
        {
            var comments = await _commentRepo.GetAllAsync();

            var commentDTOs = comments.Select(comment => new CommentDTO
            {
                Id = comment.Id,
                PostId = comment.PostId,
                UserId = comment.UserId,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt
            }).ToList();

            return commentDTOs;
        }

        public async Task<int> CreateCommentAsync(CommentDTO commentDTO)
        {
            // Add a new Comment to the DB.
            var comment = new Comment
            {
                PostId = commentDTO.PostId,
                UserId = commentDTO.UserId,
                Content = commentDTO.Content,
                CreatedAt = commentDTO.CreatedAt
            };

            await _commentRepo.AddAsync(comment);
            await _context.SaveChangesAsync();

            return comment.Id;
        }
    }
}
