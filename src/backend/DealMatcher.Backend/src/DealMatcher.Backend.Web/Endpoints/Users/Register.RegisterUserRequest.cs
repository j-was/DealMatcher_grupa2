namespace DealMatcher.Backend.Web.Endpoints.Users;

public sealed record class RegisterUserRequest(string Email, string Name, string Surname, string Password);
