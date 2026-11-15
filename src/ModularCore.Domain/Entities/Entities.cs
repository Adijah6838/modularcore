using ModularCore.Domain.Enums;
using ModularCore.Domain.Exceptions;
namespace ModularCore.Domain.Entities;
public abstract class BaseEntity { public Guid Id { get; set; } = Guid.NewGuid(); public DateTime CreatedAt { get; set; } = DateTime.UtcNow; public DateTime? UpdatedAt { get; set; } }
public class User : BaseEntity { public string Email { get; set; } = string.Empty; public string PasswordHash { get; set; } = string.Empty; public string FirstName { get; set; } = string.Empty; public string LastName { get; set; } = string.Empty; public UserRole Role { get; set; } = UserRole.Customer; }
public class Customer : BaseEntity { public string CompanyName { get; set; } = string.Empty; public string ContactName { get; set; } = string.Empty; public string Email { get; set; } = string.Empty; public string Phone { get; set; } = string.Empty; }
public class Team : BaseEntity { public string Name { get; set; } = string.Empty; public string Description { get; set; } = string.Empty; public ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>(); }
public class ServiceRequest : BaseEntity {
    public string Title { get; set; } = string.Empty; public string Description { get; set; } = string.Empty;
    public ServiceRequestStatus Status { get; private set; } = ServiceRequestStatus.Open; public ServiceRequestPriority Priority { get; set; } = ServiceRequestPriority.Medium;
    public Guid CustomerId { get; set; } public Customer? Customer { get; set; }
    public Guid? AssignedTeamId { get; set; } public Team? AssignedTeam { get; set; }
    public ICollection<Comment> Comments { get; set; } = new List<Comment>(); public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
    public void ChangeStatus(ServiceRequestStatus newStatus, string changedBy) {
        if (Status == newStatus) return;
        if (Status == ServiceRequestStatus.Closed) throw new InvalidStatusTransitionException("Cannot modify a Closed service request.");
        if (Status == ServiceRequestStatus.Resolved && newStatus == ServiceRequestStatus.Open) throw new InvalidStatusTransitionException("Resolved requests cannot be moved back to Open.");
        var oldStatus = Status; Status = newStatus; UpdatedAt = DateTime.UtcNow;
        AuditLogs.Add(new AuditLog { Action = "Status Changed", Details = $"Status changed from {oldStatus} to {newStatus} by {changedBy}" });
    }
}
public class Comment : BaseEntity { public Guid ServiceRequestId { get; set; } public string Content { get; set; } = string.Empty; public string AuthorName { get; set; } = string.Empty; public bool IsInternal { get; set; } }
public class AuditLog : BaseEntity { public Guid ServiceRequestId { get; set; } public string Action { get; set; } = string.Empty; public string Details { get; set; } = string.Empty; }