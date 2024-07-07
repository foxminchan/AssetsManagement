import { buildQueryString } from "@libs/helpers/query.helper"
import HttpService from "@libs/services/http.service"

import { OrderParams } from "@/types/api"

import {
  Assignment,
  AssignmentFilter,
  CreateAssignmentRequest,
  ListAssignments,
  UpdateAssignmentRequest,
} from "./assignment.type"

class AssignmentService extends HttpService {
  constructor() {
    super()
  }

  listOwnAssignments(options?: Partial<OrderParams>): Promise<Assignment[]> {
    return this.get(`/assignments/own?${buildQueryString(options)}`)
  }

  listAssignments(
    options?: Partial<AssignmentFilter>
  ): Promise<ListAssignments> {
    return this.get(`/assignments?${buildQueryString(options)}`)
  }

  getAssignment(id: string): Promise<Assignment | null> {
    if (id) return this.get(`/assignments/${id}`)
    else return null as never
  }

  getOwnAssignment(id: string): Promise<Assignment> {
    return this.get(`/assignments/own/${id}`)
  }

  acceptAssignment(id: string): Promise<void> {
    return this.patch(`/assignments/${id}/accepted`, {})
  }

  requestForReturningAssignment(id: string): Promise<void> {
    return this.patch(`/assignments/${id}/request-for-returning`, {})
  }

  deleteAssignment(id: string): Promise<void> {
    return this.delete(`/assignments/${id}`)
  }

  createAssignment(data: CreateAssignmentRequest): Promise<string> {
    return this.post("/assignments", data)
  }

  updateAssignment(data: UpdateAssignmentRequest): Promise<void> {
    return this.put("/assignments", data)
  }
}

export default new AssignmentService()
