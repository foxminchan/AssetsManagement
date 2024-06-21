import { useMutation } from "@tanstack/react-query"

import userService from "./user.service"
import { UpdatePasswordRequest } from "./user.type"

export default function useUpdatePassword() {
  return useMutation<void, AppAxiosError, UpdatePasswordRequest>({
    mutationFn: (data: UpdatePasswordRequest) => {
      return userService.updatePassword(data)
    },
  })
}
