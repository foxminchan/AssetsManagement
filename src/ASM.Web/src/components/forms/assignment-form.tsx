import { useEffect, useState } from "react"
import { AssetState } from "@features/assets/asset.type"
import useListAsset from "@features/assets/useListAsset"
import { createAssignmentSchema } from "@features/assignments/assignment.schema"
import {
  CreateAssignmentRequest,
  UpdateAssignmentRequest,
  ViewUpdateAssignmentRequest,
} from "@features/assignments/assignment.type"
import useCreateAssignment from "@features/assignments/useCreateAssignment"
import useListUsers from "@features/users/useListUsers"
import { RoleType } from "@features/users/user.type"
import { selectedRowAsset } from "@libs/jotai/assetAtom"
import { assignmentAtoms, selectedRowUser } from "@libs/jotai/assignmentAtom"
import SearchIcon from "@mui/icons-material/Search"
import {
  Box,
  Button,
  CircularProgress,
  Container,
  DialogActions,
  DialogContent,
  DialogTitle,
  FormControl,
  FormHelperText,
  FormLabel,
  Grid,
  IconButton,
  InputAdornment,
  TextField,
} from "@mui/material"
import Dialog from "@mui/material/Dialog"
import { DatePicker } from "@mui/x-date-pickers"
import { useForm } from "@tanstack/react-form"
import { Link, useNavigate, useSearch } from "@tanstack/react-router"
import { zodValidator } from "@tanstack/zod-form-adapter"
import { format } from "date-fns"
import { useAtom, useSetAtom } from "jotai"
import { z } from "zod"

import AssetDialog from "../dialogs/asset/asset-dialog"
import UserDialog from "../dialogs/user/user-dialog"
import SearchInput from "../fields/search-dialog"

const types = ["All", RoleType.Admin, RoleType.Staff]

type AssignmentProps = {
  initialData?:
    | CreateAssignmentRequest
    | ViewUpdateAssignmentRequest
    | UpdateAssignmentRequest
  isEditing?: boolean
}

