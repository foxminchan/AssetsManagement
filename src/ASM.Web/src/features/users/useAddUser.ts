import { useMutation } from "@tanstack/react-query"

import userService from "./user.service"
import { CreateUserRequest } from "./user.type"

export default function useAddUser() {
  return useMutation<string, AppAxiosError, CreateUserRequest>({
    mutationFn: (data: CreateUserRequest) => userService.addUser(data),
  })
}
