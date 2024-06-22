using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.IdentityAggregate.Enums;
using Bogus;

namespace ASM.IntegrationTest.Fakers;

public sealed class StaffFaker : Faker<Staff>
{
    public StaffFaker()
    {
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
            var number = f.UniqueIndex + 1;
            return $"SD{number:D4}";
        });
    }
}
