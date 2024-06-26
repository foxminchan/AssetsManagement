import { FilterParams, PagedInfo } from "@/types/api"
import { BaseEntity } from "@/types/data"

// Response

export type Assignment = BaseEntity & {
  no: number
  assetCode: string
  assetName: string
  specification: string
  category: string
  assignedTo: string
  assignedBy: string
  assignedDate: Date
  state: State
  note: string
}

export type ListAssignments = {
  assignments: Assignment[]
  pagedInfo: PagedInfo
}

// Requests

export type AssignmentFilter = FilterParams & {
  state?: State
}

// Additional types

export enum State {
  WaitingForAcceptance = "WaitingForAcceptance",
  Accepted = "Accepted",
  IsRequested = "IsRequested",
}
