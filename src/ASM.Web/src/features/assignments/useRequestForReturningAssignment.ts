import { useMutation, useQueryClient } from "@tanstack/react-query"

import assignmentService from "./assignment.service"

export default function useRequestForReturningAssignment() {
  const queryClient = useQueryClient()
  return useMutation<void, AppAxiosError, string>({
    mutationFn: (id: string) => {
      return assignmentService.requestForReturningAssignment(id)
    },
    onSuccess: async () => {
      queryClient.invalidateQueries({ queryKey: ["assignment"] })
      queryClient.invalidateQueries({ queryKey: ["assignments"] })
      queryClient.invalidateQueries({ queryKey: ["own-assignment"] })
      queryClient.invalidateQueries({ queryKey: ["own-assignments"] })
      queryClient.invalidateQueries({ queryKey: ["returning-requests"] })
    },
  })
}
