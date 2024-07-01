using FluentValidation;

namespace ASM.Application.Features.Assignments.Update;

public sealed class UpdateAssignmentValidator : AbstractValidator<UpdateAssignmentRequest>
{
    public UpdateAssignmentValidator()
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
