namespace ASM.Application.Features.Categories;

public sealed record CategoryDto(
    Guid Id,
    string? Name,
    string? Prefix);
