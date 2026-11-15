namespace ModularCore.Application.Customers.Commands.CreateCustomer;
public record CustomerDto(Guid Id, string CompanyName, string ContactName, string Email, string Phone);