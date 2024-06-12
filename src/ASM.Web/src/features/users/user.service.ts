import { buildQueryString } from "@libs/helpers/query.helper"
import HttpService from "@libs/services/http.service"

import { CreateUserRequest, ListUsers, User, UserFilter } from "./user.type"

class UserService extends HttpService {
  constructor() {
    super()
  }

  listUsers(options?: Partial<UserFilter>): Promise<ListUsers> {
    return this.get(`/users?${buildQueryString(options)}`)
  }

  getUser(id: string): Promise<User> {
    return this.get(`/users/${id}`)
  }

  addUser(data: CreateUserRequest): Promise<string> {
    return this.post("/users", data)
  }
}

export default new UserService()
