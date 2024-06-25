using FluentValidation;

namespace ASM.Application.Features.Assignments.List;

public sealed class ListAssignmentsValidator : AbstractValidator<ListAssignmentsQuery>
{
    public ListAssignmentsValidator()
    {
        RuleFor(x => x.State).IsInEnum();
        RuleFor(x => x.PageIndex).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}
