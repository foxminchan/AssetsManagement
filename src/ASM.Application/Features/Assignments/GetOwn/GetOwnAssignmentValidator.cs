using FluentValidation;

namespace ASM.Application.Features.Assignments.GetOwn;

public sealed class GetOwnAssignmentValidator : AbstractValidator<GetOwnAssignmentQuery>
{
    public GetOwnAssignmentValidator() => RuleFor(x => x.Id).NotEmpty();
}
