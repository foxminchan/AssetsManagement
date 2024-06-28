import { useMutation } from "@tanstack/react-query"

import assignmentService from "./assignment.service"

export default function useAcceptAssignment() {
  return useMutation<void, AppAxiosError, string>({
    mutationFn: (id: string) => {
      return assignmentService.acceptAssignment(id)
    },
  })
}
