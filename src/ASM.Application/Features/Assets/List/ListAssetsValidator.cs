using ASM.Application.Domain.AssetAggregate.Enums;
using FluentValidation;

namespace ASM.Application.Features.Assets.List;

public sealed class ListAssetsValidator : AbstractValidator<ListAssetsQuery>
{
    public ListAssetsValidator()
    {
        RuleFor(x => x.State).ForEach(x => x.IsInEnum());
        RuleFor(x => x.PageIndex).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}
