using Manuscrypt.Server.Data.DTOs.Post;
using Manuscrypt.Server.Data.DTOs.Comment;
using Manuscrypt.Server.Data;
using Manuscrypt.Server.Data.Models;
using Manuscrypt.Server.Data.Repositories;
using Manuscrypt.Server.Services;
using Manuscrypt.Server.Services.Exceptions;
using Moq;
using Xunit;

namespace Manuscrypt.Server.Tests.Services;

public class PostServiceTests
{
    [Fact]
    public async Task GetPostAsync_ReturnsDTO_WhenFound()
    {
        var mockPostRepo = new Mock<PostRepo>(MockBehavior.Strict, null as ManuscryptContext);
        var mockUserRepo = new Mock<UserRepo>(MockBehavior.Strict, null as ManuscryptContext);

        var post = new Post
        {
            Id = 1,
            UserId = 2,
            Title = "Test",
            Description = "Desc",
            PublishedAt = DateTime.UtcNow,
            Views = 10,
            FileUrl = "file",
            FileName = "fileName",
            FileType = "txt",
            FileSizeBytes = 1024
        };
        var user = new User
        {
            Id = 2,
            DisplayName = "User",
            Description = "User Desc",
            Email = "user@email.com",
            CreatedAt = DateTime.UtcNow,
            PhotoUrl = "photo"
        };

        mockPostRepo.Setup(r => r.GetAsync(1)).ReturnsAsync(post);
        mockUserRepo.Setup(r => r.GetAsync(2)).ReturnsAsync(user);

        var service = new PostService(mockPostRepo.Object, mockUserRepo.Object);

        var result = await service.GetPostAsync(1);

        Assert.Equal(1, result.Id);
        Assert.Equal("Test", result.Title);
        Assert.Equal("Desc", result.Description);
        Assert.Equal("User", result.DisplayName);
        Assert.Equal(10, result.Views);
        Assert.Equal("file", result.FileUrl);
    }

    [Fact]
    public async Task GetPostAsync_Throws_WhenNotFound()
    {
        var mockPostRepo = new Mock<PostRepo>(MockBehavior.Strict, null as ManuscryptContext);
        var mockUserRepo = new Mock<UserRepo>(MockBehavior.Strict, null as ManuscryptContext);

        mockPostRepo.Setup(r => r.GetAsync(1)).ReturnsAsync((Post)null);

        var service = new PostService(mockPostRepo.Object, mockUserRepo.Object);

        await Assert.ThrowsAsync<PostDoesNotExistException>(() => service.GetPostAsync(1));
    }

    [Fact]
    public async Task CreatePostAsync_AddsAndReturnsId()
    {
        var mockPostRepo = new Mock<PostRepo>(MockBehavior.Strict, null as ManuscryptContext);
        var mockUserRepo = new Mock<UserRepo>(MockBehavior.Strict, null as ManuscryptContext);

        var dto = new CreatePostDTO
        {
            Title = "Test",
            Description = "Desc",
            FileName = "fileName",
            FileType = "txt",
            FileSizeBytes = 1024,
            UserId = 2
        };

        mockPostRepo.Setup(r => r.AddAsync(It.IsAny<Post>())).Returns(Task.CompletedTask);

        var service = new PostService(mockPostRepo.Object, mockUserRepo.Object);

        await service.CreatePostAsync(dto);

        mockPostRepo.Verify(r => r.AddAsync(It.Is<Post>(p =>
            p.Title == "Test" &&
            p.Description == "Desc" &&
            p.FileName == "fileName" &&
            p.FileType == "txt" &&
            p.FileSizeBytes == 1024 &&
            p.UserId == 2
        )), Times.Once);
    }

    [Fact]
    public async Task UpdatePostAsync_Throws_WhenNotFound()
    {
        var mockPostRepo = new Mock<PostRepo>(MockBehavior.Strict, null as ManuscryptContext);
        var mockUserRepo = new Mock<UserRepo>(MockBehavior.Strict, null as ManuscryptContext);

        mockPostRepo.Setup(r => r.GetAsync(1)).ReturnsAsync((Post)null);

        var service = new PostService(mockPostRepo.Object, mockUserRepo.Object);

        var dto = new UpdatePostDTO { Id = 1, Title = "Updated" };

        await Assert.ThrowsAsync<PostDoesNotExistException>(() => service.UpdatePostAsync(dto));
    }

