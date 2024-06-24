using FluentValidation;

namespace ASM.Application.Features.Staffs.Delete;

public class DeleteStaffValidator : AbstractValidator<DeleteStaffRequest>
{
    public DeleteStaffValidator() => RuleFor(x => x.UserId).NotEmpty();
}
