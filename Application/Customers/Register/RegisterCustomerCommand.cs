using Application.Abstractions.Messaging;

namespace Application.Customers.Create;

public record RegisterCustomerCommand(string Email, string Password, string Name) : ICommand;

public record RegisterCustomerRequest(string Email, string Password, string Name);
