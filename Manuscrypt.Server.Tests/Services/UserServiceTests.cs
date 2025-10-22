using Manuscrypt.Server.Data;
using Manuscrypt.Server.Data.DTOs.User;
using Manuscrypt.Server.Data.Models;
using Manuscrypt.Server.Data.Repositories;
using Manuscrypt.Server.Services;
using Manuscrypt.Server.Services.Exceptions;
using Moq;

namespace Manuscrypt.Server.Tests.Services;

public class UserServiceTests
{
    [Fact]
    public async Task GetUserAsync_ReturnsDTO_WhenFound()
    {
        var mockRepo = new Mock<UserRepo>(MockBehavior.Strict, null as ManuscryptContext);
        var user = new User
        {
            Id = 1,
            DisplayName = "TestUser",
            Description = "Desc",
            Email = "test@email.com",
            CreatedAt = DateTime.UtcNow,
            PhotoUrl = "photo"
        };
        mockRepo.Setup(r => r.GetAsync(1)).ReturnsAsync(user);

        var service = new UserService(mockRepo.Object);

        var result = await service.GetUserAsync(1);

        Assert.Equal(1, result.Id);
        Assert.Equal("TestUser", result.DisplayName);
        Assert.Equal("Desc", result.Description);
        Assert.Equal("test@email.com", result.Email);
        Assert.Equal(user.CreatedAt, result.CreatedAt);
        Assert.Equal("photo", result.PhotoUrl);
    }

    [Fact]
    public async Task GetUserAsync_Throws_WhenNotFound()
    {
        var mockRepo = new Mock<UserRepo>(MockBehavior.Strict, null as ManuscryptContext);
        mockRepo.Setup(r => r.GetAsync(1)).ReturnsAsync((User)null);

        var service = new UserService(mockRepo.Object);

        await Assert.ThrowsAsync<UserDNEWithIdException>(() => service.GetUserAsync(1));
    }

    [Fact]
    public async Task GetUsersByIdsAsync_ReturnsDTOs()
    {
        var mockRepo = new Mock<UserRepo>(MockBehavior.Strict, null as ManuscryptContext);
        var users = new List<User>
        {
            new User { Id = 1, DisplayName = "User1", Description = "Desc1", Email = "u1@email.com", CreatedAt = DateTime.UtcNow },
            new User { Id = 2, DisplayName = "User2", Description = "Desc2", Email = "u2@email.com", CreatedAt = DateTime.UtcNow }
        };
        mockRepo.Setup(r => r.GetAllAsync(It.IsAny<IEnumerable<int>>())).ReturnsAsync(users);

        var service = new UserService(mockRepo.Object);

        var result = (await service.GetUsersByIdsAsync(new[] { 1, 2 })).ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal(1, result[0].Id);
        Assert.Equal("User1", result[0].DisplayName);
        Assert.Equal(2, result[1].Id);
        Assert.Equal("User2", result[1].DisplayName);
    }

    [Fact]
    public async Task GetPostsForUserAsync_ReturnsDTOs()
    {
        var mockRepo = new Mock<UserRepo>(MockBehavior.Strict, null as ManuscryptContext);
        var posts = new List<Post>
        {
            new Post { Id = 1, Title = "Post1", Description = "Desc1", PublishedAt = DateTime.UtcNow, Views = 10, FileUrl = "file1" },
            new Post { Id = 2, Title = "Post2", Description = "Desc2", PublishedAt = DateTime.UtcNow, Views = 20, FileUrl = "file2" }
        };
        var user = new User { Id = 1, DisplayName = "User1" };
        mockRepo.Setup(r => r.GetPostsForUserAsync(1)).ReturnsAsync(posts);
        mockRepo.Setup(r => r.GetAsync(1)).ReturnsAsync(user);

        var service = new UserService(mockRepo.Object);

        var result = (await service.GetPostsForUserAsync(1)).ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal("User1", result[0].DisplayName);
        Assert.Equal("Post1", result[0].Title);
        Assert.Equal("Post2", result[1].Title);
    }

