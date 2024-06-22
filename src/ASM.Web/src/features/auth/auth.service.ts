import HttpService from "@libs/services/http.service"

import {
  AuthUser,
  LoginRequest,
  LoginResponse,
  UpdatePasswordRequest,
} from "./auth.type"

class AuthService extends HttpService {
  constructor() {
    super()
  }

  login(data: LoginRequest): Promise<LoginResponse> {
    return this.post("/login", data)
  }

  getMe(): Promise<AuthUser> {
    return this.get("/me")
  }

  updatePassword(data: UpdatePasswordRequest): Promise<void> {
    return this.patch("/updatePassword", data)
  }
}

export default new AuthService()
