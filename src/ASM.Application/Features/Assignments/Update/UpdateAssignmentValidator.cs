using FluentValidation;

namespace ASM.Application.Features.Assignments.Update;

public sealed class UpdateAssignmentValidator : AbstractValidator<UpdateAssignmentCommand>
{
    public UpdateAssignmentValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required");

        RuleFor(x => x.AssetId)
            .NotEmpty()
            .WithMessage("AssetId is required");

        RuleFor(x => x.AssignedDate)
            .NotEmpty()
            .WithMessage("Assign Date is required");
    }
}
