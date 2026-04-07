using DealMatcher.Backend.Core.Aggregates.User;

namespace DealMatcher.Backend.Core.Interfaces;

public interface IPasswordHashService
{
    public string HashPassword(string password);
    public bool AuthorizePassword(string hash, string password);
}
