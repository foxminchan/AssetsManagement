import { buildQueryString } from "@libs/helpers/query.helper"
import HttpService from "@libs/services/http.service"

import { ListUsers, User, UserFilter } from "./user.type"

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
}

export default new UserService()
