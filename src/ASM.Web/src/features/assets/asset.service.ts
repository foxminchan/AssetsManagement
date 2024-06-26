import { buildQueryString } from "@libs/helpers/query.helper"
import HttpService from "@libs/services/http.service"

import { Asset, AssetFilter, ListAssets } from "./asset.type"

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

  deleteAsset(id: string): Promise<void> {
    return this.delete(`/assets/${id}`)
  }
}

export default new AssetService()
