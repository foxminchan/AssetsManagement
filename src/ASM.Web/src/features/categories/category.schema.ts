import { z } from "zod"

export const categorySchema = z.object({
  name: z.string(),
  prefix: z.string(),
})
