using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModularCore.Application.ServiceRequests.Commands.AddComment;
using ModularCore.Application.ServiceRequests.Commands.AssignTeam;
using ModularCore.Application.ServiceRequests.Commands.ChangeStatus;
using ModularCore.Application.ServiceRequests.Commands.CreateServiceRequest;
using ModularCore.Application.ServiceRequests.Queries.SearchServiceRequests;
using ModularCore.Domain.Enums;

namespace ModularCore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceRequestsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ServiceRequestsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string? searchTerm, [FromQuery] ServiceRequestStatus? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new SearchServiceRequestsQuery(searchTerm, status, page, pageSize));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateServiceRequestCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(new { id });
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] ChangeStatusRequest request)
    {
        await _mediator.Send(new ChangeStatusCommand(id, request.NewStatus, request.ChangedBy));
        return NoContent();
    }

    [HttpPatch("{id}/assign")]
    public async Task<IActionResult> AssignTeam(Guid id, [FromBody] AssignTeamRequest request)
    {
        await _mediator.Send(new AssignTeamCommand(id, request.TeamId));
        return NoContent();
    }

    [HttpPost("{id}/comments")]
    public async Task<IActionResult> AddComment(Guid id, [FromBody] AddCommentRequest request)
    {
        var commentId = await _mediator.Send(new AddCommentCommand(id, request.Content, request.AuthorName, request.IsInternal));
        return Ok(new { commentId });
    }
}

public record ChangeStatusRequest(ServiceRequestStatus NewStatus, string ChangedBy);
public record AssignTeamRequest(Guid TeamId);
public record AddCommentRequest(string Content, string AuthorName, bool IsInternal);