    [Fact]
    public async Task UpdatePostAsync_Updates_WhenFound()
    {
        var mockPostRepo = new Mock<PostRepo>(MockBehavior.Strict, null as ManuscryptContext);
        var mockUserRepo = new Mock<UserRepo>(MockBehavior.Strict, null as ManuscryptContext);

        var post = new Post
        {
            Id = 1,
            UserId = 2,
            Title = "Old",
            Description = "OldDesc",
            PublishedAt = DateTime.UtcNow,
            Views = 5,
            FileUrl = "oldfile",
            FileName = "oldfileName",
            FileType = "oldtxt",
            FileSizeBytes = 512
        };

        mockPostRepo.Setup(r => r.GetAsync(1)).ReturnsAsync(post);
        mockPostRepo.Setup(r => r.UpdateAsync(post)).Returns(Task.CompletedTask);

        var service = new PostService(mockPostRepo.Object, mockUserRepo.Object);

        var dto = new UpdatePostDTO
        {
            Id = 1,
            Title = "Updated",
            Description = "NewDesc",
            FileUrl = "newfile",
        };

        await service.UpdatePostAsync(dto);

        Assert.Equal("Updated", post.Title);
        Assert.Equal("NewDesc", post.Description);
        Assert.Equal("newfile", post.FileUrl);

        mockPostRepo.Verify(r => r.UpdateAsync(post), Times.Once);
    }

    [Fact]
    public async Task DeletePostAsync_Throws_WhenNotFound()
    {
        var mockPostRepo = new Mock<PostRepo>(MockBehavior.Strict, null as ManuscryptContext);
        var mockUserRepo = new Mock<UserRepo>(MockBehavior.Strict, null as ManuscryptContext);

        mockPostRepo.Setup(r => r.DeleteAsync(1)).ThrowsAsync(new PostDoesNotExistException(1));

        var service = new PostService(mockPostRepo.Object, mockUserRepo.Object);

        await Assert.ThrowsAsync<PostDoesNotExistException>(() => service.DeletePostAsync(1));
    }

    [Fact]
    public async Task DeletePostAsync_Deletes_WhenFound()
    {
        var mockPostRepo = new Mock<PostRepo>(MockBehavior.Strict, null as ManuscryptContext);
        var mockUserRepo = new Mock<UserRepo>(MockBehavior.Strict, null as ManuscryptContext);

        var post = new Post { Id = 1, UserId = 2, Title = "Test", Description = "Desc", 
            PublishedAt = DateTime.UtcNow, Views = 10, FileUrl = "file", FileName = "fileName", FileType = "txt", FileSizeBytes = 1024 };
        mockPostRepo.Setup(r => r.GetAsync(1)).ReturnsAsync(post);
        mockPostRepo.Setup(r => r.DeleteAsync(post.Id)).Returns(Task.CompletedTask);

        var service = new PostService(mockPostRepo.Object, mockUserRepo.Object);

        await service.DeletePostAsync(1);

        mockPostRepo.Verify(r => r.DeleteAsync(post.Id), Times.Once);
    }

    [Fact]
    public async Task GetCommentsForPostAsync_ReturnsDTOs()
    {
        var mockPostRepo = new Mock<PostRepo>(MockBehavior.Strict, null as ManuscryptContext);
        var mockUserRepo = new Mock<UserRepo>(MockBehavior.Strict, null as ManuscryptContext);

        var comments = new List<Comment>
        {
            new Comment { Id = 1, PostId = 1, UserId = 2, Content = "Comment1", CreatedAt = DateTime.UtcNow },
            new Comment { Id = 2, PostId = 1, UserId = 3, Content = "Comment2", CreatedAt = DateTime.UtcNow }
        };
        var user = new User { Id = 2, DisplayName = "User2", Description = "Desc2", Email = "user2@email.com", CreatedAt = DateTime.UtcNow, PhotoUrl = "photo2" };

        mockPostRepo.Setup(r => r.GetCommentsForPostAsync(1)).ReturnsAsync(comments);
        mockUserRepo.Setup(r => r.GetAsync(2)).ReturnsAsync(user);

        var service = new PostService(mockPostRepo.Object, mockUserRepo.Object);

        var result = (await service.GetCommentsForPostAsync(1)).ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal(1, result[0].Id);
        Assert.Equal("Comment1", result[0].Content);
        Assert.Equal(2, result[1].Id);
        Assert.Equal("Comment2", result[1].Content);
    }
}