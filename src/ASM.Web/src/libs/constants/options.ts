import { AssetState } from "@features/assets/asset.type"
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

export const createAssetStateOptions: OptionItem<AssetState>[] = [
  {
    label: "Available",
    value: AssetState.Available,
  },
  {
    label: "Not available",
    value: AssetState.NotAvailable,
  },
]

export const updateAssetStateOptions: OptionItem<AssetState>[] = [
  {
    label: "Available",
    value: AssetState.Available,
  },
  {
    label: "Not available",
    value: AssetState.NotAvailable,
  },
  {
    label: "Waiting for recycling",
    value: AssetState.WaitingForRecycling,
  },
  {
    label: "Recycled",
    value: AssetState.Recycled,
  },
]
