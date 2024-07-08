import { buildQueryString } from "@libs/helpers/query.helper"
import HttpService from "@libs/services/http.service"

import {
  ListReturningRequests,
  ReturningRequestFilter,
} from "./returning-request.type"

class ReturningRequestService extends HttpService {
  constructor() {
    super()
  }

  listReturningRequests(
    options?: Partial<ReturningRequestFilter>
  ): Promise<ListReturningRequests> {
    return this.get(`/returning-requests?${buildQueryString(options)}`)
  }
}

export default new ReturningRequestService()
