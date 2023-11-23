using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Idempotency;

namespace Persistence.Configuration;

internal sealed class IdempotentRequestConfiguration : IEntityTypeConfiguration<IdempotentRequest>
{
    public void Configure(EntityTypeBuilder<IdempotentRequest> builder)
    {
        builder.ToTable("idempotent_request");

        builder.HasKey(ir => ir.Id);

        builder.Property(ir => ir.Name).IsRequired();
    }
}
