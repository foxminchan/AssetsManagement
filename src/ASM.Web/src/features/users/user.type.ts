import { FilterParams, PagedInfo } from "@/types/api"

// Response

export type User = {
  id: string
  firstName: string
  lastName: string
  fullName: string
  staffCode: string
  userName: string
  joinedDate: Date
  dob: Date
  roleType: RoleType
  gender: Gender
  location: Location
}

export type ListUsers = {
  users: User[]
  pagedInfo: PagedInfo
}

// Requests

export type UserFilter = FilterParams & {
  roleType?: RoleType
}
export type UpdateUserRequest = CreateUserRequest & {
  id: string
}

export type CreateUserRequest = {
  firstName: string
  lastName: string
  joinedDate: string
  dob: string
  roleType: RoleType
  gender: Gender
}

// Additional types

export enum RoleType {
  Admin = "Admin",
  Staff = "Staff",
}

export enum Gender {
  Male = "Male",
  Female = "Female",
}

export enum Location {
  HoChiMinh = "HoChiMinh",
  DaNang = "DaNang",
  Hanoi = "Hanoi",
}
