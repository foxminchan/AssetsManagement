using ASM.Application.Domain.AssetAggregate.Enums;
using ASM.Application.Infrastructure.Persistence;
using FluentValidation;

namespace ASM.Application.Features.Assets.Update;

public sealed class UpdateAssetValidator : AbstractValidator<UpdateAssetCommand>
{
    public UpdateAssetValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(DataSchemaLength.SuperLarge);

        RuleFor(x => x.Specification)
            .MaximumLength(DataSchemaLength.Max);

        RuleFor(x => x.InstalledDate)
            .NotEmpty()
            .WithMessage("Install date is required")
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));

        RuleFor(x => x.State)
            .IsInEnum()
            .Must(x => x != State.Assigned);
    }
}
