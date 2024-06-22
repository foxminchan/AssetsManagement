using System.Text.Json.Serialization;
using ASM.Application.Common.SeedWorks;
using ASM.Application.Domain.IdentityAggregate.Enums;
using Microsoft.AspNetCore.Identity;

namespace ASM.Application.Domain.IdentityAggregate;

public sealed class ApplicationUser : IdentityUser, IAggregateRoot
{
    public AccountStatus AccountStatus { get; set; }
    public Guid? StaffId { get; set; }
    [JsonIgnore] public Staff? Staff { get; set; }

    public static string GenerateUserName(string firstName, string lastName, List<ApplicationUser> users)
    {
        var firstNameWord = firstName.ToLowerInvariant();

        var lastNameWord = lastName.ToLowerInvariant().Split(' ').Select(x => x[0]);

        var baseUserName = $"{firstNameWord}{string.Join("", lastNameWord)}";
        var userName = baseUserName;

        var count = 1;

        while (users.Exists(x => x.UserName == userName))
        {
            userName = $"{baseUserName}{count}";
            count++;
        }

        return userName;
    }

    public static string GeneratePassword(string userName, DateOnly dob)
        => $"{userName}@{dob.Day:D2}{dob.Month:D2}{dob.Year}";
}
