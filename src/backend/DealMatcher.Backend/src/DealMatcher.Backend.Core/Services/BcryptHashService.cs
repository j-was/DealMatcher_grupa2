using DealMatcher.Backend.Core.Aggregates.User;
using DealMatcher.Backend.Core.Interfaces;

namespace DealMatcher.Backend.Core.Services;

public sealed class BcryptHashService : IPasswordHashService
{
    private const int BcryptWorkFactor = 12;

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, BcryptWorkFactor);
    }

    public bool AuthorizePassword(string hash, string password)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hash))
        {
            return false;
        }

        try
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
        catch (SaltParseException)
        {
            return false;
        }
    }


}
