import { useMutation } from "@tanstack/react-query"

import assignmentService from "./assignment.service"
import { UpdateAssignmentRequest } from "./assignment.type"

export default function useUpdateAssignment() {
  return useMutation<void, AppAxiosError, UpdateAssignmentRequest>({
    mutationFn: (data: UpdateAssignmentRequest) => {
      return assignmentService.updateAssignment(data)
    },
  })
}
