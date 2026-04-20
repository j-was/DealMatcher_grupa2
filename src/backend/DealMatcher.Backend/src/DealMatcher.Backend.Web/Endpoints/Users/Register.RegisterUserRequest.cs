namespace DealMatcher.Backend.Web.Endpoints.Users;

public sealed record RegisterUserRequest(string Email, string Name, string Surname, string Password);
