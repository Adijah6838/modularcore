using MediatR;
using Microsoft.EntityFrameworkCore;
using ModularCore.Application.Common.Interfaces;
using ModularCore.Domain.Enums;
namespace ModularCore.Application.ServiceRequests.Queries.SearchServiceRequests;

public record ServiceRequestSummaryDto(Guid Id, string Title, string Status, string Priority, string CustomerName);

public record SearchServiceRequestsQuery(string? SearchTerm, ServiceRequestStatus? Status, int Page, int PageSize) : IRequest<List<ServiceRequestSummaryDto>>;

public class SearchServiceRequestsQueryHandler : IRequestHandler<SearchServiceRequestsQuery, List<ServiceRequestSummaryDto>>
{
    private readonly IApplicationDbContext _context;
    public SearchServiceRequestsQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<List<ServiceRequestSummaryDto>> Handle(SearchServiceRequestsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.ServiceRequests.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            query = query.Where(x => x.Title.Contains(request.SearchTerm));

        if (request.Status.HasValue)
            query = query.Where(x => x.Status == request.Status.Value);

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new ServiceRequestSummaryDto(
                x.Id, x.Title, x.Status.ToString(), x.Priority.ToString(), x.Customer!.CompanyName
            ))
            .ToListAsync(cancellationToken);
    }
}