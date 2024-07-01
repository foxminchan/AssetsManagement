import { useMutation, useQueryClient } from "@tanstack/react-query"

import userService from "./asset.service"
import { UpdateAssetRequest } from "./asset.type"

export default function useUpdateUser() {
  const queryClient = useQueryClient()

  return useMutation<void, AppAxiosError, UpdateAssetRequest>({
    mutationFn: (data: UpdateAssetRequest) => {
      return userService.updateAsset(data)
    },
    onSettled: async () =>
      queryClient.invalidateQueries({ queryKey: ["asset"] }),
  })
}
