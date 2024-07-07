import { FilterParams, PagedInfo } from "@/types/api"

// Response

export type Asset = {
  id: string
  assetCode: string
  name: string
  specification: string
  category: string
  categoryId: string
  state: AssetState
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
  featuredAssetId?: string
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

export type UpdateAssetRequest = CreateAssetRequest & {
  id: string
}

export type CreateAssetRequest = {
  name: string
  specification: string
  installDate: Date
  state: AssetState
  categoryId: string
}
// export type Asset = {
//   id: string
//   name: string
//   assetCode: string
//   specification: string
//   category: string
//   installDate: string
//   state: string
//   location: string
//   categoryId: string
// }

//export type AssetFilter = FilterParams
