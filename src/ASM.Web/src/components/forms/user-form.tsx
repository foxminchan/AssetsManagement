import { useEffect } from "react"
import useAddUser from "@features/users/useAddUser"
import { userSchema } from "@features/users/user.schema"
import {
  CreateUserRequest,
  Gender,
  RoleType,
  UpdateUserRequest,
} from "@features/users/user.type"
import useUpdateUser from "@features/users/useUpdateUser"
import { genderOptions, roleTypeOptions } from "@libs/constants/options"
import { featuredUserAtom } from "@libs/jotai/userAtom"
import {
  Button,
  CircularProgress,
  Container,
  FormControl,
  FormControlLabel,
  FormHelperText,
  FormLabel,
  MenuItem,
  Radio,
  RadioGroup,
  Select,
  TextField,
} from "@mui/material"
import { DatePicker } from "@mui/x-date-pickers/DatePicker"
import { useForm } from "@tanstack/react-form"
import { Link, useNavigate, useParams } from "@tanstack/react-router"
import { zodValidator } from "@tanstack/zod-form-adapter"
import { differenceInYears, format } from "date-fns"
import { useSetAtom } from "jotai"
import { z } from "zod"

import { OptionItem } from "@/types/data"

type UserFormProps = {
  initialData?: CreateUserRequest | UpdateUserRequest | null
  isEditing?: boolean
}

