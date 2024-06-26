import { useQuery } from "@tanstack/react-query"

import categoryService from "./categories.service"

export default function useListCategories() {
  return useQuery({
    queryKey: ["category"],
    queryFn: () => categoryService.listCategories(),
  })
}