    [Fact]
    public async Task GetPostsForUserAsync_Throws_WhenUserNotFound()
    {
        var mockRepo = new Mock<UserRepo>(MockBehavior.Strict, null as ManuscryptContext);
        mockRepo.Setup(r => r.GetPostsForUserAsync(1)).ReturnsAsync(new List<Post>());
        mockRepo.Setup(r => r.GetAsync(1)).ReturnsAsync((User)null);

        var service = new UserService(mockRepo.Object);

        await Assert.ThrowsAsync<UserDNEWithIdException>(() => service.GetPostsForUserAsync(1));
    }

    [Fact]
    public async Task CreateUserAsync_AddsAndReturnsId()
    {
        var mockRepo = new Mock<UserRepo>(MockBehavior.Strict, null as ManuscryptContext);
        var dto = new CreateUserDTO { DisplayName = "TestUser", Email = "test@email.com", Password = "pw" };
        mockRepo.Setup(r => r.GetByEmailAsync(dto.Email)).ReturnsAsync((User)null);
        mockRepo.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

        var service = new UserService(mockRepo.Object);

        var id = await service.CreateUserAsync(dto);

        mockRepo.Verify(r => r.AddAsync(It.Is<User>(u =>
            u.DisplayName == "TestUser" &&
            u.Email == "test@email.com" &&
            !string.IsNullOrEmpty(u.PasswordHash)
        )), Times.Once);
    }

    [Fact]
    public async Task CreateUserAsync_Throws_WhenUserExists()
    {
        var mockRepo = new Mock<UserRepo>(MockBehavior.Strict, null as ManuscryptContext);
        var dto = new CreateUserDTO { DisplayName = "TestUser", Email = "test@email.com", Password = "pw" };
        mockRepo.Setup(r => r.GetByEmailAsync(dto.Email)).ReturnsAsync(new User());

        var service = new UserService(mockRepo.Object);

        await Assert.ThrowsAsync<UserExistsException>(() => service.CreateUserAsync(dto));
    }

    [Fact]
    public async Task LoginAsync_ReturnsId_WhenValid()
    {
        var mockRepo = new Mock<UserRepo>(MockBehavior.Strict, null as ManuscryptContext);
        var password = "pw";
        var hash = BCrypt.Net.BCrypt.HashPassword(password);
        var user = new User { Id = 1, Email = "test@email.com", PasswordHash = hash };
        var dto = new LoginDTO { Email = "test@email.com", Password = password };
        mockRepo.Setup(r => r.GetByEmailAsync(dto.Email)).ReturnsAsync(user);

        var service = new UserService(mockRepo.Object);

        var id = await service.LoginAsync(dto);

        Assert.Equal(1, id);
    }

    [Fact]
    public async Task LoginAsync_Throws_WhenUserNotFound()
    {
        var mockRepo = new Mock<UserRepo>(MockBehavior.Strict, null as ManuscryptContext);
        var dto = new LoginDTO { Email = "test@email.com", Password = "pw" };
        mockRepo.Setup(r => r.GetByEmailAsync(dto.Email)).ReturnsAsync((User)null);

        var service = new UserService(mockRepo.Object);

        await Assert.ThrowsAsync<UserDNEWithEmailException>(() => service.LoginAsync(dto));
    }

    [Fact]
    public async Task LoginAsync_Throws_WhenPasswordIncorrect()
    {
        var mockRepo = new Mock<UserRepo>(MockBehavior.Strict, null as ManuscryptContext);
        var user = new User { Id = 1, Email = "test@email.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("otherpw") };
        var dto = new LoginDTO { Email = "test@email.com", Password = "pw" };
        mockRepo.Setup(r => r.GetByEmailAsync(dto.Email)).ReturnsAsync(user);

        var service = new UserService(mockRepo.Object);

        await Assert.ThrowsAsync<IncorrectPasswordException>(() => service.LoginAsync(dto));
    }
}