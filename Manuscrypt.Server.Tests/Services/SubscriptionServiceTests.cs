using Manuscrypt.Server.Data;
using Manuscrypt.Server.Data.DTOs.Subscription;
using Manuscrypt.Server.Data.Models;
using Manuscrypt.Server.Data.Repositories;
using Manuscrypt.Server.Services;
using Manuscrypt.Server.Services.Exceptions;
using Moq;

namespace Manuscrypt.Server.Tests.Services;

public class SubscriptionServiceTests
{
    [Fact]
    public async Task GetSubscriptionAsync_ReturnsDTO_WhenFound()
    {
        var mockRepo = new Mock<SubscriptionRepo>(MockBehavior.Strict, null as ManuscryptContext);
        var subscription = new Subscription
        {
            Id = 1,
            SubscriberId = 2,
            SubscribedToId = 3,
            CreatedAt = DateTime.UtcNow
        };
        mockRepo.Setup(r => r.GetAsync(1)).ReturnsAsync(subscription);

        var service = new SubscriptionService(mockRepo.Object);

        var result = await service.GetSubscriptionAsync(1);

        Assert.Equal(1, result.Id);
        Assert.Equal(2, result.SubscriberId);
        Assert.Equal(3, result.SubscribedToId);
        Assert.Equal(subscription.CreatedAt, result.CreatedAt);
    }

    [Fact]
    public async Task GetSubscriptionAsync_Throws_WhenNotFound()
    {
        var mockRepo = new Mock<SubscriptionRepo>(MockBehavior.Strict, null as ManuscryptContext);
        mockRepo.Setup(r => r.GetAsync(1)).ReturnsAsync((Subscription)null);

        var service = new SubscriptionService(mockRepo.Object);

        await Assert.ThrowsAsync<CommentDoesNotExistException>(() => service.GetSubscriptionAsync(1));
    }

    [Fact]
    public async Task CreateSubscriptionAsync_AddsAndReturnsId()
    {
        var mockRepo = new Mock<SubscriptionRepo>(MockBehavior.Strict, null as ManuscryptContext);
        var dto = new CreateSubscriptionDTO
        {
            SubscriberId = 2,
            SubscribedToId = 3,
            CreatedAt = DateTime.UtcNow
        };
        // Simulate repo setting the Id after add
        mockRepo.Setup(r => r.AddAsync(It.IsAny<Subscription>()))
            .Callback<Subscription>(s => s.Id = 42)
            .Returns(Task.CompletedTask);

        var service = new SubscriptionService(mockRepo.Object);

        var id = await service.CreateSubscriptionAsync(dto);

        Assert.Equal(42, id);
        mockRepo.Verify(r => r.AddAsync(It.Is<Subscription>(s =>
            s.SubscriberId == 2 &&
            s.SubscribedToId == 3 &&
            s.CreatedAt == dto.CreatedAt
        )), Times.Once);
    }

    [Fact]
    public async Task DeleteSubscriptionAsync_CallsRepo()
    {
        var mockRepo = new Mock<SubscriptionRepo>(MockBehavior.Strict, null as ManuscryptContext);
        mockRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

        var service = new SubscriptionService(mockRepo.Object);

        await service.DeleteSubscriptionAsync(1);

        mockRepo.Verify(r => r.DeleteAsync(1), Times.Once);
    }
}