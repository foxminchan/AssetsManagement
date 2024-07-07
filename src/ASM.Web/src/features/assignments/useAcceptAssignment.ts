import { useMutation, useQueryClient } from "@tanstack/react-query"

import assignmentService from "./assignment.service"

export default function useAcceptAssignment() {
  const queryClient = useQueryClient()
  return useMutation<void, AppAxiosError, string>({
    mutationFn: (id: string) => {
      return assignmentService.acceptAssignment(id)
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["assignments"] })
      queryClient.invalidateQueries({ queryKey: ["own-assignments"] })
    },
  })
}
