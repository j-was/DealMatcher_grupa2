using DealMatcher.Backend.Core.Aggregates.User;

namespace DealMatcher.Backend.Core.Interfaces;

public interface ITokenProvider
{
    public string GenerateToken(User user);
    public Task<bool> ValidateTokenAsync(string token);
}
