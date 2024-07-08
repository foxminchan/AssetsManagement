﻿using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.IdentityAggregate.Enums;
using ASM.Application.Domain.Shared;

namespace ASM.UnitTests.Builder;

public static class ListStaffsBuilder
{
    private static List<Staff> _staffs = [];

    public static List<Staff> WithDefaultValues()
    {
        _staffs =
        [
            new()
            {
                Id = Guid.NewGuid(),
                FirstName = "Nhan",
                LastName = "Nguyen Xuan",
                Dob = new(2000, 1, 1),
                JoinedDate = new(2021, 1, 1),
                Gender = Gender.Male,
                Location = Location.HoChiMinh,
                RoleType = RoleType.Admin,
                StaffCode = "SD2002",
                Users = [new() { UserName = "nhannx" }],
            },
            new()
            {
                Id = Guid.NewGuid(),
                FirstName = "Dien",
                LastName = "Truong Kim",
                Dob = new(2000, 1, 1),
                JoinedDate = new(2021, 1, 1),
                Gender = Gender.Male,
                Location = Location.DaNang,
                RoleType = RoleType.Admin,
                StaffCode = "SD2001",
                Users = [new() { UserName = "dientk" }]
            },
            new()
            {
                Id = Guid.NewGuid(),
                FirstName = "Man",
                LastName = "Vo Minh",
                Dob = new(2002, 8, 1),
                JoinedDate = new(2021, 1, 1),
                Gender = Gender.Male,
                Location = Location.HoChiMinh,
                RoleType = RoleType.Admin,
                StaffCode = "SD2000",
                Users = [new() { UserName = "manvm" }]
            },
            new()
            {
                Id = Guid.NewGuid(),
                FirstName = "Khoi",
                LastName = "Tran Minh",
                Dob = new(2003, 8, 1),
                JoinedDate = new(2024, 1, 1),
                Gender = Gender.Male,
                Location = Location.HaNoi,
                RoleType = RoleType.Admin,
                StaffCode = "SD1890",
                Users = [new() { UserName = "khoitm" }]
            },
            new()
            {
                Id = Guid.NewGuid(),
                FirstName = "Minh",
                LastName = "Pham Le",
                Dob = new(2002, 8, 1),
                JoinedDate = new(2024, 9, 1),
                Gender = Gender.Male,
                Location = Location.HoChiMinh,
                RoleType = RoleType.Admin,
                StaffCode = "SD1800",
                Users = [new() { UserName = "minhpl" }]
            }
        ];

        return _staffs;
    }

}
