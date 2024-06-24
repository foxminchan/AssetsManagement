using ASM.Application.Infrastructure.Persistence;
using FluentValidation;

namespace ASM.Application.Features.Staffs.Create;

public sealed class CreateStaffValidator : AbstractValidator<CreateStaffRequest>
{
    public CreateStaffValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(DataSchemaLength.Small);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(DataSchemaLength.Medium);

        RuleFor(x => x.Dob)
            .NotEmpty().WithMessage("Date of birth name is required")
            .Must(x => x.AddYears(18) <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("User is under 18. Please select a different date");

        RuleFor(x => x.JoinedDate)
            .NotEmpty().WithMessage("Join day is required")
            .Must(x => x.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
            .WithMessage("Joined date is Saturday or Sunday. Please select a different date")
            .GreaterThan(x => x.Dob)
            .WithMessage("Joined date must be later than Date of Birth. Please select a different date");

        RuleFor(x => x.Gender)
            .IsInEnum();

        RuleFor(x => x.RoleType)
            .IsInEnum();
    }
}
