import { FilterParams, PagedInfo } from "@/types/api"

export type ListAssets = {
  assets: Asset[]
  pagedInfo: PagedInfo
}

export type Asset = {
  id: string
  name: string
  assetCode: string
  specification: string
  category: string
  installDate: string
  state: string
  location: string
  categoryId: string
}

export type AssetFilter = FilterParams
