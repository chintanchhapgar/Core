using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Persistence.Configurations;

public sealed class ShortUrlConfiguration : IEntityTypeConfiguration<ShortUrl>
{
    public void Configure(EntityTypeBuilder<ShortUrl> builder)
    {
        builder.ToTable("ShortUrls");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.OriginalUrl)
            .HasMaxLength(2048)
            .IsRequired();

        builder.Property(x => x.ShortCode)
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(x => x.ShortCode)
            .IsUnique();

        builder.Property(x => x.ClickCount)
            .HasDefaultValue(0);

        builder.Property(x => x.CreatedOnUtc)
            .IsRequired();

        builder.Property(x => x.ExpiresOnUtc);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);
    }
}