import { useMutation, useQueryClient } from "@tanstack/react-query"

import returningRequestsService from "./returning-requests.service"

export default function useCompleteRequest() {
  const queryClient = useQueryClient()
  return useMutation<void, AppAxiosError, string>({
    mutationFn: (data: string) => {
      return returningRequestsService.completeRequest(data)
    },
    onSettled: async () =>
      queryClient.invalidateQueries({ queryKey: ["returning-requests"] }),
  })
}
