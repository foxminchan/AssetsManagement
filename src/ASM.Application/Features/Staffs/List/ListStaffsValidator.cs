using FluentValidation;

namespace ASM.Application.Features.Staffs.List;

public sealed class ListStaffsValidator : AbstractValidator<ListStaffsQuery>
{
    public ListStaffsValidator()
    {
        RuleFor(x => x.RoleType).IsInEnum();
        RuleFor(x => x.PageIndex).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}
