namespace DealMatcher.Backend.UseCases.Features.User.Login;

public sealed record LoginDTO(string AccesToken, UserDTO User);
