using MediatR;
using ModularCore.Application.Common.Interfaces;
using ModularCore.Domain.Entities;
using ModularCore.Domain.Exceptions;
namespace ModularCore.Application.Customers.Commands.CreateCustomer;
public record CreateCustomerCommand(string CompanyName, string ContactName, string Email, string Phone) : IRequest<CustomerDto>;
public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDto> {
    private readonly IApplicationDbContext _context;
    public CreateCustomerCommandHandler(IApplicationDbContext context) { _context = context; }
    public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken) {
        if (string.IsNullOrWhiteSpace(request.CompanyName)) throw new DomainValidationException("Company name is required.");
        var customer = new Customer { CompanyName = request.CompanyName, ContactName = request.ContactName, Email = request.Email, Phone = request.Phone };
        _context.Customers.Add(customer); await _context.SaveChangesAsync(cancellationToken);
        return new CustomerDto(customer.Id, customer.CompanyName, customer.ContactName, customer.Email, customer.Phone);
    }
}