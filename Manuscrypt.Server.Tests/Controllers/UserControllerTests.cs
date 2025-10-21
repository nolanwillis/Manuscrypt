using Manuscrypt.Server.Controllers;
using Manuscrypt.Server.Data.DTOs.User;
using Manuscrypt.Server.Data.DTOs.Post;
using Manuscrypt.Server.Data.DTOs.Comment;
using Manuscrypt.Server.Data.DTOs.Subscription;
using Manuscrypt.Server.Services;
using Manuscrypt.Server.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Manuscrypt.Server.Tests.Controllers;

public class UserControllerTests
{
    [Fact]
    public async Task GetUserAsync_ReturnsOk_WhenFound()
    {
        var mockService = new Mock<UserService>(null, null);
        var expected = new GetUserDTO { Id = 1 };
        mockService.Setup(s => s.GetUserAsync(1)).ReturnsAsync(expected);
        var controller = new UserController(mockService.Object);

        var result = await controller.GetUserAsync(1);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsType<GetUserDTO>(ok.Value);
        Assert.Equal(1, value.Id);
    }

    [Fact]
    public async Task GetUserAsync_ReturnsNotFound_WhenNotFound()
    {
        var mockService = new Mock<UserService>(null, null);
        mockService.Setup(s => s.GetUserAsync(1)).ThrowsAsync(new UserDNEWithIdException(1));
        var controller = new UserController(mockService.Object);

        var result = await controller.GetUserAsync(1);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetUserAsync_ReturnsBadRequest_OnException()
    {
        var mockService = new Mock<UserService>(null, null);
        mockService.Setup(s => s.GetUserAsync(1)).ThrowsAsync(new Exception());
        var controller = new UserController(mockService.Object);

        var result = await controller.GetUserAsync(1);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetCommentsByUserAsync_ReturnsOk()
    {
        var mockService = new Mock<UserService>(null, null);
        var expected = new List<GetCommentDTO> { new GetCommentDTO { Id = 1 } };
        mockService.Setup(s => s.GetCommentsByUserAsync(1)).ReturnsAsync(expected);
        var controller = new UserController(mockService.Object);

        var result = await controller.GetCommentsByUserAsync(1);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsAssignableFrom<IEnumerable<GetCommentDTO>>(ok.Value);
        Assert.Single(value);
    }

    [Fact]
    public async Task GetCommentsByUserAsync_ReturnsBadRequest_OnException()
    {
        var mockService = new Mock<UserService>(null, null);
        mockService.Setup(s => s.GetCommentsByUserAsync(1)).ThrowsAsync(new Exception());
        var controller = new UserController(mockService.Object);

        var result = await controller.GetCommentsByUserAsync(1);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetPostsForUserAsync_ReturnsOk()
    {
        var mockService = new Mock<UserService>(null, null);
        var expected = new List<GetPostDTO> { new GetPostDTO { Id = 1 } };
        mockService.Setup(s => s.GetPostsForUserAsync(1)).ReturnsAsync(expected);
        var controller = new UserController(mockService.Object);

        var result = await controller.GetPostsForUserAsync(1);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsAssignableFrom<IEnumerable<GetPostDTO>>(ok.Value);
        Assert.Single(value);
    }

    [Fact]
    public async Task GetPostsForUserAsync_ReturnsBadRequest_OnException()
    {
        var mockService = new Mock<UserService>(null, null);
        mockService.Setup(s => s.GetPostsForUserAsync(1)).ThrowsAsync(new Exception());
        var controller = new UserController(mockService.Object);

        var result = await controller.GetPostsForUserAsync(1);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetSubscribersForUserAsync_ReturnsOk()
    {
        var mockService = new Mock<UserService>(null, null);
        var expected = new List<GetSubscriptionDTO> { new GetSubscriptionDTO { Id = 1 } };
        mockService.Setup(s => s.GetSubscribersForUserAsync(1)).ReturnsAsync(expected);
        var controller = new UserController(mockService.Object);

        var result = await controller.GetSubscribersForUserAsync(1);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsAssignableFrom<IEnumerable<GetSubscriptionDTO>>(ok.Value);
        Assert.Single(value);
    }

    [Fact]
    public async Task GetSubscribersForUserAsync_ReturnsBadRequest_OnException()
    {
        var mockService = new Mock<UserService>(null, null);
        mockService.Setup(s => s.GetSubscribersForUserAsync(1)).ThrowsAsync(new Exception());
        var controller = new UserController(mockService.Object);

        var result = await controller.GetSubscribersForUserAsync(1);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetSubscriptionsForUserAsync_ReturnsOk()
    {
        var mockService = new Mock<UserService>(null, null);
        var expected = new List<GetSubscriptionDTO> { new GetSubscriptionDTO { Id = 1 } };
        mockService.Setup(s => s.GetSubscriptionsForUserAsync(1)).ReturnsAsync(expected);
        var controller = new UserController(mockService.Object);

        var result = await controller.GetSubscrptionsForUserAsync(1);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsAssignableFrom<IEnumerable<GetSubscriptionDTO>>(ok.Value);
        Assert.Single(value);
    }

    [Fact]
    public async Task GetSubscriptionsForUserAsync_ReturnsBadRequest_OnException()
    {
        var mockService = new Mock<UserService>(null, null);
        mockService.Setup(s => s.GetSubscriptionsForUserAsync(1)).ThrowsAsync(new Exception());
        var controller = new UserController(mockService.Object);

        var result = await controller.GetSubscrptionsForUserAsync(1);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreateUserAsync_ReturnsCreated_WhenValid()
    {
        var mockService = new Mock<UserService>(null, null);
        var createDto = new CreateUserDTO { DisplayName = "Test", Email = "test@example.com", Password = "pw" };
        mockService.Setup(s => s.CreateUserAsync(createDto)).ReturnsAsync(42);
        var controller = new UserController(mockService.Object);

        var result = await controller.CreateUserAsync(createDto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(42, createdResult.RouteValues["id"]);
    }

    [Fact]
    public async Task CreateUserAsync_ReturnsBadRequest_WhenNull()
    {
        var mockService = new Mock<UserService>(null, null);
        var controller = new UserController(mockService.Object);

        var result = await controller.CreateUserAsync(null);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreateUserAsync_ReturnsBadRequest_OnException()
    {
        var mockService = new Mock<UserService>(null, null);
        var createDto = new CreateUserDTO { DisplayName = "Test", Email = "test@example.com", Password = "pw" };
        mockService.Setup(s => s.CreateUserAsync(createDto)).ThrowsAsync(new Exception());
        var controller = new UserController(mockService.Object);

        var result = await controller.CreateUserAsync(createDto);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task LoginAsync_ReturnsOk_WhenValid()
    {
        var mockService = new Mock<UserService>(null, null);
        var loginDto = new LoginDTO { Email = "test@example.com", Password = "pw" };
        mockService.Setup(s => s.LoginAsync(loginDto)).ReturnsAsync(42);
        var controller = new UserController(mockService.Object);

        var result = await controller.LoginAsync(loginDto);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var value = ok.Value;
        var idProperty = value.GetType().GetProperty("id");
        Assert.NotNull(idProperty);
        Assert.Equal(42, (int)idProperty.GetValue(value));
    }

    [Fact]
    public async Task LoginAsync_ReturnsNotFound_WhenUserNotFound()
    {
        var mockService = new Mock<UserService>(null, null);
        var loginDto = new LoginDTO { Email = "test@example.com", Password = "pw" };
        mockService.Setup(s => s.LoginAsync(loginDto)).ThrowsAsync(new UserDNEWithEmailException("test@example.com"));
        var controller = new UserController(mockService.Object);

        var result = await controller.LoginAsync(loginDto);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task LoginAsync_ReturnsBadRequest_WhenNull()
    {
        var mockService = new Mock<UserService>(null, null);
        var controller = new UserController(mockService.Object);

        var result = await controller.LoginAsync(null);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task LoginAsync_ReturnsBadRequest_OnException()
    {
        var mockService = new Mock<UserService>(null, null);
        var loginDto = new LoginDTO { Email = "test@example.com", Password = "pw" };
        mockService.Setup(s => s.LoginAsync(loginDto)).ThrowsAsync(new Exception());
        var controller = new UserController(mockService.Object);

        var result = await controller.LoginAsync(loginDto);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }
}