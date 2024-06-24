import { useMutation } from "@tanstack/react-query"

import userService from "./user.service"

export default function useDeleteUser() {
  return useMutation<void, AppAxiosError, string>({
    mutationFn: (data: string) => {
      return userService.deleteUser(data)
    },
  })
}
