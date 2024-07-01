import { useQuery } from "@tanstack/react-query"

import categoryService from "./category.service"

export default function useListCategories() {
  return useQuery({
    queryKey: ["categories"],
    queryFn: () => categoryService.listCategories(),
  })
}
