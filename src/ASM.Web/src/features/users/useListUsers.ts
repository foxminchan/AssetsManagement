import { useQuery } from "@tanstack/react-query"

import userService from "./user.service"
import { UserFilter } from "./user.type"

export default function useListUsers(options?: Partial<UserFilter>) {
  return useQuery({
    queryKey: ["users", options],
    queryFn: () => userService.listUsers(options),
  })
}
