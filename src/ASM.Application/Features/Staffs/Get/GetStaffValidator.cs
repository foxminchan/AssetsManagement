using FluentValidation;

namespace ASM.Application.Features.Staffs.Get;

public sealed class GetStaffValidator : AbstractValidator<GetStaffQuery>
{
    public GetStaffValidator() => RuleFor(x => x.Id).NotEmpty();
}
