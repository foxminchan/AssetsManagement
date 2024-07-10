import { useMutation, useQueryClient } from "@tanstack/react-query"

import categoryService from "./category.service"
import { CreateCategoryRequest } from "./category.type"

export default function useCreateCategory() {
  const queryClient = useQueryClient()
  return useMutation<string, AppAxiosError, CreateCategoryRequest>({
    mutationFn: (data: CreateCategoryRequest) =>
      categoryService.createCategory(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["export-assets-by-category"] })
    },
  })
}