export default function AssignmentForm({
  initialData,
}: Readonly<AssignmentProps>) {
  const userParams = useSearch({
    strict: false,
  })
  const queryUserParameters = {
    orderBy: (userParams as { orderBy?: string }).orderBy ?? "staffCode",
    isDescending:
      (userParams as { isDescending?: boolean }).isDescending ?? false,
    RoleType:
      (userParams as { roleType?: string }).roleType === types[0]
        ? undefined
        : (userParams as { roleType?: string }).roleType,
    search: (userParams as { search?: string }).search ?? "",
  }
  const { data: userData, isLoading: listLoading } =
    useListUsers(queryUserParameters)
  const [userChoose] = useAtom(selectedRowUser)
  const [isUserDialogOpen, setIsUserDialogOpen] = useState(false)

  const navigate = useNavigate({ from: "/assignment/new" })

  const assetParams = useSearch({
    strict: false,
  })
  const queryAssetParameters = {
    orderBy: (assetParams as { orderBy?: string }).orderBy ?? "assetCode",
    isDescending:
      (assetParams as { isDescending?: boolean }).isDescending ?? false,
    search: (assetParams as { search?: string }).search ?? undefined,
    state: [AssetState.Available],
  }
  const { data: assetData, isLoading: listAssetLoading } =
    useListAsset(queryAssetParameters)
  const [assetChoose] = useAtom(selectedRowAsset)
  const [isAssetDialogOpen, setIsAssetDialogOpen] = useState(false)
  const isEditing = !!initialData

  const setAssignmentId = useSetAtom(assignmentAtoms)
  const today = new Date(Date.now())
  const defaultValues = initialData
  const {
    data: createdAssignmentId,
    mutate: createAssignment,
    isSuccess: createAssignmentSuccess,
    isPending: createAssignmentPending,
  } = useCreateAssignment()

  const { Field, Subscribe, handleSubmit, getFieldValue, setFieldValue } =
    useForm({
      defaultValues,
      validatorAdapter: zodValidator,
      validators: {
        onSubmitAsync: createAssignmentSchema,
      },
      onSubmit: async ({
        value,
      }: {
        value:
          | CreateAssignmentRequest
          | ViewUpdateAssignmentRequest
          | UpdateAssignmentRequest
          | z.infer<typeof createAssignmentSchema>
      }) => {
        const formattedValue = {
          ...value,
          assignedDate: format(value.assignedDate, "yyyy-MM-dd"),
        }
        if (!isEditing) {
          const request: CreateAssignmentRequest = {
            ...formattedValue,
            assetId: assetChoose!.id,
            userId: userChoose!.id,
            note: formattedValue.note ?? "",
          }
          console.log(request)
          createAssignment(request)
        }
      },
    })
  useEffect(() => {
    if (createAssignmentSuccess) {
      navigate({
        to: "/assignment",
      })
      setAssignmentId(createdAssignmentId)
    }
  }, [createAssignmentSuccess])

  const onSubmitValue = (type: string, name: string | undefined) => {
    if (type === "Asset" && name !== undefined) {
      setFieldValue("assetId", name)
    } else if (name !== undefined) {
      setFieldValue("userId", name)
    }
    setIsAssetDialogOpen(false)
    setIsUserDialogOpen(false)
  }

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
        <FormLabel>User</FormLabel>
        <Field
          name="userId"
          validators={{
            onChange: createAssignmentSchema.shape.userId,
            onChangeAsyncDebounceMs: 500,
          }}
        >
          {({ handleChange, state }) => (
            <Box display="flex" alignItems="center">
              <TextField
                id="txt-asset-name"
                disabled={true}
                defaultValue={state.value}
                value={state.value}
                color={state.meta.errors.length ? "error" : "primary"}
                focused={state.meta.errors.length !== 0}
                onChange={(e) => handleChange(e.target.value)}
                size="small"
                inputProps={{ maxLength: 20 }}
                InputProps={{
                  endAdornment: (
                    <InputAdornment position="end">
                      <IconButton
                        onClick={() => setIsUserDialogOpen(true)}
                        color="primary"
                        size="small"
                      >
                        <SearchIcon className="text-red-500" />
                      </IconButton>
                    </InputAdornment>
                  ),
                }}
                style={{ flexGrow: 1 }}
              />
            </Box>
          )}
        </Field>
      </FormControl>
      <FormControl>
        <FormLabel>Asset</FormLabel>
        <Field
          name="assetId"
          validators={{
            onChange: createAssignmentSchema.shape.userId,
            onChangeAsyncDebounceMs: 500,
          }}
        >
          {({ handleChange, state }) => (
            <Box display="flex" alignItems="center">
              <TextField
                id="txt-asset-name"
                disabled={true}
                defaultValue={state.value}
                value={state.value}
                color={state.meta.errors.length ? "error" : "primary"}
                focused={state.meta.errors.length !== 0}
                onChange={(e) => handleChange(e.target.value)}
                size="small"
                inputProps={{ maxLength: 20 }}
                InputProps={{
                  endAdornment: (
                    <InputAdornment position="end">
                      <IconButton
                        onClick={() => setIsAssetDialogOpen(true)}
                        color="primary"
                        size="small"
                      >
                        <SearchIcon className="text-red-500" />
                      </IconButton>
                    </InputAdornment>
                  ),
                }}
                style={{ flexGrow: 1 }}
              />
            </Box>
          )}
        </Field>
      </FormControl>

      <FormControl>
        <FormLabel>Assigned Date:</FormLabel>
        <Field
          name="assignedDate"
          defaultValue={today}
          validators={{
            onChangeListenTo: ["assignedDate"],
            onChange: createAssignmentSchema.shape.assignedDate.refine(
              (data) => {
                if (!data) return true // Allow empty dates if not required
                const selectedDate = getFieldValue("assignedDate") as Date
                return selectedDate
              },
              "Date must be today or in the future"
            ),
          }}
        >
          {({ handleChange, state }) => (
            <>
              <DatePicker
                className=""
                minDate={today}
                defaultValue={new Date(state.value as Date)}
                name="assignedDate"
                onChange={(value) => {
                  value && handleChange(value)
                }}
                format="dd/MM/yyyy"
                slotProps={{
                  textField: {
                    size: "small",
                    id: "dpk-assigned-date",
                    disabled: true,
                    color: state.meta.errors.length !== 0 ? "error" : "primary",
                    focused: state.meta.errors.length !== 0,
                  },
                  openPickerButton: {
                    id: "btn-assigned-date",
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
        <FormLabel>Note</FormLabel>
        <Field
          name="note"
          validators={{
            onChange: createAssignmentSchema.shape.note,
            onChangeAsyncDebounceMs: 500,
          }}
        >
          {({ handleChange, state }) => (
            <Box
              display="flex"
              alignItems="center"
              className="rounded-lg bg-white"
            >
              <TextField
                id="txt-note"
                defaultValue={state.value ?? ""}
                onChange={(e) => handleChange(e.target.value)}
                multiline
                rows={3}
                variant="outlined"
                fullWidth
                inputProps={{ maxLength: 200 }}
                InputProps={{
                  className: " border-0 focus:ring-0",
                  disableUnderline: true,
                }}
              />
            </Box>
          )}
        </Field>
      </FormControl>
      <Container className="mt-6 !flex flex-row-reverse gap-10">
        <Link to="/assignment">
          <Button
            id="btn-edit-userc-close"
            variant="outlined"
            className="!border-black-400 !text-gray-400"
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
                !(values as CreateAssignmentRequest).userId ||
                !(values as CreateAssignmentRequest).assetId ||
                !(values as CreateAssignmentRequest).assignedDate ||
                createAssignmentPending
              }
            >
              {createAssignmentPending ? (
                <CircularProgress size={25} />
              ) : (
                "Save"
              )}
            </Button>
          )}
        </Subscribe>
      </Container>

      {/* User Dialog */}
      <Dialog
        open={isUserDialogOpen}
        onClose={() => setIsUserDialogOpen(false)}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="sm"
        fullWidth
      >
        <div className="w-full transform overflow-hidden rounded-lg bg-white shadow-xl transition-all sm:max-w-4xl">
          <DialogTitle
            id="alert-dialog-title"
            className="bg-white-100 flex items-center justify-between border-b p-4 text-red-500"
          >
            <span>Select User</span>
            <Grid item xs={7} className="!flex !flex-row-reverse">
              <SearchInput />
            </Grid>
          </DialogTitle>
          <DialogContent className="p-4">
            <UserDialog
              data={[...(userData?.users || [])]}
              isLoading={listLoading}
            />
          </DialogContent>
          <DialogActions className="border-t bg-gray-100 p-4">
            <Button
              id="btn-save"
              disabled={!userChoose?.name}
              onClick={() => {
                navigate({ to: "/assignment/new" })
                onSubmitValue("User", userChoose?.name)
              }}
              className="disabled:!hover:bg-red-800 !bg-red-500 !text-white disabled:!bg-red-700 disabled:!text-gray-300"
            >
              Save
            </Button>
            <Button
              id="btn-cancel"
              onClick={() => {
                navigate({ to: "/assignment/new" })
                setIsUserDialogOpen(false)
              }}
              autoFocus
              className="!hover:bg-gray-700 rounded !bg-gray-100 px-4 py-2 font-bold !text-black"
            >
              Cancel
            </Button>
          </DialogActions>
        </div>
      </Dialog>

      <Dialog
        open={isAssetDialogOpen}
        onClose={() => setIsAssetDialogOpen(false)}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="sm"
        fullWidth
      >
        <div className="w-full transform overflow-hidden rounded-lg bg-white shadow-xl transition-all sm:max-w-4xl">
          <DialogTitle
            id="alert-dialog-title"
            className="bg-white-100 flex items-center justify-between border-b p-4 text-red-500"
          >
            <span>Select Asset</span>
            <Grid item xs={7} className="!flex !flex-row-reverse">
              <SearchInput />
            </Grid>
          </DialogTitle>
          <DialogContent className="p-4">
            <AssetDialog
              data={[...(assetData?.assets || [])]}
              isLoading={listAssetLoading}
            />
          </DialogContent>
          <DialogActions className="bg-white-100 border-t p-4">
            <Button
              disabled={!assetChoose?.name}
              onClick={() => {
                navigate({ to: "/assignment/new" })
                onSubmitValue("Asset", assetChoose?.name)
              }}
              className="disabled:!hover:bg-red-800 !bg-red-500 !text-white disabled:!bg-red-700 disabled:!text-gray-300"
            >
              Save
            </Button>
            <Button
              onClick={() => {
                navigate({ to: "/assignment/new" })
                setIsAssetDialogOpen(false)
              }}
              autoFocus
              className="!hover:bg-gray-700 rounded !bg-gray-100 px-4 py-2 font-bold !text-black"
            >
              Cancel
            </Button>
          </DialogActions>
        </div>
      </Dialog>
    </form>
  )
}
