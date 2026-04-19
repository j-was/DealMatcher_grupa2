namespace DealMatcher.Backend.UseCases.Features.User.Login;

public sealed record LoginDTO(string AccessToken, UserDTO User);
