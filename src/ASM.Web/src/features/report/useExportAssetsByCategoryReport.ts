import { useQuery } from "@tanstack/react-query"

import reportService from "./report.service"
import { AssetsByCategoryRequest } from "./report.type"

export default function useExportAssetsByCategoryReport(
  options?: Partial<AssetsByCategoryRequest>
) {
  return useQuery({
    queryKey: ["export-assets-by-category", options],
    queryFn: () => reportService.exportAssetsByCategoryReport(options),
  })
}
