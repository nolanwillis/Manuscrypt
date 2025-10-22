using Manuscrypt.Server.Data;
using Manuscrypt.Server.Data.DTOs.Comment;
using Manuscrypt.Server.Data.Models;
using Manuscrypt.Server.Data.Repositories;
using Manuscrypt.Server.Services;
using Manuscrypt.Server.Services.Exceptions;
using Moq;

namespace Manuscrypt.Server.Tests.Services;

public class CommentServiceTests
{ 
    [Fact]
    public async Task GetCommentAsync_ReturnsDTO_WhenFound()
    {
        var mockRepo = new Mock<CommentRepo>(MockBehavior.Strict, null as ManuscryptContext);
        var creationTime = DateTime.UtcNow;
        var comment = new Comment { Id = 1, PostId = 2, UserId = 3, Content = "Test", CreatedAt = creationTime };
        mockRepo.Setup(r => r.GetAsync(1)).ReturnsAsync(comment);

        var service = new CommentService(mockRepo.Object);

        var result = await service.GetCommentAsync(1);

        Assert.Equal(1, result.Id);
        Assert.Equal(2, result.PostId);
        Assert.Equal(3, result.UserId);
        Assert.Equal("Test", result.Content);
        Assert.Equal(creationTime, result.CreatedAt);
    }

    [Fact]
    public async Task GetCommentAsync_Throws_WhenNotFound()
    {
        var mockRepo = new Mock<CommentRepo>(MockBehavior.Strict, null as ManuscryptContext);
        mockRepo.Setup(r => r.GetAsync(1)).ReturnsAsync((Comment)null);

        var service = new CommentService(mockRepo.Object);

        await Assert.ThrowsAsync<CommentDoesNotExistException>(() => service.GetCommentAsync(1));
    }

    [Fact]
    public async Task CreateCommentAsync_AddsAndReturnsId()
    {
        var mockRepo = new Mock<CommentRepo>(MockBehavior.Strict, null as ManuscryptContext);
        var dto = new CreateCommentDTO { PostId = 2, UserId = 3, Content = "Test", CreatedAt = DateTime.UtcNow };
        mockRepo.Setup(r => r.AddAsync(It.IsAny<Comment>())).Returns(Task.CompletedTask);

        var service = new CommentService(mockRepo.Object);

        var id = await service.CreateCommentAsync(dto);

        mockRepo.Verify(r => r.AddAsync(It.Is<Comment>(c => c.Content == "Test" && c.PostId == 2 && c.UserId == 3)), Times.Once);
    }

    [Fact]
    public async Task UpdateCommentAsync_Throws_WhenNotFound()
    {
        var mockRepo = new Mock<CommentRepo>(MockBehavior.Strict, null as ManuscryptContext);
        mockRepo.Setup(r => r.GetAsync(1)).ReturnsAsync((Comment)null);

        var service = new CommentService(mockRepo.Object);

        var dto = new UpdateCommentDTO { Id = 1, Content = "Updated" };

        await Assert.ThrowsAsync<CommentDoesNotExistException>(() => service.UpdateCommentAsync(dto));
    }

    [Fact]
    public async Task UpdateCommentAsync_Updates_WhenFound()
    {
        var mockRepo = new Mock<CommentRepo>(MockBehavior.Strict, null as ManuscryptContext);
        var comment = new Comment { Id = 1, Content = "Old" };
        mockRepo.Setup(r => r.GetAsync(1)).ReturnsAsync(comment);
        mockRepo.Setup(r => r.UpdateAsync(comment)).Returns(Task.CompletedTask);

        var service = new CommentService(mockRepo.Object);

        var dto = new UpdateCommentDTO { Id = 1, Content = "Updated" };
        await service.UpdateCommentAsync(dto);

        Assert.Equal("Updated", comment.Content);
        mockRepo.Verify(r => r.UpdateAsync(comment), Times.Once);
    }

    [Fact]
    public async Task DeleteCommentAsync_Throws_WhenNotFound()
    {
        var mockRepo = new Mock<CommentRepo>(MockBehavior.Strict, null as ManuscryptContext);
        mockRepo.Setup(r => r.DeleteAsync(1)).ThrowsAsync(new CommentDoesNotExistException(1));

        var service = new CommentService(mockRepo.Object);

        await Assert.ThrowsAsync<CommentDoesNotExistException>(() => service.DeleteCommentAsync(1));
    }

    [Fact]
    public async Task DeleteCommentAsync_Deletes_WhenFound()
    {
        var mockRepo = new Mock<CommentRepo>(MockBehavior.Strict, null as ManuscryptContext);
        var comment = new Comment { Id = 1 };
        mockRepo.Setup(r => r.GetAsync(1)).ReturnsAsync(comment);
        mockRepo.Setup(r => r.DeleteAsync(comment.Id)).Returns(Task.CompletedTask);

        var service = new CommentService(mockRepo.Object);

        await service.DeleteCommentAsync(1);

        mockRepo.Verify(r => r.DeleteAsync(comment.Id), Times.Once);
    }
}