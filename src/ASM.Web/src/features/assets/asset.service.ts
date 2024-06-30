import { buildQueryString } from "@libs/helpers/query.helper"
import HttpService from "@libs/services/http.service"

import { AssetFilter, ListAssets } from "./asset.type"

class AssetService extends HttpService {
  constructor() {
    super()
  }

  deleteAsset(id: string): Promise<void> {
    return this.delete(`/assets/${id}`)
  }
  
  listAssets(options?: Partial<AssetFilter>): Promise<ListAssets> {
    return this.get(`/assets?${buildQueryString(options)}`)
  }
}

export default new AssetService()
