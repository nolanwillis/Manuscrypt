using Manuscrypt.CommentService.Data;
using Manuscrypt.CommentService.Data.Repositories;
using Manuscrypt.CommentService.Exceptions;
using Manuscrypt.Shared;
using Manuscrypt.Shared.DTOs.Comment;

namespace Manuscrypt.CommentService
{
    public class CommentDomainService
    {
        private readonly CommentRepo _commentRepo;
        private readonly EventRepo _eventRepo;
        private readonly AuthServiceClient _authServiceClient;
        private readonly PostServiceClient _postServiceClient;

        public CommentDomainService(CommentRepo commentRepo, EventRepo eventRepo, AuthServiceClient authServiceClient, 
            PostServiceClient postServiceClient)
        {
            _commentRepo = commentRepo;
            _eventRepo = eventRepo;
            _authServiceClient = authServiceClient;
            _postServiceClient = postServiceClient;
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

        public virtual async Task<IEnumerable<GetCommentDTO>> GetCommentsByUserAsync(int userId)
        {
            // Ensure the user exists.
            try
            {
                await _authServiceClient.GetAsync(userId);
            }
            catch (HttpRequestException)
            {
                throw new UserForCommentDoesNotExistException(userId);
            }

            var comments = await _commentRepo.GetCommentsByUserAsync(userId);

            var getCommentDTOs = comments.Select(comment => new GetCommentDTO
            {
                Id = comment.Id,
                PostId = comment.PostId,
                UserId = comment.UserId,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
            });

            return getCommentDTOs;
        }

        public virtual async Task<IEnumerable<GetCommentDTO>> GetCommentsForPostAsync(int postId)
        {
            // Ensure the post exists. 
            try
            {
                await _postServiceClient.GetAsync(postId);
            }
            catch (HttpRequestException)
            {
                throw new PostForCommentDoesNotExistException(postId);
            }

            var comments = await _commentRepo.GetCommentsForPostAsync(postId);

            var getCommentDTOs = comments.Select(comment => new GetCommentDTO
            {
                Id = comment.Id,
                PostId = comment.PostId,
                UserId = comment.UserId,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
            });

            return getCommentDTOs;
        }

        public async Task<IEnumerable<Event>> GetEventsAsync(int startId, int endId)
            => await _eventRepo.GetAsync(startId, endId);

        public virtual async Task<GetCommentDTO> CreateCommentAsync(CreateCommentDTO createCommentDTO)
        {
            // Ensure the associated user exists.
            try
            {
                await _authServiceClient.GetAsync(createCommentDTO.UserId);
            }
            catch (HttpRequestException)
            {
                throw new UserForCommentDoesNotExistException(createCommentDTO.UserId);
            }
            // Ensure the associated post exists. 
            try
            {
                await _postServiceClient.GetAsync(createCommentDTO.PostId);
            }
            catch (HttpRequestException)
            {
                throw new PostForCommentDoesNotExistException(createCommentDTO.PostId);
            }

            var comment = new Comment
            {
                PostId = createCommentDTO.PostId,
                UserId = createCommentDTO.UserId,
                Content = createCommentDTO.Content,
                CreatedAt = createCommentDTO.CreatedAt
            };

            await _commentRepo.CreateAsync(comment);
            await _eventRepo.AddCreateCommentEventAsync(comment);

            var getCommentDTO = new GetCommentDTO
            {
                Id = comment.Id,
                PostId = comment.PostId,
                UserId = comment.UserId,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt
            };

            return getCommentDTO;
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

        public virtual async Task DeleteCommentAsync(int commentId) 
        { 
            await _commentRepo.DeleteAsync(commentId);
            await _eventRepo.AddDeleteCommentEventAsync(commentId);
        } 
    }
}
