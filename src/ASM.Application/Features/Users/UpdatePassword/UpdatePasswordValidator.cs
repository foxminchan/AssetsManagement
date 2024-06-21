using System.Security.Claims;
using Ardalis.GuardClauses;
using ASM.Application.Domain.IdentityAggregate;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ASM.Application.Features.Users.UpdatePassword;

public sealed class UpdatePasswordValidator : AbstractValidator<UpdatePasswordCommand>
{
    public UpdatePasswordValidator(PasswordValidator passwordValidator)
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.OldPassword)
            .NotEmpty()
            .SetValidator(passwordValidator);

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(8)
            .WithMessage("New password must contain at least 8 characters.")
            .NotEqual(x => x.OldPassword)
            .WithMessage("New password must not be the same as old password.")
            .Matches(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
            .WithMessage("New password must contain at least one letter, one number, and one special character (@$!%*?&).");
    }
}

public sealed class PasswordValidator : AbstractValidator<string>
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IHttpContextAccessor httpContextAccessor;

    public PasswordValidator(UserManager<ApplicationUser> _userManager, IHttpContextAccessor _httpContextAccessor)
    {
        userManager = _userManager;
        httpContextAccessor = _httpContextAccessor;

        RuleFor(x => x)
            .MustAsync(PasswordCorrect)
            .WithMessage("Old password is incorrect.");
    }

    private async Task<bool> PasswordCorrect(string password, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext?.User
            .FindFirstValue(ClaimTypes.NameIdentifier);
        Guard.Against.NullOrEmpty(userId);
        var user = await userManager.FindByIdAsync(userId);
        Guard.Against.NotFound(userId, user);
        return await userManager.CheckPasswordAsync(user, password);
    }
}
