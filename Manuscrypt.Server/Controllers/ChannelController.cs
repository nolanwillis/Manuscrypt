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
}
