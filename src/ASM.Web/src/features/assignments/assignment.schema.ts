import { isAfter, startOfDay } from "date-fns"
import { z } from "zod"

export const createAssignmentSchema = z.object({
  userId: z.string().min(1),
  assetId: z.string().min(1),
  assignedDate: z.coerce
    .date()
    .refine((data) => !isAfter(startOfDay(new Date()), data), {
      message: "Can not assign date in the part",
    }),
  note: z.string().optional(),
})
