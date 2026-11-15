using ModularCore.Domain.Entities;
using ModularCore.Domain.Enums;
using ModularCore.Domain.Exceptions;
using Xunit;

namespace ModularCore.Tests;

public class ServiceRequestDomainTests
{
    [Fact]
    public void ChangeStatus_ShouldUpdateStatus_WhenValid()
    {
        // Arrange
        var sr = new ServiceRequest { Title = "Test" };

        // Act
        sr.ChangeStatus(ServiceRequestStatus.InProgress, "admin");

        // Assert
        Assert.Equal(ServiceRequestStatus.InProgress, sr.Status);
        Assert.Single(sr.AuditLogs);
    }

    [Fact]
    public void ChangeStatus_ShouldThrow_WhenClosed()
    {
        // Arrange
        var sr = new ServiceRequest { Title = "Test" };
        sr.ChangeStatus(ServiceRequestStatus.Closed, "admin");

        // Act & Assert
        var exception = Assert.Throws<InvalidStatusTransitionException>(() => 
            sr.ChangeStatus(ServiceRequestStatus.InProgress, "admin"));
        
        Assert.Contains("Cannot modify a Closed", exception.Message);
    }
}