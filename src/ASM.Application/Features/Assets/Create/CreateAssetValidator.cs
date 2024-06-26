using ASM.Application.Domain.AssetAggregate.Enums;
using ASM.Application.Infrastructure.Persistence;
using FluentValidation;

namespace ASM.Application.Features.Assets.Create;

public sealed class CreateAssetValidator : AbstractValidator<CreateAssetCommand>
{
    public CreateAssetValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(DataSchemaLength.SuperLarge);

        RuleFor(x => x.Specification)
            .MaximumLength(DataSchemaLength.Max);

        RuleFor(x => x.InstallDate)
            .NotEmpty()
            .WithMessage("Install date is required")
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Category is required");

        RuleFor(x => x.State)
            .IsInEnum()
            .Must(x => x is State.Available or State.NotAvailable);
    }
}
