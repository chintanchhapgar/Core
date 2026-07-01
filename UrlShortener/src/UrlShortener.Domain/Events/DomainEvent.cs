using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener.Domain.Events;

public abstract class DomainEvent : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}