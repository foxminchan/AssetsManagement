import { useQuery } from "@tanstack/react-query"

import assetService from "./asset.service"
import { AssetFilter } from "./asset.type"

export default function useListAssets(options?: Partial<AssetFilter>) {
  return useQuery({
    queryKey: ["assets", options],
    queryFn: () => assetService.listAssets(options),
  })
}
