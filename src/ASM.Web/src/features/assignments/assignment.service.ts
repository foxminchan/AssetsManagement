import { buildQueryString } from "@libs/helpers/query.helper"
import HttpService from "@libs/services/http.service"

import { OrderParams } from "@/types/api"

import { Assignment } from "./assignment.type"

class AssignmentService extends HttpService {
  constructor() {
    super()
  }

  listOwnAssignments(options?: Partial<OrderParams>): Promise<Assignment[]> {
    return this.get(`/assignments/own?${buildQueryString(options)}`)
  }

  getOwnAssignment(id: string): Promise<Assignment> {
    return this.get(`/assignments/own/${id}`)
  }
}

export default new AssignmentService()
