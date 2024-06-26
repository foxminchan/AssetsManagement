using ASM.Application.Infrastructure.Persistence;
using FluentValidation;

namespace ASM.Application.Features.Categories.Create;

public sealed class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(DataSchemaLength.Large);

        RuleFor(x => x.Prefix)
            .NotEmpty()
            .WithMessage("Prefix is required")
            .MaximumLength(DataSchemaLength.Tiny);
    }
}
