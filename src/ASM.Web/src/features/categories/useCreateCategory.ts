import { useMutation } from "@tanstack/react-query"

import categoryService from "./category.service"
import { CreateCategoryRequest } from "./category.type"

export default function useCreateCategory() {
  return useMutation<string, AppAxiosError, CreateCategoryRequest>({
    mutationFn: (data: CreateCategoryRequest) =>
      categoryService.createCategory(data),
  })
}
