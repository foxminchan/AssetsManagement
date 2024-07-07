import { useMutation, useQueryClient } from "@tanstack/react-query"

import userService from "./user.service"
import { CreateUserRequest } from "./user.type"

export default function useAddUser() {
  const queryClient = useQueryClient()
  return useMutation<string, AppAxiosError, CreateUserRequest>({
    mutationFn: (data: CreateUserRequest) => userService.addUser(data),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ["users"] }),
  })
}
