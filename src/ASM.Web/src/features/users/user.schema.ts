import { ValidationError } from "@tanstack/react-form"
import { SafeParseError, z, ZodIssue, ZodType, ZodTypeAny } from "zod"

const passwordRegex =
  /^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/

export const passwordSchema = z.object({
  id: z.string(),
  oldPassword: z.string(),
  newPassword: z
    .string()
    .min(8, {
      message: "New password must contain at least 8 characters.",
    })
    .refine((data) => passwordRegex.test(data), {
      message:
        "New password must contain at least one letter, one number, and one special character (@$!%*?&).",
    }),
})

// Customization of zodValidator to take only the first error message
type Params = {
  transformErrors?: (errors: ZodIssue[]) => ValidationError
}

export const passwordValidator =
  (params: Params = {}) =>
  () => {
    return {
      validate({ value }: { value: unknown }, fn: ZodType): ValidationError {
        // Call Zod on the value here and return the error message
        const result = (fn as ZodTypeAny).safeParse(value)
        if (!result.success) {
          if (params.transformErrors) {
            return params.transformErrors(
              (result as SafeParseError<any>).error.issues
            )
          }
          return (result as SafeParseError<any>).error.issues.map(
            (issue) => issue.message
          )[0]
        }
        return
      },
      async validateAsync(
        { value }: { value: unknown },
        fn: ZodType
      ): Promise<ValidationError> {
        // Call Zod on the value here and return the error message
        const result = await (fn as ZodTypeAny).safeParseAsync(value)
        if (!result.success) {
          if (params.transformErrors) {
            return params.transformErrors(
              (result as SafeParseError<any>).error.issues
            )
          }
          return (result as SafeParseError<any>).error.issues.map(
            (issue) => issue.message
          )[0]
        }
        return
      },
    }
  }
