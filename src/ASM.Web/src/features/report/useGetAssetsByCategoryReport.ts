import { useQuery, UseQueryResult } from "@tanstack/react-query"

import reportService from "./report.service"
import { AssetsByCategory, AssetsByCategoryRequest } from "./report.type"

export default function useGetAssetsByCategoryReport(
  options?: Partial<AssetsByCategoryRequest>
): UseQueryResult<AssetsByCategory[]> {
  return useQuery({
    queryKey: ["assets-by-category", options],
    queryFn: () => reportService.getAssetsByCategoryReport(options),
  })
}
