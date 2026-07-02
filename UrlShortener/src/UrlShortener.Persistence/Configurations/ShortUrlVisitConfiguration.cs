using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Persistence.Configurations;

public sealed class ShortUrlVisitConfiguration
    : IEntityTypeConfiguration<ShortUrlVisit>
{
    public void Configure(EntityTypeBuilder<ShortUrlVisit> builder)
    {
        builder.ToTable("ShortUrlVisits");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.VisitedOnUtc)
            .IsRequired();

        builder.Property(x => x.IpAddress)
            .HasMaxLength(45);

        builder.Property(x => x.UserAgent)
            .HasMaxLength(512);

        builder.Property(x => x.Browser)
            .HasMaxLength(100);

        builder.Property(x => x.OperatingSystem)
            .HasMaxLength(100);

        builder.Property(x => x.Referrer)
            .HasMaxLength(2048);

        builder.HasIndex(x => x.ShortUrlId);

        builder.HasOne<ShortUrl>()
            .WithMany(x => x.Visits)
            .HasForeignKey(x => x.ShortUrlId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}