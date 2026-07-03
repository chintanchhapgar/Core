using UrlShortener.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreatedOnUtc { get; protected set; }

    public DateTime? UpdatedOnUtc { get; protected set; }

    public Guid? CreatedBy { get; protected set; }

    public Guid? UpdatedBy { get; protected set; }

    public void SetCreated(Guid? userId)
    {
        CreatedOnUtc = DateTime.UtcNow;
        CreatedBy = userId;
    }

    public void SetUpdated(Guid? userId)
    {
        UpdatedOnUtc = DateTime.UtcNow;
        UpdatedBy = userId;
    }
}