using Ardalis.GuardClauses;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.IdentityAggregate.Enums;
using ASM.Application.Domain.IdentityAggregate.Specifications;
using ASM.Application.Domain.Shared;
using ASM.Application.Features.Staffs.Get;
using Moq;

namespace ASM.UnitTests.UseCases.Staffs;

public sealed class GetStaffHandlerTests
{
    private readonly Mock<IReadRepository<Staff>> _repositoryMock;
    private readonly GetStaffHandler _handler;

    public GetStaffHandlerTests()
    {
        _repositoryMock = new();
        _handler = new(_repositoryMock.Object);
    }

    [Fact]
    public async Task GivenStaffId_ShouldReturnStaff_WhenStaffExists()
    {
        // Arrange
        var staffId = Guid.NewGuid();
        var staff = new Staff
        {
            Id = staffId,
            FirstName = "Nhan",
            LastName = "Nguyen Xuan",
            Dob = new(2000, 1, 1),
            JoinedDate = new(2021, 1, 1),
            Gender = Gender.Male,
            Location = Location.HoChiMinh,
            RoleType = RoleType.Admin,
            StaffCode = "SD2002"
        };
        var query = new GetStaffQuery(staffId);

        _repositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<StaffFilterSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(staff);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Value.Id.Should().Be(staffId);
        result.Value.FirstName.Should().Be("Nhan");
        result.Value.LastName.Should().Be("Nguyen Xuan");
        result.Value.Dob.Should().Be(new(2000, 1, 1));
        result.Value.JoinedDate.Should().Be(new(2021, 1, 1));
        _repositoryMock.Verify(r => r.FirstOrDefaultAsync(It.IsAny<StaffFilterSpec>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GivenStaffId_ShouldThrowNotFoundException_WhenStaffDoesNotExist()
    {
        // Arrange
        var staffId = Guid.NewGuid();
        var query = new GetStaffQuery(staffId);

        _repositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<StaffFilterSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Staff?)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        _repositoryMock.Verify(r => r.FirstOrDefaultAsync(It.IsAny<StaffFilterSpec>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
