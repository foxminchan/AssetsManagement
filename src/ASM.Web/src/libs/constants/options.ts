import { Gender, RoleType } from "@features/users/user.type"

import { OptionItem } from "@/types/data"

export const roleTypeOptions: OptionItem<RoleType>[] = [
  {
    label: "Admin",
    value: RoleType.Admin,
  },
  {
    label: "Staff",
    value: RoleType.Staff,
  },
]

export const genderOptions: OptionItem<Gender>[] = [
  {
    label: "Male",
    value: Gender.Male,
  },
  {
    label: "Female",
    value: Gender.Female,
  },
]
