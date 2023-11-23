using MediatR;

namespace Application.Abstractions.Idempotency;

public abstract record IdempotencyCommand(
    Guid RequestId) : IRequest;
