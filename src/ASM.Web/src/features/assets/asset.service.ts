import { buildQueryString } from "@libs/helpers/query.helper"
import HttpService from "@libs/services/http.service"
import { format } from "date-fns"

import {
  Asset,
  AssetFilter,
  CreateAssetRequest,
  ListAssets,
  UpdateAssetRequest,
} from "./asset.type"

class AssetService extends HttpService {
  constructor() {
    super()
  }

  listAssets(options?: Partial<AssetFilter>): Promise<ListAssets> {
    return this.get(`/assets?${buildQueryString(options)}`)
  }

  getAsset(id: string): Promise<Asset | null> {
    if (id) return this.get(`/assets/${id}`)
    else return null as never
  }

  createAsset(data: CreateAssetRequest): Promise<string> {
    const formattedData = {
      ...data,
      installDate: format(data.installDate, "yyyy-MM-dd"),
    }
    return this.post("/assets", formattedData)
  }

  deleteAsset(id: string): Promise<void> {
    return this.delete(`/assets/${id}`)
  }

  updateAsset(data: UpdateAssetRequest): Promise<void> {
    const formattedData = {
      ...data,
      installDate: format(data.installDate, "yyyy-MM-dd"),
    }
    return this.put("/assets", formattedData)
  }
}

export default new AssetService()
