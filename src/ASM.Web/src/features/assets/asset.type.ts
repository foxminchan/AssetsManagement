import { FilterParams, PagedInfo } from "@/types/api"

// Response

export type Asset = {
  id: string
  assetCode: string
  name: string
  specification: string
  category: string
  categoryId: string
  state: string
  installDate: Date
  location: Location
}

export type ListAssets = {
  assets: Asset[]
  pagedInfo: PagedInfo
}

// Requests

export type AssetFilter = FilterParams & {
  state?: AssetState[]
  categories?: string[]
}

// Additional types

export enum Location {
  HoChiMinh = "HoChiMinh",
  DaNang = "DaNang",
  Hanoi = "Hanoi",
}

export enum AssetState {
  Available = "Available",
  Assigned = "Assigned",
  NotAvailable = "NotAvailable",
  WaitingForRecycling = "WaitingForRecycling",
  Recycled = "Recycled",
}
