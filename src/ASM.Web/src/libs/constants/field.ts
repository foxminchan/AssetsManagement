import { Assignment } from "@features/assignments/assignment.type"
import { User } from "@features/users/user.type"

// User

export const userFieldOrder: (keyof User)[] = [
  "staffCode",
  "fullName",
  "userName",
  "dob",
  "gender",
  "joinedDate",
  "roleType",
  "location",
]

export const userfieldLabels: Record<keyof User, string> = {
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

// Assignment

export const assignmentFieldOrder: (keyof Assignment)[] = [
  "assetCode",
  "assetName",
  "specification",
  "assignedTo",
  "assignedBy",
  "assignedDate",
  "state",
  "note",
]

export const assignmentFieldLabels: Record<keyof Assignment, string> = {
  id: "ID",
  no: "No",
  assetCode: "Asset Code",
  assetName: "Asset Name",
  specification: "Specification",
  category: "Category",
  assignedTo: "Assigned To",
  assignedBy: "Assigned By",
  assignedDate: "Assigned Date",
  state: "State",
  note: "Note",
}
