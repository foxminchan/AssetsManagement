using FluentValidation;

namespace ASM.Application.Features.ReturningRequests.Complete;

public sealed class CompleteReturningRequestValidator : AbstractValidator<CompleteReturningRequestCommand>
{
    public CompleteReturningRequestValidator() => RuleFor(x => x.Id).NotEmpty();
}
