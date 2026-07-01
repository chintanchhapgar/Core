using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener.Domain.Enums;

public enum ShortUrlStatus
{
    Active = 1,
    Disabled = 2,
    Expired = 3
}