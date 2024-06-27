using FluentValidation;

namespace ASM.Application.Features.Assignments.UpdateState;

public sealed class UpdateAssignmentStateValidator : AbstractValidator<UpdateAssignmentStateCommand>
{
    public UpdateAssignmentStateValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.State).IsInEnum();
    }
}
