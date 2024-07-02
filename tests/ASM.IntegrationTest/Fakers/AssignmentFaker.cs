using ASM.Application.Domain.AssignmentAggregate;
using ASM.Application.Domain.AssignmentAggregate.Enums;
using Bogus;

namespace ASM.IntegrationTest.Fakers;

public sealed class AssignmentFaker : Faker<Assignment>
{
    public AssignmentFaker()
    {
        RuleFor(x => x.Id, f => f.Random.Guid());
        RuleFor(x => x.State, f => f.PickRandom<State>());
        RuleFor(x => x.AssignedDate, f => f.Date.PastDateOnly());
        RuleFor(x => x.Note, f => f.Lorem.Sentence());
    }
}
