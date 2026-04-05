using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealMatcher.Backend.Core.Aggregates.Category.DTOs;

public sealed record CategoryPropertyDTO(
    int Id,
    string Name,
    string? Description
    );
