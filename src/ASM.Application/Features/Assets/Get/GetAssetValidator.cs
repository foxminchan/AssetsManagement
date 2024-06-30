using FluentValidation;

namespace ASM.Application.Features.Assets.Get;

public sealed class GetAssetValidator : AbstractValidator<GetAssetQuery>
{
    public GetAssetValidator() => RuleFor(x => x.Id).NotEmpty();
}
