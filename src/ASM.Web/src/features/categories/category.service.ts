import HttpService from "@libs/services/http.service"

import { CreateCategoryRequest, ListCategories } from "./category.type"

class CategoryService extends HttpService {
  constructor() {
    super()
  }

  listCategories(): Promise<ListCategories> {
    return this.get(`/categories`)
  }

  createCategory(data: CreateCategoryRequest): Promise<string> {
    return this.post("/categories", data)
  }
}

export default new CategoryService()
