import { useMutation, useQueryClient } from "@tanstack/react-query"

import assignmentService from "./assignment.service"
import { CreateAssignmentRequest } from "./assignment.type"

export default function useCreateAssignment() {
  const queryClient = useQueryClient()
  return useMutation<string, AppAxiosError, CreateAssignmentRequest>({
    mutationFn: (data: CreateAssignmentRequest) =>
      assignmentService.createAssignment(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["assignment"] })
      queryClient.invalidateQueries({ queryKey: ["assignments"] })
      queryClient.invalidateQueries({ queryKey: ["own-assignment"] })
      queryClient.invalidateQueries({ queryKey: ["own-assignments"] })
    },
  })
}
