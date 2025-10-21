using Manuscrypt.Server.Controllers;
using Manuscrypt.Server.Data.DTOs.Subscription;
using Manuscrypt.Server.Services;
using Manuscrypt.Server.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Manuscrypt.Server.Tests.Controllers;

public class SubscriptionControllerTests
{
    [Fact]
    public async Task GetSubscriptionsAsync_ReturnsOk_WithSubscriptions()
    {
        var mockService = new Mock<SubscriptionService>(null, null);
        var expected = new List<GetSubscriptionDTO> { new GetSubscriptionDTO { Id = 1 } };
        mockService.Setup(s => s.GetSubscriptionsAsync()).ReturnsAsync(expected);
        var controller = new SubscriptionController(mockService.Object);

        var result = await controller.GetSubscriptionsAsync();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsAssignableFrom<IEnumerable<GetSubscriptionDTO>>(okResult.Value);
        Assert.Single(value);
    }

    [Fact]
    public async Task GetSubscriptionsAsync_ReturnsBadRequest_OnException()
    {
        var mockService = new Mock<SubscriptionService>(null, null);
        mockService.Setup(s => s.GetSubscriptionsAsync()).ThrowsAsync(new Exception());
        var controller = new SubscriptionController(mockService.Object);

        var result = await controller.GetSubscriptionsAsync();

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetSubscriptionAsync_ReturnsOk_WhenFound()
    {
        var mockService = new Mock<SubscriptionService>(null, null);
        var expected = new GetSubscriptionDTO { Id = 1 };
        mockService.Setup(s => s.GetSubscriptionAsync(1)).ReturnsAsync(expected);
        var controller = new SubscriptionController(mockService.Object);

        var result = await controller.GetSubscriptionAsync(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsType<GetSubscriptionDTO>(okResult.Value);
        Assert.Equal(1, value.Id);
    }

    [Fact]
    public async Task GetSubscriptionAsync_ReturnsNotFound_WhenNotFound()
    {
        var mockService = new Mock<SubscriptionService>(null, null);
        mockService.Setup(s => s.GetSubscriptionAsync(1)).ThrowsAsync(new SubscriptionDoesNotExistException(1));
        var controller = new SubscriptionController(mockService.Object);

        var result = await controller.GetSubscriptionAsync(1);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetSubscriptionAsync_ReturnsBadRequest_OnException()
    {
        var mockService = new Mock<SubscriptionService>(null, null);
        mockService.Setup(s => s.GetSubscriptionAsync(1)).ThrowsAsync(new Exception());
        var controller = new SubscriptionController(mockService.Object);

        var result = await controller.GetSubscriptionAsync(1);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreateSubscriptionAsync_ReturnsCreated_WhenValid()
    {
        var mockService = new Mock<SubscriptionService>(null, null);
        var createDto = new CreateSubscriptionDTO { SubscriberId = 1 };
        mockService.Setup(s => s.CreateSubscriptionAsync(createDto)).ReturnsAsync(42);
        var controller = new SubscriptionController(mockService.Object);

        var result = await controller.CreateSubscriptionAsync(createDto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(42, createdResult.RouteValues["id"]);
    }

    [Fact]
    public async Task CreateSubscriptionAsync_ReturnsBadRequest_WhenNull()
    {
        var mockService = new Mock<SubscriptionService>(null, null);
        var controller = new SubscriptionController(mockService.Object);

        var result = await controller.CreateSubscriptionAsync(null);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreateSubscriptionAsync_ReturnsBadRequest_OnException()
    {
        var mockService = new Mock<SubscriptionService>(null, null);
        var createDto = new CreateSubscriptionDTO { SubscriberId = 1 };
        mockService.Setup(s => s.CreateSubscriptionAsync(createDto)).ThrowsAsync(new Exception());
        var controller = new SubscriptionController(mockService.Object);

        var result = await controller.CreateSubscriptionAsync(createDto);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task DeleteSubscriptionAsync_ReturnsNoContent_WhenValid()
    {
        var mockService = new Mock<SubscriptionService>(null, null);
        mockService.Setup(s => s.DeleteSubscriptionAsync(1)).Returns(Task.CompletedTask);
        var controller = new SubscriptionController(mockService.Object);

        var result = await controller.DeleteSubscriptionAsync(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteSubscriptionAsync_ReturnsNotFound_WhenNotFound()
    {
        var mockService = new Mock<SubscriptionService>(null, null);
        mockService.Setup(s => s.DeleteSubscriptionAsync(1)).ThrowsAsync(new SubscriptionDoesNotExistException(1));
        var controller = new SubscriptionController(mockService.Object);

        var result = await controller.DeleteSubscriptionAsync(1);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task DeleteSubscriptionAsync_ReturnsBadRequest_OnException()
    {
        var mockService = new Mock<SubscriptionService>(null, null);
        mockService.Setup(s => s.DeleteSubscriptionAsync(1)).ThrowsAsync(new Exception());
        var controller = new SubscriptionController(mockService.Object);

        var result = await controller.DeleteSubscriptionAsync(1);

        Assert.IsType<BadRequestObjectResult>(result);
    }
}