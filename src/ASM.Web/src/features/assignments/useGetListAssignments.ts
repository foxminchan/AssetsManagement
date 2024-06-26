import { useQuery } from "@tanstack/react-query"

import { FilterParams } from "@/types/api"

import assignmentService from "./assignment.service"

export default function useGetListAssignments(options?: Partial<FilterParams>) {
  return useQuery({
    queryKey: ["assignments", options],
    queryFn: () => assignmentService.listAssignments(options),
  })
}
