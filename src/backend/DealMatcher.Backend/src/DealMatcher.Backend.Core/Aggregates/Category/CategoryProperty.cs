using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealMatcher.Backend.Core.Aggregates.Category;

public sealed record CategoryProperty
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description = null;
}
