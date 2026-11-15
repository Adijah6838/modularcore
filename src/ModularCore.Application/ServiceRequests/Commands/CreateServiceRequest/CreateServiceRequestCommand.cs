using MediatR;
using ModularCore.Application.Common.Interfaces;
using ModularCore.Domain.Entities;
using ModularCore.Domain.Enums;
using ModularCore.Domain.Exceptions;
namespace ModularCore.Application.ServiceRequests.Commands.CreateServiceRequest;

public record CreateServiceRequestCommand(string Title, string Description, Guid CustomerId, ServiceRequestPriority Priority) : IRequest<Guid>;

public class CreateServiceRequestCommandHandler : IRequestHandler<CreateServiceRequestCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public CreateServiceRequestCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<Guid> Handle(CreateServiceRequestCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            throw new DomainValidationException("Title is required.");

        var customerExists = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.AnyAsync(_context.Customers, c => c.Id == request.CustomerId, cancellationToken);
        if (!customerExists)
            throw new NotFoundException("Customer not found.");

        // 🟢 Status defaults to Open inside the ServiceRequest entity
        var serviceRequest = new ServiceRequest
        {
            Title = request.Title,
            Description = request.Description,
            CustomerId = request.CustomerId,
            Priority = request.Priority
        };

        _context.ServiceRequests.Add(serviceRequest);
        await _context.SaveChangesAsync(cancellationToken);
        return serviceRequest.Id;
    }
}