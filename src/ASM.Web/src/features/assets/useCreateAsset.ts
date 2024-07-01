import { useMutation } from "@tanstack/react-query"

import assetService from "./asset.service"
import { CreateAssetRequest } from "./asset.type"

export default function useCreateAsset() {
  return useMutation<string, AppAxiosError, CreateAssetRequest>({
    mutationFn: (data: CreateAssetRequest) => assetService.createAsset(data),
  })
}
