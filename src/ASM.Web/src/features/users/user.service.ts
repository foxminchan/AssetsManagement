import HttpService from "@/services/http.service"

import { UpdatePasswordRequest } from "./user.type"

class UserService extends HttpService {
  constructor() {
    super()
  }

  updatePassword(data: UpdatePasswordRequest): Promise<void> {
    return this.patch("/updatePassword", data)
  }
}

export default new UserService()
