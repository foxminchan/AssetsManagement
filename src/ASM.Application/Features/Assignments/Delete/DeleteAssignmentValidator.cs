using FluentValidation;

namespace ASM.Application.Features.Assignments.Delete;

public sealed class DeleteAssignmentValidator : AbstractValidator<DeleteAssignmentCommand>
{
    public DeleteAssignmentValidator() => RuleFor(x => x.Id).NotEmpty();
}
