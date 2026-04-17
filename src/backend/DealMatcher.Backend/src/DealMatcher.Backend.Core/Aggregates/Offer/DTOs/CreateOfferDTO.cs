namespace DealMatcher.Backend.Core.Aggregates.Offer.DTOs;
public class CreateOfferDTO
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public double Price { get; init; }
    public List<string> Tags { get; init; } = [];
    public int CategoryId { get; init; }
    public Dictionary<string, string> Properties { get; init; } = [];
    public int Availability { get; init; }
}