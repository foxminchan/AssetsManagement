import { useMutation, useQueryClient } from "@tanstack/react-query"

import assetService from "./asset.service"
import { CreateAssetRequest } from "./asset.type"

export default function useCreateAsset() {
  const queryClient = useQueryClient()
  return useMutation<string, AppAxiosError, CreateAssetRequest>({
    mutationFn: (data: CreateAssetRequest) => assetService.createAsset(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["assets"] })
      queryClient.invalidateQueries({ queryKey: ["categories"] })
      queryClient.invalidateQueries({ queryKey: ["assets-by-category"] })
      queryClient.invalidateQueries({ queryKey: ["export-assets-by-category"] })
    },
  })
}
