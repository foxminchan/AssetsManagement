import { buildQueryString } from "@libs/helpers/query.helper"
import HttpService from "@libs/services/http.service"

import { OrderParams } from "@/types/api"

class AssignmentService extends HttpService {
  constructor() {
    super()
  }

  getOwnAssignments(options?: Partial<OrderParams>) {
    return this.get(`/assignments/own?${buildQueryString(options)}`)
  }
}

export default new AssignmentService()
