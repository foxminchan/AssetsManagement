import { z } from "zod"

export const createAssignmentSchema = z.object({
  userId: z.string().min(1),
  assetId: z.string().min(1),
  assignedDate: z.coerce.date(),
  note: z.string().optional(),
})
