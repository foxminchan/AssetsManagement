import { useEffect, useState } from "react"
import { passwordSchema, passwordValidator } from "@features/auth/auth.schema"
import { AccountStatus, AuthUser } from "@features/auth/auth.type"
import useUpdatePassword from "@features/auth/useUpdatePassword"
import { useShowHint } from "@libs/hooks/useShowHint"
import Visibility from "@mui/icons-material/Visibility"
import VisibilityOff from "@mui/icons-material/VisibilityOff"
import { FormHelperText } from "@mui/material"
import IconButton from "@mui/material/IconButton"
import InputAdornment from "@mui/material/InputAdornment"
import TextField from "@mui/material/TextField"
import { useForm } from "@tanstack/react-form"
import { z } from "zod"

import AlertModal from "./alert-modal"
import InputModal from "./input-modal"

type ChangePasswordModalProps = {
  open: boolean
  onClose?: () => void
  user: AuthUser | null
  FirstTime: boolean
}

export default function ChangePasswordModal({
  open,
  onClose,
  user,
  FirstTime,
}: Readonly<ChangePasswordModalProps>) {
  const [showOldPassword, setShowOldPassword] = useState(false)
  const [showNewPassword, setShowNewPassword] = useState(false)
  const [canSubmit, setCanSubmit] = useState(false)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [showOldPasswordError, setShowOldPasswordError] = useState(false)
  const [showNewPasswordError, setShowNewPasswordError] = useState(false)
  const showOldPasswordHint = useShowHint(setShowOldPassword)
  const showNewPasswordHint = useShowHint(setShowNewPassword)
  const {
    mutate: updatePassword,
    isSuccess,
    error,
    isError,
  } = useUpdatePassword()
  const [openAlertModal, setOpenAlertModal] = useState(false)

  const { Field, handleSubmit, getFieldValue, reset } = useForm({
    defaultValues: {
      id: user?.id,
      oldPassword: "",
      newPassword: "",
    } as z.infer<typeof passwordSchema>,

    validatorAdapter: passwordValidator({}),

    onSubmit: async ({ value }) => {
      updatePassword({
        id: value.id,
        oldPassword: value.oldPassword,
        newPassword: value.newPassword,
      })
      setIsSubmitting(true)
      setShowOldPasswordError(false)
    },
  })

  useEffect(() => {
    if (isSuccess) {
      closeInputModal()
      setOpenAlertModal(true)
      setIsSubmitting(false)
      setShowNewPasswordError(false)
    }
    if (isError) {
      setIsSubmitting(false)
      setShowOldPasswordError(true)
    }
  }, [isSuccess, isError, error])

  const getErrorMessage = (_error: AppAxiosError, identifier: string) => {
    let error = JSON.parse(JSON.stringify(_error.response?.data))
    let errors = error.value
    if (errors) {
      for (const element of errors) {
        const _error = element
        if (_error.identifier === identifier) {
          return _error.errorMessage
        }
      }
    }
  }

  const handleFormChange = () => {
    const oldPassword = getFieldValue("oldPassword")
    const newPassword = getFieldValue("newPassword")
    setCanSubmit(oldPassword !== "" && newPassword !== "")
  }

  const closeInputModal = () => {
    onClose?.()
    reset()
    setCanSubmit(false)
    setShowOldPasswordError(false)
    setShowNewPasswordError(false)
  }

  const closeAlertModal = () => {
    setOpenAlertModal(false)
    if (user?.accountStatus === AccountStatus.FirstTime) {
      window.location.reload()
    }
  }

  return (
    <div>
      <InputModal
        open={open}
        onClose={closeInputModal}
        onOk={() => {
          setShowNewPasswordError(true)
          handleSubmit()
        }}
        title="Change Password"
        buttonOkLabel="Save"
        buttonOkDisabled={!canSubmit || isSubmitting}
        buttonCloseShow={!FirstTime}
      >
        {FirstTime && (
          <div>
            <p>This is the first time you logged in.</p>
            <p>You have to change your password to continue.</p>
          </div>
        )}
        <form onChange={handleFormChange}>
          <Field
            name="oldPassword"
            validators={{
              onChange: passwordSchema.shape.oldPassword,
              onChangeAsyncDebounceMs: 500,
            }}
          >
            {({ state, handleChange, handleBlur }) => (
              <div>
                <TextField
                  error={showOldPasswordError}
                  margin="normal"
                  fullWidth
                  id="old-password"
                  defaultValue={state.value}
                  onChange={(e) => {
                    setShowOldPasswordError(false)
                    handleChange(e.target.value)
                  }}
                  onBlur={handleBlur}
                  label="Old Password"
                  type={showOldPassword ? "text" : "password"}
                  autoComplete="old-password"
                  InputProps={{
                    endAdornment: (
                      <InputAdornment position="end">
                        <IconButton
                          aria-label="toggle password visibility"
                          onMouseDown={showOldPasswordHint.onMouseDown}
                          onMouseUp={showOldPasswordHint.onMouseUp}
                          onMouseLeave={showOldPasswordHint.onMouseLeave}
                        >
                          {showOldPassword ? <VisibilityOff /> : <Visibility />}
                        </IconButton>
                      </InputAdornment>
                    ),
                  }}
                />
                <FormHelperText className="px-2 !text-red-500">
                  {showOldPasswordError &&
                    error &&
                    getErrorMessage(error, "OldPassword")}
                </FormHelperText>
              </div>
            )}
          </Field>
          <Field
            name="newPassword"
            validators={{
              onChangeListenTo: ["oldPassword"],
              onChange: passwordSchema.shape.newPassword.refine(
                (newPassword) => newPassword !== getFieldValue("oldPassword"),
                {
                  message: "New password must not be the same as old password",
                }
              ),
              onChangeAsyncDebounceMs: 500,
            }}
          >
            {({ state, handleChange, handleBlur }) => (
              <div>
                <TextField
                  margin="normal"
                  fullWidth
                  id="new-password"
                  defaultValue={state.value}
                  onChange={(e) => {
                    setShowNewPasswordError(false)
                    handleChange(e.target.value)
                  }}
                  onBlur={handleBlur}
                  label="New Password"
                  type={showNewPassword ? "text" : "password"}
                  autoComplete="new-password"
                  InputProps={{
                    endAdornment: (
                      <InputAdornment position="end">
                        <IconButton
                          aria-label="toggle password visibility"
                          onMouseDown={showNewPasswordHint.onMouseDown}
                          onMouseUp={showNewPasswordHint.onMouseUp}
                          onMouseLeave={showNewPasswordHint.onMouseLeave}
                        >
                          {showNewPassword ? <VisibilityOff /> : <Visibility />}
                        </IconButton>
                      </InputAdornment>
                    ),
                  }}
                />
                <FormHelperText className="px-2 !text-red-500">
                  {showNewPasswordError && state.meta.errors}
                </FormHelperText>
              </div>
            )}
          </Field>
        </form>
      </InputModal>
      <AlertModal
        open={openAlertModal}
        onClose={closeAlertModal}
        title="Change Password"
        message="Your password has been changed successfully!"
      />
    </div>
  )
}
