import { User } from "@features/users/user.type"

export const fieldOrder: (keyof User)[] = [
  "staffCode",
  "fullName",
  "userName",
  "dob",
  "gender",
  "joinedDate",
  "roleType",
  "location",
]

export const fieldLabels: Record<keyof User, string> = {
  id: "ID",
  staffCode: "Staff Code",
  fullName: "Full Name",
  userName: "Username",
  dob: "Date of Birth",
  gender: "Gender",
  joinedDate: "Joined Date",
  roleType: "Role Type",
  location: "Location",
  firstName: "",
  lastName: "",
}
