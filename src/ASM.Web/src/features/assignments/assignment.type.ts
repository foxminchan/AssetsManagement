export type Assignment = {
  No: number
  Id: string
  AssetCode: string
  AssetName: string
  Specification: string
  Category: string
  AssignedTo: string
  AssignedBy: string
  AssignedDate: Date
  State: State
  Note: string
}

export enum State {
  WaitingForAcceptance = "WaitingForAcceptance",
  Accepted = "Accepted",
  IsRequested = "IsRequested",
}
