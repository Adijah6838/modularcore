using MediatR;
using Microsoft.EntityFrameworkCore;
using ModularCore.Application.Common.Interfaces;
using ModularCore.Application.Customers.Commands.CreateCustomer;
namespace ModularCore.Application.Customers.Queries.GetAllCustomers;
public record GetAllCustomersQuery : IRequest<List<CustomerDto>>;
public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, List<CustomerDto>> {
    private readonly IApplicationDbContext _context;
    public GetAllCustomersQueryHandler(IApplicationDbContext context) { _context = context; }
    public async Task<List<CustomerDto>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken) {
        return await _context.Customers.AsNoTracking().Select(c => new CustomerDto(c.Id, c.CompanyName, c.ContactName, c.Email, c.Phone)).ToListAsync(cancellationToken);
    }
}