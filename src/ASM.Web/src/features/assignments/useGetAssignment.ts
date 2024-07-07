import { keepPreviousData, useQuery } from "@tanstack/react-query"

import assignmentService from "./assignment.service"

export default function useGetAssignment(id: string) {
  return useQuery({
    queryKey: ["assignment", id],
    queryFn: () => assignmentService.getAssignment(id),
    placeholderData: keepPreviousData,
  })
}
