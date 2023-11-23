using Application.Abstractions.Idempotency;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Idempotency;

internal sealed class IdempotencyService : IIdempotencyService
{
    private readonly ApplicationDbContext _context;

    public IdempotencyService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> RequestExistAsync(Guid requestId)
    {
        return await _context.Set<IdempotentRequest>().AnyAsync(r => r.Id == requestId);
    }

    public async Task CreateRequestAsync(Guid requestId, string name)
    {
        var idempotencyRequest = new IdempotentRequest
        {
            Id = requestId,
            Name = name
        };

        _context.Add(idempotencyRequest);

        await _context.SaveChangesAsync();
    }
}
