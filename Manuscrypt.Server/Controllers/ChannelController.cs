using Microsoft.AspNetCore.Mvc;
using Manuscrypt.Server.Data.DTOs;
using Manuscrypt.Server.Services;

namespace Manuscrypt.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class ChannelController : ControllerBase
{
    private readonly ChannelService _channelService;

    public ChannelController(ChannelService channelService)
    {
        _channelService = channelService;
    }

    [HttpPost]
    public async Task<ActionResult<ChannelDTO>> CreateChannel([FromBody] ChannelDTO channelDto)
    {
        if (channelDto == null)
        {
            return BadRequest("Post data is required.");
        }

        ChannelDTO createdChannel = await _channelService.CreateChannelAsync(channelDto);

        if (createdChannel == null)
        {
            return StatusCode(500, "A problem happened with creating the post.");
        }

        return CreatedAtAction(nameof(CreateChannel), new { id = createdChannel.Id }, createdChannel);
    }
}
