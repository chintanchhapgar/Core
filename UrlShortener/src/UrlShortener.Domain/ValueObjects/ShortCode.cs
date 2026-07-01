using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener.Domain.ValueObjects;

public sealed record ShortCode(string Value)
{
    public override string ToString() => Value;
}