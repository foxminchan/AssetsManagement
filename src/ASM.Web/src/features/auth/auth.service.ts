import HttpService from "@/services/http.service"

import { AuthUser, LoginRequest, LoginResponse } from "./auth.type"

class AuthService extends HttpService {
  constructor() {
    super()
  }

  login(data: LoginRequest): Promise<LoginResponse> {
    return this.post("/login", data)
  }

  getUser(): Promise<AuthUser> {
    return this.get("/me")
  }
}

export default new AuthService()
