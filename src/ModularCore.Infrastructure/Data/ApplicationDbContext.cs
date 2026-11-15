using Microsoft.EntityFrameworkCore;
using ModularCore.Application.Common.Interfaces;
using ModularCore.Domain.Entities;
namespace ModularCore.Infrastructure.Data;
public class ApplicationDbContext : DbContext, IApplicationDbContext {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<Customer> Customers => Set<Customer>(); public DbSet<ServiceRequest> ServiceRequests => Set<ServiceRequest>();
    public DbSet<Team> Teams => Set<Team>(); public DbSet<User> Users => Set<User>();
    public DbSet<Comment> Comments => Set<Comment>(); public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ServiceRequest>().HasOne(s => s.Customer).WithMany().HasForeignKey(s => s.CustomerId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<ServiceRequest>().HasOne(s => s.AssignedTeam).WithMany(t => t.ServiceRequests).HasForeignKey(s => s.AssignedTeamId).OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
    }
}