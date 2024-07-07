import { useEffect, useState } from "react"
import { AssetState } from "@features/assets/asset.type"
import useListAsset from "@features/assets/useListAssets"
import { createAssignmentSchema } from "@features/assignments/assignment.schema"
import {
  CreateAssignmentRequest,
  UpdateAssignmentRequest,
  ViewUpdateAssignmentRequest,
} from "@features/assignments/assignment.type"
import useCreateAssignment from "@features/assignments/useCreateAssignment"
import useUpdateAssignment from "@features/assignments/useUpdateAssignment"
import useListUsers from "@features/users/useListUsers"
import { RoleType } from "@features/users/user.type"
import { selectedRowAsset, submitAsset } from "@libs/jotai/assetAtom"
import {
  featuredAssignmentAtom,
  selectedRowUser,
  submitUser,
} from "@libs/jotai/assignmentAtom"
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
import { FieldMeta, useForm } from "@tanstack/react-form"
import { Link, useNavigate, useParams, useSearch } from "@tanstack/react-router"
import { zodValidator } from "@tanstack/zod-form-adapter"
import { format } from "date-fns"
import { useAtom, useSetAtom } from "jotai"
import { match } from "ts-pattern"
import { z } from "zod"

import AssetDialog from "../dialogs/asset/asset-dialog"
import UserDialog from "../dialogs/user/user-dialog"
import SearchInput from "../fields/search-dialog"

const types = ["All", RoleType.Admin, RoleType.Staff]

