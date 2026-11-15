using MediatR;
using ModularCore.Application.Common.Interfaces;
using ModularCore.Domain.Enums;
using ModularCore.Domain.Exceptions;
namespace ModularCore.Application.ServiceRequests.Commands.ChangeStatus;

public record ChangeStatusCommand(Guid ServiceRequestId, ServiceRequestStatus NewStatus, string ChangedBy) : IRequest<Unit>;

public class ChangeStatusCommandHandler : IRequestHandler<ChangeStatusCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    public ChangeStatusCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<Unit> Handle(ChangeStatusCommand request, CancellationToken cancellationToken)
    {
        var sr = await _context.ServiceRequests.FindAsync(new object[] { request.ServiceRequestId }, cancellationToken);
        if (sr == null) throw new NotFoundException("Service request not found.");

        // Calls the Domain method that enforces business rules!
        sr.ChangeStatus(request.NewStatus, request.ChangedBy);
        
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}