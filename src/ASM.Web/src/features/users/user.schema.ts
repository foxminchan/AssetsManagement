import { differenceInYears, format } from "date-fns"
import { z } from "zod"

import { Gender, RoleType } from "./user.type"

export const userSchema = z.object({
  lastName: z.string().min(1, "Last name is required"),
  firstName: z.string().min(1, "First name is required"),
  dob: z.coerce
    .date()
    .refine((data) => differenceInYears(new Date(), data) >= 18, {
      message: "User is under 18. Please select a different date",
    }),
  joinedDate: z.coerce
    .date()
    .optional()
    .refine((data) => {
      const dayOfWeek = data && format(data, "EEEE")
      return dayOfWeek != "Sunday" && dayOfWeek != "Saturday"
    }, "Joined date is Saturday or Sunday. Please select a different date"),
  gender: z.nativeEnum(Gender),
  roleType: z.nativeEnum(RoleType),
})
