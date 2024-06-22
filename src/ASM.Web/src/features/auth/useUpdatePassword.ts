import { useMutation } from "@tanstack/react-query"

import authService from "./auth.service"
import { UpdatePasswordRequest } from "./auth.type"

export default function useUpdatePassword() {
  return useMutation<void, AppAxiosError, UpdatePasswordRequest>({
    mutationFn: (data: UpdatePasswordRequest) => {
      return authService.updatePassword(data)
    },
  })
}
