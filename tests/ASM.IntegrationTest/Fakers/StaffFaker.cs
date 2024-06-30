using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.IdentityAggregate.Enums;
using ASM.Application.Domain.Shared;
using Bogus;

namespace ASM.IntegrationTest.Fakers;

public sealed class StaffFaker : Faker<Staff>
{
    public StaffFaker()
    {
        RuleFor(x => x.Id, f => f.Random.Guid());
        RuleFor(x => x.Gender, f => f.PickRandom<Gender>());
        RuleFor(x => x.Location, f => f.PickRandom<Location>());
        RuleFor(x => x.FirstName, f => f.Person.FirstName);
        RuleFor(x => x.LastName, f => f.Person.LastName);
        RuleFor(x => x.Dob, f =>
        {
            var date = f.Person.DateOfBirth;
            return new(date.Year, date.Month, date.Day);
        });
        RuleFor(x => x.JoinedDate, f =>
        {
            var date = f.Date.Past(18, DateTime.Today.AddYears(-18).AddDays(-1));
            return new(date.Year, date.Month, date.Day);
        });
        RuleFor(x => x.StaffCode, f =>
        {
            var number = 9999 - f.UniqueIndex;
            return $"SD{number:D4}";
        });
        RuleFor(x => x.Users, f => [new() { UserName = f.Person.UserName }]);
    }
}
