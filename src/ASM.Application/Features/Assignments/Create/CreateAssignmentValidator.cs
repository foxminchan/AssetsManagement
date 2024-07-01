using FluentValidation;

namespace ASM.Application.Features.Assignments.Create;

public class CreateAssignmentValidator : AbstractValidator<CreateAssignmentRequest>
{
    public CreateAssignmentValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required");

        RuleFor(x => x.AssetId)
            .NotEmpty()
            .WithMessage("AssetId is required");

        RuleFor(x => x.AssignedDate)
            .NotEmpty()
            .WithMessage("Assign Date is required")
            .Must(date => date >= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Assign Date must not be in the past");
    }
}
