using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Domain.Customers;
using EntityFramework.Exceptions.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Customers.Create;

internal sealed class RegisterCustomerCommandHandler
    : IRequestHandler<RegisterCustomerCommand>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RegisterCustomerCommandHandler> _logger;

    public RegisterCustomerCommandHandler(ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork,
        ILogger<RegisterCustomerCommandHandler> logger,
        IAuthenticationService authenticationService)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _authenticationService = authenticationService;
    }

    public async Task Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var identityId = await _authenticationService.RegisterAsync(
                request.Email,
                request.Password);

            var customer = new Customer(
                new CustomerId(
                    Guid.NewGuid()), 
                request.Email, 
                request.Email, 
                identityId);

            _customerRepository.Add(customer);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (UniqueConstraintException e)
        {
            _logger.LogError(e.Message, e);

            throw;
        }
    }
}
