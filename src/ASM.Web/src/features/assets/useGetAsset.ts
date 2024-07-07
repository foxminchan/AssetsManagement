import { keepPreviousData, useQuery } from "@tanstack/react-query"

import assetService from "./asset.service"

export default function useGetAsset(id: string) {
  return useQuery({
    queryKey: ["asset", id],
    queryFn: () => assetService.getAsset(id),
    placeholderData: keepPreviousData,
  })
}
