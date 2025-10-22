using Manuscrypt.Server.Controllers;
using Manuscrypt.Server.Data.DTOs.Comment;
using Manuscrypt.Server.Services;
using Manuscrypt.Server.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Manuscrypt.Server.Tests.Controllers;

public class CommentControllerTests
{ 
    [Fact]
    public async Task GetCommentAsync_ReturnsOk_WhenFound()
    {
        var mockService = new Mock<CommentService>(null);
        var creationTime = DateTime.UtcNow;
        var expectedComment = new GetCommentDTO { Id = 1, PostId = 2, UserId = 3, Content = "Content", CreatedAt = creationTime};
        mockService.Setup(s => s.GetCommentAsync(1))
                   .ReturnsAsync(expectedComment);
        var controller = new CommentController(mockService.Object);

        var result = await controller.GetCommentAsync(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<GetCommentDTO>(okResult.Value);
        Assert.Equal(1, returnValue.Id);
        Assert.Equal(2, returnValue.PostId);
        Assert.Equal(3, returnValue.UserId);
        Assert.Equal("Content", returnValue.Content);
        Assert.Equal(creationTime, returnValue.CreatedAt);
    }

    [Fact]
    public async Task GetCommentAsync_ReturnsNotFound_WhenNotFound()
    {
        var mockService = new Mock<CommentService>(null);
        mockService.Setup(s => s.GetCommentAsync(1))
                   .ThrowsAsync(new CommentDoesNotExistException(1));
        var controller = new CommentController(mockService.Object);

        var result = await controller.GetCommentAsync(1);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetCommentAsync_ReturnsBadRequest_OnException()
    {
        var mockService = new Mock<CommentService>(null);
        mockService.Setup(s => s.GetCommentAsync(1))
                   .ThrowsAsync(new Exception("Service error"));
        var controller = new CommentController(mockService.Object);

        var result = await controller.GetCommentAsync(1);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreateCommentAsync_ReturnsCreated_WhenValid()
    {
        var mockService = new Mock<CommentService>(null);
        var createDto = new CreateCommentDTO { PostId = 2, UserId = 3, Content = "Content", CreatedAt = DateTime.UtcNow };
        mockService.Setup(s => s.CreateCommentAsync(createDto))
                   .ReturnsAsync(42);
        var controller = new CommentController(mockService.Object);

        var result = await controller.CreateCommentAsync(createDto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(42, createdResult.RouteValues["id"]);
    }

    [Fact]
    public async Task CreateCommentAsync_ReturnsBadRequest_WhenNull()
    {
        var mockService = new Mock<CommentService>(null);
        var controller = new CommentController(mockService.Object);

        var result = await controller.CreateCommentAsync(null);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreateCommentAsync_ReturnsBadRequest_OnException()
    {
        var mockService = new Mock<CommentService>(null);
        var createDto = new CreateCommentDTO { PostId = 2, UserId = 3, Content = "Content", CreatedAt = DateTime.UtcNow };
        mockService.Setup(s => s.CreateCommentAsync(createDto))
                   .ThrowsAsync(new Exception("Service error"));
        var controller = new CommentController(mockService.Object);

        var result = await controller.CreateCommentAsync(createDto);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdateCommentAsync_ReturnsNoContent_WhenValid()
    {
        var mockService = new Mock<CommentService>(null);
        var updateDto = new UpdateCommentDTO { Id = 1, Content = "Updated" };
        mockService.Setup(s => s.UpdateCommentAsync(updateDto))
                   .Returns(Task.CompletedTask);
        var controller = new CommentController(mockService.Object);

        var result = await controller.UpdateCommentAsync(updateDto);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateCommentAsync_ReturnsBadRequest_WhenNull()
    {
        var mockService = new Mock<CommentService>(null);
        var controller = new CommentController(mockService.Object);

        var result = await controller.UpdateCommentAsync(null);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdateCommentAsync_ReturnsNotFound_WhenNotFound()
    {
        var mockService = new Mock<CommentService>(null);
        var updateDto = new UpdateCommentDTO { Id = 1, Content = "Updated" };
        mockService.Setup(s => s.UpdateCommentAsync(updateDto))
                   .ThrowsAsync(new CommentDoesNotExistException(1));
        var controller = new CommentController(mockService.Object);

        var result = await controller.UpdateCommentAsync(updateDto);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task UpdateCommentAsync_ReturnsBadRequest_OnException()
    {
        var mockService = new Mock<CommentService>(null);
        var updateDto = new UpdateCommentDTO { Id = 1, Content = "Updated" };
        mockService.Setup(s => s.UpdateCommentAsync(updateDto))
                   .ThrowsAsync(new Exception("Service error"));
        var controller = new CommentController(mockService.Object);

        var result = await controller.UpdateCommentAsync(updateDto);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task DeleteCommentAsync_ReturnsNoContent_WhenValid()
    {
        var mockService = new Mock<CommentService>(null);
        mockService.Setup(s => s.DeleteCommentAsync(1))
                   .Returns(Task.CompletedTask);
        var controller = new CommentController(mockService.Object);

        var result = await controller.DeleteCommentAsync(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteCommentAsync_ReturnsNotFound_WhenNotFound()
    {
        var mockService = new Mock<CommentService>(null);
        mockService.Setup(s => s.DeleteCommentAsync(1))
                   .ThrowsAsync(new CommentDoesNotExistException(1));
        var controller = new CommentController(mockService.Object);

        var result = await controller.DeleteCommentAsync(1);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task DeleteCommentAsync_ReturnsBadRequest_OnException()
    {
        var mockService = new Mock<CommentService>(null);
        mockService.Setup(s => s.DeleteCommentAsync(1))
                   .ThrowsAsync(new Exception("Service error"));
        var controller = new CommentController(mockService.Object);

        var result = await controller.DeleteCommentAsync(1);

        Assert.IsType<BadRequestObjectResult>(result);
    }
}
