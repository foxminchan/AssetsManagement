import { FilterParams, PagedInfo } from "@/types/api"
import { BaseEntity } from "@/types/data"

// Response

export type ReturningRequest = BaseEntity & {
  no: number
  assetCode: string
  assetName: string
  requestedBy: string
  assignedDate: Date
  acceptedBy: string
  returnedDate: Date
  state: ReturningRequestState
}

export type ListReturningRequests = {
  returningRequests: ReturningRequest[]
  pagedInfo: PagedInfo
}

// Requests

export type ReturningRequestFilter = FilterParams & {
  state?: ReturningRequestState
  returnedDate?: Date
}

// Additional types

export enum ReturningRequestState {
  WaitingForReturning = "WaitingForReturning",
  Completed = "Completed",
}
