using Manuscrypt.Server.Data;
using Manuscrypt.Server.Data.DTOs;
using Manuscrypt.Server.Data.Models;
using Manuscrypt.Server.Data.Repositories;
using Manuscrypt.Server.Services.Exceptions;

namespace Manuscrypt.Server.Services;

public class ChannelService
{
    private readonly ManuscryptContext _context;
    private readonly ChannelRepo _channelRepo;

    public ChannelService(ManuscryptContext context, ChannelRepo channelRepo)
    {
        _context = context;
        _channelRepo = channelRepo;
    } 
}
