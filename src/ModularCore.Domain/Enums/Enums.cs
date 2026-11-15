namespace ModularCore.Domain.Enums;
public enum UserRole { Admin, Manager, Agent, Customer }
public enum ServiceRequestStatus { Open, InProgress, PendingCustomer, Resolved, Closed }
public enum ServiceRequestPriority { Low, Medium, High, Critical }