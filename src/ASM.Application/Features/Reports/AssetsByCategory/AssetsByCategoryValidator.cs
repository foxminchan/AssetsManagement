using FluentValidation;

namespace ASM.Application.Features.Reports.AssetsByCategory;

public sealed class AssetsByCategoryValidator : AbstractValidator<AssetsByCategoryQuery>
{
    public AssetsByCategoryValidator() => RuleFor(x => x.OrderBy).NotEmpty();
}
