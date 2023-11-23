namespace Application.Abstractions.Idempotency;

public interface IIdempotencyService
{
    Task<bool> RequestExistAsync(Guid requestId);

    Task CreateRequestAsync(Guid requestId, string name);
}
