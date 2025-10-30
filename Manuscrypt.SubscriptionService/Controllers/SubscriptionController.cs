using Manuscrypt.Shared.DTOs.Subscription;
using Manuscrypt.SubscriptionService.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Manuscrypt.SubscriptionService.Controllers;

[ApiController]
[Route("[controller]")]
public class SubscriptionController : ControllerBase
{
    private readonly SubscriptionDomainService _domainService;

    public SubscriptionController(SubscriptionDomainService domainService)
    {
        _domainService = domainService;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetSubscriptionDTO>> GetSubscriptionAsync(int id)
    {
        try
        {
            var subscriptionDTO = await _domainService.GetSubscriptionAsync(id);
            return Ok(subscriptionDTO);
        }
        catch (SubscriptionDoesNotExistException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("User/{id:int}/Subscriptions")]
    public async Task<ActionResult<IEnumerable<GetSubscriptionDTO>>> GetSubscriptionsForUserAsync(int id)
    {
        try
        {
            var getSubscriptionDTOs = await _domainService.GetSubscriptionsForUserAsync(id);
            return Ok(getSubscriptionDTOs);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("User/{id:int}/Subscribers")]
    public async Task<ActionResult<IEnumerable<GetSubscriptionDTO>>> GetSubscribersForUserAsync(int id)
    {
        try
        {
            var getSubscriptionDTOs = await _domainService.GetSubscribersForUserAsync(id);
            return Ok(getSubscriptionDTOs);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<GetSubscriptionDTO>> CreateSubscriptionAsync([FromBody] CreateSubscriptionDTO createSubscriptionDTO)
    {
        if (createSubscriptionDTO == null)
        {
            return BadRequest("Subscription data is required.");
        }

        try
        {
            var created = await _domainService.CreateSubscriptionAsync(createSubscriptionDTO);
            return Created($"/subscription/{created.Id}", created);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteSubscriptionAsync(int id)
    {
        try
        {
            await _domainService.DeleteSubscriptionAsync(id);
            return NoContent();
        }
        catch (SubscriptionDoesNotExistException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
