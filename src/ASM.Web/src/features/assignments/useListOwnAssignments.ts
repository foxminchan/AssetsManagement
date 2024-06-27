import { useQuery } from "@tanstack/react-query"

import { OrderParams } from "@/types/api"

import assignmentService from "./assignment.service"

export default function useListOwnAssignments(options?: Partial<OrderParams>) {
  return useQuery({
    queryKey: ["own-assignments", options],
    queryFn: () => assignmentService.listOwnAssignments(options),
  })
}
