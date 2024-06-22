import { keepPreviousData, useQuery } from "@tanstack/react-query"

import authService from "./auth.service"

export default function useGetMe() {
  return useQuery({
    queryKey: ["me"],
    queryFn: () => authService.getMe(),
    placeholderData: keepPreviousData,
  })
}
