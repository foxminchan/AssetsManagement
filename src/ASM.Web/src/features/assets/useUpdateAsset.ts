import { useMutation, useQueryClient } from "@tanstack/react-query"

import userService from "./asset.service"
import { UpdateAssetRequest } from "./asset.type"

export default function useUpdateUser() {
  const queryClient = useQueryClient()

  return useMutation<void, AppAxiosError, UpdateAssetRequest>({
    mutationFn: (data: UpdateAssetRequest) => {
      return userService.updateAsset(data)
    },
    onSuccess: async () => {
      queryClient.invalidateQueries({ queryKey: ["assets"] })
      queryClient.invalidateQueries({ queryKey: ["categories"] })
      queryClient.invalidateQueries({ queryKey: ["assets-by-category"] })
      queryClient.invalidateQueries({ queryKey: ["export-assets-by-category"] })
    },
  })
}
