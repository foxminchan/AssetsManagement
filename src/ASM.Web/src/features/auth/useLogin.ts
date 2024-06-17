import { useMutation } from "@tanstack/react-query"

import authService from "./auth.service"
import { LoginRequest, LoginResponse } from "./auth.type"

export default function useLogin() {
  return useMutation<LoginResponse, AppAxiosError, LoginRequest>({
    mutationFn: (data: LoginRequest) => {
      return authService.login(data)
    },
  })
}
