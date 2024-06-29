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
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PasswordValidator(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;

        RuleFor(x => x)
            .MustAsync(PasswordCorrect)
            .WithMessage("Old password is incorrect.");
    }

    private async Task<bool> PasswordCorrect(string password, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User
            .FindFirstValue(ClaimTypes.NameIdentifier);
        Guard.Against.NullOrEmpty(userId);
        var user = await _userManager.FindByIdAsync(userId);
        Guard.Against.NotFound(userId, user);
        return await _userManager.CheckPasswordAsync(user, password);
    }
}
