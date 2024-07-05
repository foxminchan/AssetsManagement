import { BaseEntity } from "@/types/data"

export type AssetsByCategory = BaseEntity & {
  category: string
  total: number
  assigned: number
  available: number
  notAvailable: number
  waitingForRecycling: number
  recycled: number
}

export type AssetsByCategoryRequest = {
  orderBy: string
  isDescending: boolean
}
