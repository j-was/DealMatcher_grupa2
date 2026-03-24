using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealMatcher.Backend.Core.Aggregates.Offer;
public static class OfferConstants
{
    public const int TitleMaxLength = 100;
    public const int DescriptionMaxLength = 500;
    public const int TagMaxLength = 50;
    public const int MaxTagsCount = 20;
    public const int ImageUrlMaxLength = 200;
    public const int MaxImagesCount = 10;
    public const int MinImagesCount = 1;
    public const int PropertyNameMaxLength = 100;
    public const int PropertyMaxValue = 500;
}
