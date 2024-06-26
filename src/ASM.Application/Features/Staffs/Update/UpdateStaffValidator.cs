using FluentValidation;

namespace ASM.Application.Features.Staffs.Update;

public sealed class UpdateStaffValidator : AbstractValidator<UpdateStaffCommand>
{
    public UpdateStaffValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required");

        RuleFor(x => x.Dob)
            .NotEmpty()
            .WithMessage("Date of birth name is required.")
            .Must(x => x.AddYears(18) <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("User is under 18. Please select a different date");

        RuleFor(x => x.JoinedDate)
            .NotEmpty()
            .WithMessage("Join day is required.")
            .Must(x => x.DayOfWeek is not DayOfWeek.Saturday or DayOfWeek.Sunday)
            .WithMessage("Joined date is Saturday or Sunday. Please select a different date")
            .GreaterThan(x => x.Dob)
            .WithMessage("Joined date is not later than Date of Birth. Please select a different date");

        RuleFor(x => x.Gender)
            .IsInEnum();

        RuleFor(x => x.RoleType)
            .IsInEnum();
    }
}
