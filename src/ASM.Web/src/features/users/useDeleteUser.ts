import { useMutation, useQueryClient } from "@tanstack/react-query"

import userService from "./user.service"

export default function useDeleteUser() {
  const queryClient = useQueryClient()
  return useMutation<void, AppAxiosError, string>({
    mutationFn: (data: string) => {
      return userService.deleteUser(data)
    },
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ["users"] }),
  })
}
