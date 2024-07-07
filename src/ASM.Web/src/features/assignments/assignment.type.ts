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
  userId: string
  assetId: string
}

export type ListAssignments = {
  assignments: Assignment[]
  pagedInfo: PagedInfo
}

// Requests

export type AssignmentFilter = FilterParams & {
  state?: State
  featuredAssignmentId?: string
}

// Additional types

export enum State {
  WaitingForAcceptance = "WaitingForAcceptance",
  Accepted = "Accepted",
  RequestForReturning = "RequestForReturning",
}

export type UpdateAssignmentRequest = CreateAssignmentRequest & {
  id: string
}

export type ViewUpdateAssignmentRequest = {
  assetName: string
  userName: string
  userId: string
  assetId: string
  note: string
  assignedDate: Date
  id: string
}

export type CreateAssignmentRequest = {
  userId: string
  assetId: string
  assignedDate: string
  note?: string
}
