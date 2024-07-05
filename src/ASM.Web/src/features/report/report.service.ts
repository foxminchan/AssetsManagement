import { buildQueryString } from "@libs/helpers/query.helper"
import HttpService from "@libs/services/http.service"

import { AssetsByCategory, AssetsByCategoryRequest } from "./report.type"

class ReportService extends HttpService {
  constructor() {
    super()
  }

  getAssetsByCategoryReport(
    options?: Partial<AssetsByCategoryRequest>
  ): Promise<AssetsByCategory[]> {
    return this.get(`/reports/assets-by-category?${buildQueryString(options)}`)
  }
}

export default new ReportService()