const initialAssetChoose = {
  id: "",
  name: "",
}

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
  const navigate = useNavigate()
  const setFeaturedAssignmentId = useSetAtom(featuredAssignmentAtom)
  const userParams = useSearch({
    strict: false,
  })
  const queryUserParameters = {
    orderBy: (userParams as { orderBy?: string }).orderBy ?? "StaffCode",
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
  const [userChoose, setUserChooser] = useAtom(selectedRowUser)
  const [isUserDialogOpen, setIsUserDialogOpen] = useState(false)
  const [assetSubmit, setAssetSubmit] = useAtom(submitAsset)
  const [userSubmit, setUserSubmit] = useAtom(submitUser)

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
  const [assetChoose, setAssetChoose] = useAtom(selectedRowAsset)
  const [isAssetDialogOpen, setIsAssetDialogOpen] = useState(false)
  const isEditing = !!initialData
  const today = new Date(Date.now())
  const defaultValues = initialData
  const params = isEditing
    ? useParams({ from: "/_authenticated/assignment/$id" })
    : null
  const {
    data: createdAssignmentId,
    mutate: createAssignment,
    isSuccess: createAssignmentSuccess,
    isPending: createAssignmentPending,
  } = useCreateAssignment()

  const {
    mutate: updateAssignment,
    isSuccess: updateAssignmentSuccess,
    isPending: updateAssignmentPending,
  } = useUpdateAssignment()

  const { Field, Subscribe, handleSubmit, setFieldValue } = useForm({
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
      let userIdUpdate = ""
      let assetIdUpdate = ""
      if (params) {
        const { assetName, userName } =
          formattedValue as unknown as ViewUpdateAssignmentRequest

        assetIdUpdate =
          assetSubmit?.name !== "" ? assetSubmit?.id ?? "" : assetName

        userIdUpdate = userSubmit?.name !== "" ? userSubmit?.id ?? "" : userName
      }

      if (!isEditing) {
        const request: CreateAssignmentRequest = {
          ...formattedValue,
          assetId: assetSubmit?.id as string,
          userId: userSubmit?.id as string,
          note: formattedValue.note ?? "",
        }
        createAssignment(request)
      } else {
        params &&
          updateAssignment({
            id: params.id,
            ...formattedValue,
            assignedDate: format(value.assignedDate, "yyyy-MM-dd"),
            assetId: assetIdUpdate,
            userId: userIdUpdate,
            note: (formattedValue as unknown as ViewUpdateAssignmentRequest)
              .note,
          } satisfies UpdateAssignmentRequest)
      }
    },
  })

  const clearData = () => {
    setUserSubmit(initialAssetChoose)
    setAssetSubmit(initialAssetChoose)
  }

  useEffect(() => {
    if (createAssignmentSuccess) {
      setAssetSubmit(initialAssetChoose)
      setFeaturedAssignmentId(createdAssignmentId)
      navigate({ from: "/assignment/new", to: "/assignment" })
    }
  }, [createAssignmentSuccess])

  useEffect(() => {
    if (updateAssignmentSuccess) {
      params && setFeaturedAssignmentId(params.id)
      navigate({ from: "/assignment/$id", to: "/assignment" })
      setAssetSubmit(initialAssetChoose)
    }
  }, [updateAssignmentSuccess])

  const onSubmitValue = (type: string, name: string | undefined) => {
    match({ type, name })
      .with({ type: "Asset", name: name }, () => {
        setAssetSubmit(assetChoose)
        setAssetChoose(initialAssetChoose)
        setFieldValue("assetId", name!)
      })
      .with({ name: name }, () => {
        setUserSubmit(userChoose)
        setUserChooser(initialAssetChoose)
        setFieldValue("userId", name!)
      })
    setIsAssetDialogOpen(false)
    setIsUserDialogOpen(false)
  }

  const getUserValue = (state: { value: any; meta?: FieldMeta }) => {
    if (params && userSubmit?.name !== "") {
      return userSubmit?.name
    } else if (params && userSubmit?.name === "") {
      return state.value
    } else {
      return state.value
    }
  }
  const getAssetValue = (state: { value: any; meta?: FieldMeta }) => {
    if (params && assetSubmit?.name !== "") {
      return assetSubmit?.name
    } else if (params && assetSubmit?.name === "") {
      return state.value
    } else {
      return state.value
    }
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
                id="txt-user-name"
                disabled={true}
                defaultValue={state.value}
                value={getUserValue(state)}
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
                value={getAssetValue(state)}
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
          defaultValue={params ? initialData?.assignedDate : today}
        >
          {({ handleChange, state }) => (
            <>
              <DatePicker
                className=""
                minDate={today}
                defaultValue={new Date((state.value as Date) || today)}
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
                defaultValue={state.value || ""}
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
            onClick={clearData}
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
                createAssignmentPending ||
                updateAssignmentPending
              }
            >
              {createAssignmentPending || updateAssignmentPending ? (
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
              isChoose={
                userData?.users.find((x) => x.userName === initialData?.userId)
                  ?.id as string
              }
            />
          </DialogContent>
          <DialogActions className="border-t bg-gray-100 p-4">
            <Button
              id="btn-save"
              disabled={!userChoose?.name}
              onClick={() => {
                onSubmitValue("User", userChoose?.name)
                if (params) {
                  queryUserParameters.search = ""
                  navigate({ to: `/assignment/${params?.id}` })
                } else {
                  navigate({ to: "/assignment/new" })
                }
              }}
              className="disabled:!hover:bg-red-800 !bg-red-500 !text-white disabled:!bg-red-700 disabled:!text-gray-300"
            >
              Save
            </Button>
            <Button
              id="btn-cancel"
              onClick={() => {
                setIsUserDialogOpen(false)
                if (params) {
                  setUserChooser(initialAssetChoose)
                  navigate({ to: `/assignment/${params?.id}` })
                  setFieldValue("userId", initialData?.userId as string)
                } else {
                  navigate({ to: "/assignment/new" })
                }
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
                if (params) {
                  navigate({ to: `/assignment/${params?.id}` })
                } else {
                  navigate({ to: "/assignment/new" })
                }
                onSubmitValue("Asset", assetChoose?.name)
              }}
              className="disabled:!hover:bg-red-800 !bg-red-500 !text-white disabled:!bg-red-700 disabled:!text-gray-300"
            >
              Save
            </Button>
            <Button
              onClick={() => {
                setIsAssetDialogOpen(false)
                if (params) {
                  setFieldValue("assetId", initialData?.assetId as string)
                  navigate({ to: `/assignment/${params?.id}` })
                } else {
                  navigate({ to: "/assignment/new" })
                }
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
