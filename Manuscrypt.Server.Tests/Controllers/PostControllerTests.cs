using Manuscrypt.Server.Controllers;
using Manuscrypt.Server.Data.DTOs.Post;
using Manuscrypt.Server.Data.DTOs.Comment;
using Manuscrypt.Server.Services;
using Manuscrypt.Server.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Manuscrypt.Server.Tests.Controllers;

public class PostControllerTests
{ 
    [Fact]
    public async Task GetPostAsync_ReturnsOk_WhenFound()
    {
        var mockService = new Mock<PostService>(null, null);
        var expected = new GetPostDTO { Id = 1 };
        mockService.Setup(s => s.GetPostAsync(1)).ReturnsAsync(expected);
        var controller = new PostController(mockService.Object);

        var result = await controller.GetPostAsync(1);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsType<GetPostDTO>(ok.Value);
        Assert.Equal(1, value.Id);
    }

    [Fact]
    public async Task GetPostAsync_ReturnsNotFound_WhenNotFound()
    {
        var mockService = new Mock<PostService>(null, null);
        mockService.Setup(s => s.GetPostAsync(1)).ThrowsAsync(new PostDoesNotExistException(1));
        var controller = new PostController(mockService.Object);

        var result = await controller.GetPostAsync(1);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetPostAsync_ReturnsBadRequest_OnException()
    {
        var mockService = new Mock<PostService>(null, null);
        mockService.Setup(s => s.GetPostAsync(1)).ThrowsAsync(new Exception());
        var controller = new PostController(mockService.Object);

        var result = await controller.GetPostAsync(1);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetCommentsForPostAsync_ReturnsOk_WithComments()
    {
        var mockService = new Mock<PostService>(null, null);
        var expected = new List<GetCommentDTO> { new GetCommentDTO { Id = 1 } };
        mockService.Setup(s => s.GetCommentsForPostAsync(1)).ReturnsAsync(expected);
        var controller = new PostController(mockService.Object);

        var result = await controller.GetCommentsForPostAsync(1);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsAssignableFrom<IEnumerable<GetCommentDTO>>(ok.Value);
        Assert.Single(value);
    }

    [Fact]
    public async Task GetCommentsForPostAsync_ReturnsBadRequest_OnException()
    {
        var mockService = new Mock<PostService>(null, null);
        mockService.Setup(s => s.GetCommentsForPostAsync(1)).ThrowsAsync(new Exception());
        var controller = new PostController(mockService.Object);

        var result = await controller.GetCommentsForPostAsync(1);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreatePostAsync_ReturnsCreated_WhenValid()
    {
        var mockService = new Mock<PostService>(null, null);
        var createDto = new CreatePostDTO { Title = "New post" };
        mockService.Setup(s => s.CreatePostAsync(createDto)).ReturnsAsync(42);
        var controller = new PostController(mockService.Object);

        var result = await controller.CreatePostAsync(createDto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(42, createdResult.RouteValues["id"]);
    }

    [Fact]
    public async Task CreatePostAsync_ReturnsBadRequest_WhenNull()
    {
        var mockService = new Mock<PostService>(null, null);
        var controller = new PostController(mockService.Object);

        var result = await controller.CreatePostAsync(null);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreatePostAsync_ReturnsBadRequest_OnException()
    {
        var mockService = new Mock<PostService>(null, null);
        var createDto = new CreatePostDTO { Title = "New post" };
        mockService.Setup(s => s.CreatePostAsync(createDto)).ThrowsAsync(new Exception());
        var controller = new PostController(mockService.Object);

        var result = await controller.CreatePostAsync(createDto);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdatePostAsync_ReturnsNoContent_WhenValid()
    {
        var mockService = new Mock<PostService>(null, null);
        var updateDto = new UpdatePostDTO { Id = 1, Title = "Updated" };
        mockService.Setup(s => s.UpdatePostAsync(updateDto)).Returns(Task.CompletedTask);
        var controller = new PostController(mockService.Object);

        var result = await controller.UpdatePostAsync(updateDto);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdatePostAsync_ReturnsBadRequest_WhenNull()
    {
        var mockService = new Mock<PostService>(null, null);
        var controller = new PostController(mockService.Object);

        var result = await controller.UpdatePostAsync(null);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdatePostAsync_ReturnsNotFound_WhenNotFound()
    {
        var mockService = new Mock<PostService>(null, null);
        var updateDto = new UpdatePostDTO { Id = 1, Title = "Updated" };
        mockService.Setup(s => s.UpdatePostAsync(updateDto)).ThrowsAsync(new PostDoesNotExistException(1));
        var controller = new PostController(mockService.Object);

        var result = await controller.UpdatePostAsync(updateDto);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task UpdatePostAsync_ReturnsBadRequest_OnException()
    {
        var mockService = new Mock<PostService>(null, null);
        var updateDto = new UpdatePostDTO { Id = 1, Title = "Updated" };
        mockService.Setup(s => s.UpdatePostAsync(updateDto)).ThrowsAsync(new Exception());
        var controller = new PostController(mockService.Object);

        var result = await controller.UpdatePostAsync(updateDto);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task DeletePostAsync_ReturnsNoContent_WhenValid()
    {
        var mockService = new Mock<PostService>(null, null);
        mockService.Setup(s => s.DeletePostAsync(1)).Returns(Task.CompletedTask);
        var controller = new PostController(mockService.Object);

        var result = await controller.DeletePostAsync(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeletePostAsync_ReturnsNotFound_WhenNotFound()
    {
        var mockService = new Mock<PostService>(null, null);
        mockService.Setup(s => s.DeletePostAsync(1)).ThrowsAsync(new PostDoesNotExistException(1));
        var controller = new PostController(mockService.Object);

        var result = await controller.DeletePostAsync(1);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task DeletePostAsync_ReturnsBadRequest_OnException()
    {
        var mockService = new Mock<PostService>(null, null);
        mockService.Setup(s => s.DeletePostAsync(1)).ThrowsAsync(new Exception());
        var controller = new PostController(mockService.Object);

        var result = await controller.DeletePostAsync(1);

        Assert.IsType<BadRequestObjectResult>(result);
    }
}