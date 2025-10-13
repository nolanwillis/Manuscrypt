using Microsoft.AspNetCore.Mvc;
using Manuscrypt.Server.Data.DTOs;
using Manuscrypt.Server.Services;
using Manuscrypt.Server.Services.Exceptions;

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

        try
        {
            ChannelDTO createdChannel = await _channelService.CreateChannelAsync(channelDto);
            return CreatedAtAction(nameof(CreateChannel), new { id = createdChannel.Id }, createdChannel);
        }
        catch (ChannelNameTakenException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
