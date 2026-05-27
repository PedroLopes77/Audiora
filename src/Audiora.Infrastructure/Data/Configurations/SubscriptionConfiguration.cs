using Audiora.Domain.Entities;
using Audiora.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Audiora.Infrastructure.Data.Configurations;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.ToTable("Subscriptions");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Plan).HasConversion<int>();
        builder.Property(s => s.Price).HasPrecision(18, 2);

        builder.HasIndex(s => s.UserId)
            .IsUnique()
            .HasDatabaseName("IX_Subscriptions_UserId");
    }
}