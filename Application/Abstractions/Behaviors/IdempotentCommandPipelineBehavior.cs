using Application.Abstractions.Idempotency;
using MediatR;

namespace Application.Abstractions.Behaviors;

internal sealed class IdempotentCommandPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IdempotencyCommand
{
    private readonly IIdempotencyService _idempotencyService;

    public IdempotentCommandPipelineBehavior(IIdempotencyService idempotencyService)
    {
        _idempotencyService = idempotencyService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (await _idempotencyService.RequestExistAsync(request.RequestId))
        {
            return default;
        }

        await _idempotencyService.CreateRequestAsync(request.RequestId, typeof(TRequest).Name);

        return await next();
    }
}
