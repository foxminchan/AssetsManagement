using FluentValidation;

namespace ASM.Application.Features.Assignments.Get;

public sealed class GetAssignmentValidator : AbstractValidator<GetAssignmentQuery>
{
    public GetAssignmentValidator() => RuleFor(x => x.Id).NotEmpty();
}
