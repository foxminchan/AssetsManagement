import { useMutation } from "@tanstack/react-query"

import assetService from "./asset.service"

export default function useDeleteAsset() {
  return useMutation<void, AppAxiosError, string>({
    mutationFn: (data: string) => {
      return assetService.deleteAsset(data)
    },
  })
}
