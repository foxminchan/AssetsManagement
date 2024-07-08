import { useQuery } from "@tanstack/react-query"

import { ReturningRequestFilter } from "./returning-request.type"
import returningRequestService from "./returning-requests.service"

export default function useListReturningRequests(
  options?: Partial<ReturningRequestFilter>
) {
  return useQuery({
    queryKey: ["returning-requests", options],
    queryFn: () => returningRequestService.listReturningRequests(options),
  })
}
