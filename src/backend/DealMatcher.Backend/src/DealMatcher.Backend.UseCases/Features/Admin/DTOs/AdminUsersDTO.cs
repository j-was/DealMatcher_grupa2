namespace DealMatcher.Backend.UseCases.Features.Admin.DTOs;

public sealed record AdminUsersDTO(
    IReadOnlyCollection<UserDTO> Items,
    int Total,
    int Page,
    int Pages
);
