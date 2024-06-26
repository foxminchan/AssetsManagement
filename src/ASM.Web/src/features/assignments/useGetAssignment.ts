import { keepPreviousData, useQuery } from "@tanstack/react-query"

import assignmentService from "./assignment.service"

export default function useGetAssignment(id: string) {
  return useQuery({
    queryKey: ["assignment", id],
    queryFn: () => assignmentService.getUser(id),
    placeholderData: keepPreviousData,
  })
}
