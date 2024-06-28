import { useMutation } from "@tanstack/react-query"

import assignmentService from "./assignment.service"

export default function useDeleteAssignment() {
  return useMutation<void, AppAxiosError, string>({
    mutationFn: (id: string) => {
      return assignmentService.deleteAssignment(id)
    },
  })
}
