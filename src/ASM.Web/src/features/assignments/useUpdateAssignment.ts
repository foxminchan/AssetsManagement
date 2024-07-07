import { useMutation, useQueryClient } from "@tanstack/react-query"

import assignmentService from "./assignment.service"
import { UpdateAssignmentRequest } from "./assignment.type"

export default function useUpdateAssignment() {
  const queryClient = useQueryClient()
  return useMutation<void, AppAxiosError, UpdateAssignmentRequest>({
    mutationFn: (data: UpdateAssignmentRequest) => {
      return assignmentService.updateAssignment(data)
    },
    onSuccess: async () => {
      queryClient.invalidateQueries({ queryKey: ["assignment"] })
      queryClient.invalidateQueries({ queryKey: ["assignments"] })
      queryClient.invalidateQueries({ queryKey: ["own-assignment"] })
      queryClient.invalidateQueries({ queryKey: ["own-assignments"] })
    },
  })
}
