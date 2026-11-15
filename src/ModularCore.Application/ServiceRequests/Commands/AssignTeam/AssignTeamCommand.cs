using MediatR;
using ModularCore.Application.Common.Interfaces;
using ModularCore.Domain.Exceptions;
namespace ModularCore.Application.ServiceRequests.Commands.AssignTeam;

public record AssignTeamCommand(Guid ServiceRequestId, Guid TeamId) : IRequest<Unit>;

public class AssignTeamCommandHandler : IRequestHandler<AssignTeamCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    public AssignTeamCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<Unit> Handle(AssignTeamCommand request, CancellationToken cancellationToken)
    {
        var sr = await _context.ServiceRequests.FindAsync(new object[] { request.ServiceRequestId }, cancellationToken);
        if (sr == null) throw new NotFoundException("Service request not found.");

        var team = await _context.Teams.FindAsync(new object[] { request.TeamId }, cancellationToken);
        if (team == null) throw new NotFoundException("Team not found.");

        sr.AssignedTeamId = request.TeamId;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}