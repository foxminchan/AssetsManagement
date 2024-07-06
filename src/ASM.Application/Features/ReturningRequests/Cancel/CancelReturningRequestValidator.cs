using FluentValidation;

namespace ASM.Application.Features.ReturningRequests.Cancel;

public sealed class CancelReturningRequestValidator : AbstractValidator<CancelReturningRequestCommand>
{
    public CancelReturningRequestValidator() => RuleFor(x => x.Id).NotEmpty();
}