export default function UserForm({ initialData }: Readonly<UserFormProps>) {
  const navigate = useNavigate()
  const today = new Date(Date.now())
  const isEditing = !!initialData
  const setFeaturedUserId = useSetAtom(featuredUserAtom)

  const params = isEditing
    ? useParams({ from: "/_authenticated/user/$id" })
    : null

  const defaultValues =
    initialData ??
    ({
      lastName: "",
      firstName: "",
    } as z.infer<typeof userSchema>)

  const {
    data: createdUserId,
    mutate: addUser,
    isSuccess: addUserSuccess,
    isPending: addUserPending,
  } = useAddUser()

  const {
    mutate: updateUser,
    isSuccess: updateUserSuccess,
    isPending: updateUserPending,
  } = useUpdateUser()

  useEffect(() => {
    if (addUserSuccess) {
      navigate({ from: "/user/new", to: "/user" })
      setFeaturedUserId(createdUserId)
    }
  }, [addUserSuccess])

  useEffect(() => {
    if (updateUserSuccess) {
      navigate({ from: "/user/$id", to: "/user" })
      params && setFeaturedUserId(params.id)
    }
  }, [updateUserSuccess])

  const { Field, Subscribe, handleSubmit, getFieldValue } = useForm({
    defaultValues,
    validatorAdapter: zodValidator,
    validators: {
      onSubmitAsync: userSchema,
    },
    onSubmit: async ({
      value,
    }: {
      value: CreateUserRequest | UpdateUserRequest | z.infer<typeof userSchema>
    }) => {
      const formattedValue = {
        ...value,
        dob: format(value.dob ?? new Date(), "yyyy-MM-dd"),
        joinedDate: format(value.joinedDate ?? new Date(), "yyyy-MM-dd"),
      }
      if (!isEditing) {
        addUser({
          ...formattedValue,
        } satisfies CreateUserRequest)
      } else {
        params &&
          updateUser({
            id: params.id,
            ...formattedValue,
          } satisfies UpdateUserRequest)
      }
    },
  })

  return (
    <form
      onSubmit={(e) => {
        e.preventDefault()
        e.stopPropagation()
        handleSubmit()
      }}
      className="!flex flex-col gap-6"
    >
      <FormControl>
        <FormLabel>First Name:</FormLabel>
        <Field
          name="firstName"
          validators={{
            onChange: userSchema.shape.firstName,
            onChangeAsyncDebounceMs: 500,
          }}
        >
          {({ handleChange, state }) => (
            <>
              <TextField
                id="txt-first-name"
                disabled={isEditing}
                defaultValue={state.value}
                color={state.meta.errors ? "error" : "primary"}
                focused={state.meta.errors.length != 0}
                onChange={(e) => handleChange(e.target.value)}
                size="small"
                inputProps={{ maxLength: 20 }}
              />
              <FormHelperText className="!text-red-500">
                {state.meta.errors}
              </FormHelperText>
            </>
          )}
        </Field>
      </FormControl>
      <FormControl>
        <FormLabel>Last Name:</FormLabel>
        <Field
          name="lastName"
          validators={{
            onChange: userSchema.shape.lastName,
            onChangeAsyncDebounceMs: 500,
          }}
        >
          {({ handleChange, state }) => (
            <>
              <TextField
                id="txt-last-name"
                defaultValue={state.value}
                disabled={isEditing}
                color={state.meta.errors ? "error" : "primary"}
                focused={state.meta.errors.length != 0}
                onChange={(e) => handleChange(e.target.value)}
                size="small"
                inputProps={{ maxLength: 50 }}
              />
              <FormHelperText className="!text-red-500">
                {state.meta.errors}
              </FormHelperText>
            </>
          )}
        </Field>
      </FormControl>
      <FormControl>
        <FormLabel>Date of Birth:</FormLabel>
        <Field name="dob" validators={{ onChange: userSchema.shape.dob }}>
          {({ handleChange, state }) => (
            <>
              <DatePicker
                name="dob"
                maxDate={today}
                defaultValue={new Date(state.value as string)}
                onChange={(value) => {
                  value && handleChange(value)
                }}
                format="dd/MM/yyyy"
                slotProps={{
                  textField: {
                    size: "small",
                    id: "dpk-date-of-birth",
                    disabled: true,
                    color: state.meta.errors.length != 0 ? "error" : "primary",
                    focused: state.meta.errors.length != 0,
                  },
                  openPickerButton: {
                    id: "btn-date-of-birth",
                  },
                }}
                sx={{
                  ...(state.meta.errors.length !== 0 && {
                    ".MuiOutlinedInput-notchedOutline": {
                      border: "2px #d32f2f solid !important",
                      borderRadius: "4px",
                    },
                  }),
                }}
              />
              <FormHelperText className="!text-red-500">
                {state.meta.errors}
              </FormHelperText>
            </>
          )}
        </Field>
      </FormControl>
      <FormControl>
        <FormLabel>Gender:</FormLabel>
        <Field name="gender" validators={{ onChange: userSchema.shape.gender }}>
          {({ handleChange, state }) => (
            <>
              <RadioGroup
                row
                aria-labelledby="demo-radio-buttons-group-label"
                name="gender"
                id="rdo-gender"
                onChange={(e) => handleChange(e.target.value as Gender)}
                defaultValue={state.value}
              >
                {genderOptions.map((item: OptionItem<Gender>) => (
                  <FormControlLabel
                    key={item.value}
                    value={item.value}
                    control={
                      <Radio
                        sx={{
                          "&.Mui-checked": {
                            color: "#e30c18",
                          },
                        }}
                      />
                    }
                    label={item.label}
                  />
                ))}
              </RadioGroup>
              <FormHelperText className="!text-red-500">
                {state.meta.errors}
              </FormHelperText>
            </>
          )}
        </Field>
      </FormControl>
      <FormControl>
        <FormLabel>Joined Date:</FormLabel>
        <Field
          name="joinedDate"
          validators={{
            onChangeListenTo: ["dob"],
            onChange: userSchema.shape.joinedDate
              .refine(
                (data) =>
                  !data ||
                  !getFieldValue("dob") ||
                  data >= new Date(getFieldValue("dob") as string),
                "Joined date is not later than Date of Birth. Please select a different date"
              )
              .refine(
                (data) =>
                  !data ||
                  !getFieldValue("dob") ||
                  differenceInYears(
                    data,
                    new Date(getFieldValue("dob") as string)
                  ) >= 18 ||
                  data < new Date(getFieldValue("dob") as string),
                "User is under 18 when joined. Please select a different date"
              ),
          }}
        >
          {({ handleChange, state }) => (
            <>
              <DatePicker
                name="joinedDate"
                maxDate={today}
                defaultValue={new Date(state.value as string)}
                onChange={(value) => value && handleChange(value)}
                format="dd/MM/yyyy"
                slotProps={{
                  textField: {
                    size: "small",
                    id: "dpk-joined-date",
                    disabled: true,
                    color: state.meta.errors ? "error" : "primary",
                    focused: state.meta.errors.length != 0,
                  },
                  openPickerButton: {
                    id: "btn-joined-date",
                  },
                }}
                sx={{
                  ...(state.meta.errors.length !== 0 && {
                    ".MuiOutlinedInput-notchedOutline": {
                      border: "2px #d32f2f solid !important",
                      borderRadius: "4px",
                    },
                  }),
                }}
              />
              <FormHelperText className="!text-red-500">
                {state.meta.errors}
              </FormHelperText>
            </>
          )}
        </Field>
      </FormControl>
      <FormControl>
        <FormLabel>Type:</FormLabel>
        <Field
          name="roleType"
          validators={{ onChange: userSchema.shape.roleType }}
        >
          {({ handleChange, state }) => (
            <>
              <Select
                onChange={(e) => handleChange(e.target.value as RoleType)}
                defaultValue={state.value}
                id="sel-role-type"
                size="small"
              >
                {roleTypeOptions.map((item: OptionItem<RoleType>) => (
                  <MenuItem key={item.label} value={item.value}>
                    {item.label}
                  </MenuItem>
                ))}
              </Select>
              <FormHelperText className="!text-red-500">
                {state.meta.errors}
              </FormHelperText>
            </>
          )}
        </Field>
      </FormControl>
      <Container className="mt-6 !flex flex-row-reverse gap-10">
        <Link to="/user">
          <Button
            id="btn-edit-user-close"
            variant="outlined"
            className="!border-black !text-black"
          >
            Close
          </Button>
        </Link>
        <Subscribe selector={(state) => [state.canSubmit, state.values]}>
          {([canSubmit, values]) => (
            <Button
              id="btn-edit-user-ok"
              type="submit"
              variant="contained"
              className="disabled:!hover:bg-red-800 !bg-red-500 !text-white disabled:!bg-red-700 disabled:!text-gray-300"
              disabled={
                !canSubmit ||
                !(values as CreateUserRequest).firstName ||
                !(values as CreateUserRequest).lastName ||
                !(values as CreateUserRequest).joinedDate ||
                !(values as CreateUserRequest).dob ||
                !(values as CreateUserRequest).roleType ||
                !(values as CreateUserRequest).gender ||
                addUserPending ||
                updateUserPending
              }
            >
              {addUserPending || updateUserPending ? (
                <CircularProgress size={25} />
              ) : (
                "Save"
              )}
            </Button>
          )}
        </Subscribe>
      </Container>
    </form>
  )
}
