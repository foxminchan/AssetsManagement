import { useMutation, useQueryClient } from "@tanstack/react-query"

import assetService from "./asset.service"

export default function useDeleteAsset() {
  const queryClient = useQueryClient()
  return useMutation<void, AppAxiosError, string>({
    mutationFn: (data: string) => {
      return assetService.deleteAsset(data)
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["assets"] })
      queryClient.invalidateQueries({ queryKey: ["categories"] })
      queryClient.invalidateQueries({ queryKey: ["assets-by-category"] })
      queryClient.invalidateQueries({ queryKey: ["export-assets-by-category"] })
    },
  })
}
