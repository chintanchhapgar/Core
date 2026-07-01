using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;

    public DateTime? ModifiedOnUtc { get; set; }
}