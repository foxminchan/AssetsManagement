using FluentValidation;

namespace ASM.Application.Features.ReturningRequests.List;

public sealed class ListReturningRequestValidator : AbstractValidator<ListReturningRequestQuery>
{
    public ListReturningRequestValidator()
    {
        RuleFor(x => x.State).IsInEnum();
        RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1);
    }
}
