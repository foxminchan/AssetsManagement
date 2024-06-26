import { buildQueryString } from "@libs/helpers/query.helper"
import HttpService from "@libs/services/http.service"

import {
  CreateUserRequest,
  ListUsers,
  UpdateUserRequest,
  User,
  UserFilter,
} from "./user.type"

class UserService extends HttpService {
  constructor() {
    super()
  }

  listUsers(options?: Partial<UserFilter>): Promise<ListUsers> {
    return this.get(`/users?${buildQueryString(options)}`)
  }

  getUser(id: string): Promise<User | null> {
    if (id) return this.get(`/users/${id}`)
    else return null as never
  }

  addUser(data: CreateUserRequest): Promise<string> {
    return this.post("/users", data)
  }

  deleteUser(id: string): Promise<void> {
    return this.delete(`/users/${id}`)
  }

  updateUser(data: UpdateUserRequest): Promise<void> {
    return this.put("/users", data)
  }
}

export default new UserService()
