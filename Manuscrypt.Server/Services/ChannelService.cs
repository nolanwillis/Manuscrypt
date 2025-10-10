using Manuscrypt.Server.Data;
using Manuscrypt.Server.Data.DTOs;
using Manuscrypt.Server.Data.Models;
using Manuscrypt.Server.Data.Repositories;

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

    public async Task<ChannelDTO> CreateChannelAsync(ChannelDTO channelDto)
    {
        // Map DTO to Post entity.
        var channel = new Channel
        {
            Name = channelDto.Name,
            Description = channelDto?.Description ?? "",
            CreatedAt = DateTime.UtcNow
        };

        // Use ChannelRepo to add and save to the database.
        await _channelRepo.AddAsync(channel);
        await _context.SaveChangesAsync();

        // Create the return DTO.
        var channelDTO = new ChannelDTO
        {
            Id = channel.Id,
            Name = channel.Name,
            Description = channel.Description,
            CreatedAt = channel.CreatedAt
        };

        return channelDTO;
    }
}
