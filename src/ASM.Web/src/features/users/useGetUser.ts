import { keepPreviousData, useQuery } from "@tanstack/react-query"

import userService from "./user.service"

export default function useGetUser(id: string) {
  return useQuery({
    queryKey: ["user", id],
    queryFn: () => userService.getUser(id),
    placeholderData: keepPreviousData,
    retry: 1,
  })
}
