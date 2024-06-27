import { keepPreviousData, useQuery } from "@tanstack/react-query"

import assignmentService from "./assignment.service"

export default function useGetOwnAssignment(id: string) {
  return useQuery({
    queryKey: ["own-assignment", id],
    queryFn: () => assignmentService.getOwnAssignment(id),
    placeholderData: keepPreviousData,
  })
}
