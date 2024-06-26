using ASM.Application.Domain.AssetAggregate;
using ASM.Application.Domain.AssetAggregate.Enums;
using ASM.Application.Domain.Shared;

namespace ASM.UnitTests.Builder;

public static class ListAssetsBuilder
{
    private static List<Asset> _staffs = [];

    public static List<Asset> WithDefaultValues()
    {
        _staffs =
        [
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Practical Soft Car",
                AssetCode = "BS000001",
                Specification = "The Apollotech B340 is an affordable wireless mouse with reliable connectivity, 12 months battery life and modern design",
                InstallDate = new(2000, 1, 1),
                State = State.Available,
                Location = Location.HoChiMinh,
                CategoryId = Guid.NewGuid(),
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Unbranded Fresh Tuna",
                AssetCode = "LT000017",
                Specification = "The automobile layout consists of a front-engine design, with transaxle-type transmissions mounted at the rear of the engine and four wheel drive",
                InstallDate = new(2000, 1, 1),
                State = State.Assigned,
                Location = Location.HoChiMinh,
                CategoryId = Guid.NewGuid(),
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Practical Soft Car",
                AssetCode = "BS000001",
                Specification = "The Apollotech B340 is an affordable wireless mouse with reliable connectivity, 12 months battery life and modern design",
                InstallDate = new(2000, 1, 1),
                State = State.Available,
                Location = Location.HoChiMinh,
                CategoryId = Guid.NewGuid(),
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Small Rubber Chair",
                AssetCode = "MC000003",
                Specification = "The Apollotech B340 is an affordable wireless mouse with reliable connectivity, 12 months battery life and modern design",
                InstallDate = new(2000, 1, 1),
                State = State.Recycled,
                Location = Location.HoChiMinh,
                CategoryId = Guid.NewGuid(),
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Practical Concrete Bacon",
                AssetCode = "MC000008",
                Specification = "The Nagasaki Lander is the trademarked name of several series of Nagasaki sport bikes, that started with the 1984 ABC800J",
                InstallDate = new(2000, 1, 1),
                State = State.WaitingForRecycling,
                Location = Location.HoChiMinh,
                CategoryId = Guid.NewGuid(),
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Practical Soft Car",
                AssetCode = "BS000001",
                Specification = "The Apollotech B340 is an affordable wireless mouse with reliable connectivity, 12 months battery life and modern design",
                InstallDate = new(2000, 1, 1),
                State = State.Available,
                Location = Location.HoChiMinh,
                CategoryId = Guid.NewGuid(),
            },
        ];
        return _staffs;
    }
}
