import { useMutation, useQueryClient } from "@tanstack/react-query"

import assignmentService from "./assignment.service"

export default function useDeleteAssignment() {
  const queryClient = useQueryClient()
  return useMutation<void, AppAxiosError, string>({
    mutationFn: (id: string) => {
      return assignmentService.deleteAssignment(id)
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["assignments"] })
      queryClient.invalidateQueries({ queryKey: ["own-assignments"] })
    },
  })
}
