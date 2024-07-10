import { useMutation, useQueryClient } from "@tanstack/react-query"

import returningRequestsService from "./returning-requests.service"

export default function useCancelRequest() {
  const queryClient = useQueryClient()
  return useMutation<void, AppAxiosError, string>({
    mutationFn: (data: string) => {
      return returningRequestsService.cancelRequest(data)
    },
    onSettled: async () =>
      queryClient.invalidateQueries({
        queryKey: ["returning-requests", "own"],
      }),
  })
}
