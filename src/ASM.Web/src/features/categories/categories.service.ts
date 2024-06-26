import HttpService from "@libs/services/http.service"

import { ListCategories } from "./categories.type"

class CategoryService extends HttpService {
  constructor() {
    super()
  }

  listCategories(): Promise<ListCategories> {
    return this.get(`/categories`)
  }
}

export default new CategoryService()
