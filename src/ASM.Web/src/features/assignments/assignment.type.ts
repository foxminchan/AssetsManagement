import { BaseEntity } from "@/types/data"

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

export enum State {
  WaitingForAcceptance = "WaitingForAcceptance",
  Accepted = "Accepted",
  IsRequested = "IsRequested",
}
