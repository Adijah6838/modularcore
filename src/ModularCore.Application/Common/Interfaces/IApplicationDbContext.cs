using Microsoft.EntityFrameworkCore;
using ModularCore.Domain.Entities;
namespace ModularCore.Application.Common.Interfaces;
public interface IApplicationDbContext {
    DbSet<Customer> Customers { get; } DbSet<ServiceRequest> ServiceRequests { get; } DbSet<Team> Teams { get; }
    DbSet<User> Users { get; } DbSet<Comment> Comments { get; } DbSet<AuditLog> AuditLogs { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}