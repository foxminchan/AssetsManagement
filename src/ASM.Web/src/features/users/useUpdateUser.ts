import { useMutation, useQueryClient } from "@tanstack/react-query"

import userService from "./user.service"
import { UpdateUserRequest } from "./user.type"

export default function useUpdateUser() {
  const queryClient = useQueryClient()

  return useMutation<void, AppAxiosError, UpdateUserRequest>({
    mutationFn: (data: UpdateUserRequest) => {
      return userService.updateUser(data)
    },
    onSuccess: async () => {
      queryClient.invalidateQueries({ queryKey: ["user"] })
      queryClient.invalidateQueries({ queryKey: ["users"] })
    },
  })
}
