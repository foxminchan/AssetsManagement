import { keepPreviousData, useQuery } from "@tanstack/react-query"

import authService from "./auth.service"

export default function useGetUser() {
  return useQuery({
    queryKey: ["user"],
    queryFn: () => authService.getUser(),
    placeholderData: keepPreviousData,
  })
}
