import { useQuery } from "@tanstack/react-query"

import assignmentService from "./assignment.service"
import { AssignmentFilter } from "./assignment.type"

export default function useListAssignments(
  options?: Partial<AssignmentFilter>
) {
  return useQuery({
    queryKey: ["assignments", options],
    queryFn: () => assignmentService.listAssignments(options),
  })
}
