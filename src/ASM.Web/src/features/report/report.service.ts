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

  exportAssetsByCategoryReport(
    options?: Partial<AssetsByCategoryRequest>
  ): Promise<Blob> {
    return this.get(
      `/reports/assets-by-category/export?${buildQueryString(options)}`,
      {
        responseType: "blob",
      }
    )
  }
}

export default new ReportService()
