import { useEffect, useState } from "react"
import { assetSchema } from "@features/assets/asset.schema"
import {
  AssetState,
  CreateAssetRequest,
  UpdateAssetRequest,
} from "@features/assets/asset.type"
import useCreateAsset from "@features/assets/useCreateAsset"
import useUpdateAsset from "@features/assets/useUpdateAsset"
import { categorySchema } from "@features/categories/category.schema"
import {
  Category,
  CreateCategoryRequest,
} from "@features/categories/category.type"
import useCreateCategory from "@features/categories/useCreateCategory"
import useListCategories from "@features/categories/useListCategories"
import {
  createAssetStateOptions,
  updateAssetStateOptions,
} from "@libs/constants/options"
import { featuredAssetAtom } from "@libs/jotai/assetAtom"
import CheckRoundedIcon from "@mui/icons-material/CheckRounded"
import CloseRoundedIcon from "@mui/icons-material/CloseRounded"
import {
  Button,
  CircularProgress,
  Container,
  Divider,
  FormControl,
  FormControlLabel,
  FormHelperText,
  FormLabel,
  Grid,
  IconButton,
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
import { useSetAtom } from "jotai"
import { z } from "zod"

import { OptionItem } from "@/types/data"

type AssetFormProps = {
  initialData?: UpdateAssetRequest
}

export default function AssetForm({ initialData }: Readonly<AssetFormProps>) {
  const navigate = useNavigate()
  const today = new Date(Date.now())
  const isUpdating = !!initialData
  const setFeaturedAssetId = useSetAtom(featuredAssetAtom)

  const compare = (value1: any, value2: any) => {
    if (
      typeof value1 === "object" &&
      value1 !== null &&
      typeof value2 === "object" &&
      value2 !== null
    ) {
      const keys1 = Object.keys(value1)
      const keys2 = Object.keys(value2)
      if (keys1.length !== keys2.length) {
        return false
      }

      for (let key of keys1) {
        if (!compare(value1[key], value2[key])) {
          return false
        }
      }

      if (value1 instanceof Date && value2 instanceof Date) {
        return value1.getTime() === value2.getTime()
      }

      return true
    } else {
      return value1 === value2
    }
  }

  const getErrorMessage = (error: AppAxiosError, identifier: string) => {
    if (
      identifier === "Name" &&
      error.response?.data.value?.some(
        (e) => e.errorCode === "409" && e.identifier === "IX_Categories_Name"
      )
    ) {
      return "Category is already existed. Please enter a different category"
    } else if (
      identifier === "Prefix" &&
      error.response?.data.value?.some(
        (e) => e.errorCode === "409" && e.identifier === "IX_Categories_Prefix"
      )
    ) {
      return "Prefix is already existed. Please enter a different prefix"
    }
  }

  const params = isUpdating
    ? useParams({ from: "/_authenticated/asset/$id" })
    : null

  const [openCategorySelect, setOpenCategorySelect] = useState(false)
  const [showCategoryNameError, setShowCategoryNameError] = useState(false)
  const [showCategoryPrefixError, setShowCategoryPrefixError] = useState(false)
  const [isCreatingCategory, setIsCreatingCategory] = useState(false)
  const { data: listCategories } = useListCategories()

  const {
    data: createdCategoryId,
    mutate: createCategory,
    isPending: createCategoryIsPending,
    isSuccess: createCategoryIsSuccess,
    isError: createCategoryIsError,
    error: createCategoryError,
  } = useCreateCategory()

  useEffect(() => {
    if (createCategoryIsSuccess) {
      setCategoryFieldValue("name", "")
      setCategoryFieldValue("prefix", "")
      setAssetFieldValue("categoryId", createdCategoryId, { touch: true })
      setOpenCategorySelect(false)
    }

    if (createCategoryError) {
      if (getErrorMessage(createCategoryError, "Name") !== undefined) {
        setShowCategoryNameError(true)
      }
      if (getErrorMessage(createCategoryError, "Prefix") !== undefined) {
        setShowCategoryPrefixError(true)
      }
    }
  }, [createCategoryIsSuccess, createCategoryIsError, createCategoryError])

  useEffect(() => {
    const form = document.getElementById("create-category")
    if (form) {
      form.scrollIntoView({ behavior: "smooth" })
    }

    if (showCategoryNameError) {
      const errorComponent = document.getElementById(
        "create-category-name-error"
      )
      if (errorComponent) {
        errorComponent.scrollIntoView({ behavior: "smooth" })
      }
    }

    if (showCategoryPrefixError) {
      const errorComponent = document.getElementById(
        "create-category-prefix-error"
      )
      if (errorComponent) {
        errorComponent.scrollIntoView({ behavior: "smooth" })
      }
    }
  }, [isCreatingCategory, showCategoryNameError, showCategoryPrefixError])

  const {
    Field: CategoryField,
    Subscribe: CategorySubscribe,
    handleSubmit: handleCategorySubmit,
    setFieldValue: setCategoryFieldValue,
  } = useForm({
    defaultValues: {
      name: "",
      prefix: "",
    } as z.infer<typeof categorySchema>,
    validatorAdapter: zodValidator,
    onSubmit: async ({ value }: { value: CreateCategoryRequest }) => {
      createCategory(value satisfies CreateCategoryRequest)
    },
  })

  const {
    data: createdAssetId,
    mutate: createAsset,
    isSuccess: createAssetIsSuccess,
    isPending: createAssetIsPending,
  } = useCreateAsset()

  useEffect(() => {
    if (createAssetIsSuccess) {
      navigate({ from: "/asset/new", to: "/asset" })
      setFeaturedAssetId(createdAssetId)
    }
  }, [createAssetIsSuccess])

  const {
    mutate: updateAsset,
    isSuccess: updateAssetIsSuccess,
    isPending: updateAssetIsPending,
  } = useUpdateAsset()

  useEffect(() => {
    if (updateAssetIsSuccess) {
      navigate({ from: "/asset/$id", to: "/asset" })
      params && setFeaturedAssetId(params.id)
    }
  }, [updateAssetIsSuccess])

  const {
    Field: AssetField,
    Subscribe: AssetSubscribe,
    handleSubmit: handleAssetSubmit,
    setFieldValue: setAssetFieldValue,
  } = useForm({
    defaultValues:
      initialData ??
      ({
        name: "",
        categoryId: "",
        specification: "",
        state: AssetState.Available,
      } as z.infer<typeof assetSchema>),
    validatorAdapter: zodValidator,
    onSubmit: async ({ value }: { value: CreateAssetRequest }) => {
      if (!isUpdating) {
        createAsset(value satisfies CreateAssetRequest)
      } else {
        const updateValue = {
          ...value,
          id: initialData.id,
        }
        updateAsset(updateValue satisfies UpdateAssetRequest)
      }
    },
  })

  return (
    <form
      onSubmit={(e) => {
        e.preventDefault()
        e.stopPropagation()
        handleAssetSubmit()
      }}
      id="create-asset"
      className="!flex flex-col gap-6"
    >
      <FormControl>
        <FormLabel>Name:</FormLabel>
        <AssetField name="name">
          {({ handleChange, state }) => (
            <>
              <TextField
                id="txt-asset-name"
                value={state.value}
                color={state.meta.errors ? "error" : "primary"}
                focused={state.meta.errors.length != 0}
                onChange={(e) => handleChange(e.target.value)}
                size="small"
                inputProps={{ maxLength: 150 }}
              />
              <FormHelperText className="!text-red-500">
                {state.meta.errors}
              </FormHelperText>
            </>
          )}
        </AssetField>
      </FormControl>
      <FormControl>
        <FormLabel>Category: </FormLabel>
        <AssetField name="categoryId">
          {({ handleChange, state }) => (
            <>
              <Select
                open={openCategorySelect}
                onOpen={() => setOpenCategorySelect(true)}
                onClose={() => {
                  setOpenCategorySelect(false)
                  setIsCreatingCategory(false)
                  setShowCategoryNameError(false)
                  setShowCategoryPrefixError(false)
                }}
                value={state.value}
                disabled={isUpdating}
                onChange={(e) => handleChange(e.target.value)}
                MenuProps={{
                  slotProps: { paper: { sx: { maxHeight: 195 } } },
                  MenuListProps: { sx: { p: 0 } },
                }}
                size="small"
              >
                {listCategories?.categories.map((category: Category) => (
                  <MenuItem key={category.id} value={category.id}>
                    {category.name}
                  </MenuItem>
                ))}
                <Divider className="!my-0 !bg-gray-300" />
                <form
                  onSubmit={(e) => {
                    e.preventDefault()
                    e.stopPropagation()
                    handleCategorySubmit()
                  }}
                  id="create-category"
                >
                  {!isCreatingCategory && (
                    <MenuItem onClick={() => setIsCreatingCategory(true)}>
                      <p className="italic !text-red-500 !underline">
                        Add new category
                      </p>
                    </MenuItem>
                  )}
                  {isCreatingCategory && (
                    <MenuItem
                      className="!flex-col !bg-gray-100"
                      disableRipple
                      onKeyDown={(e) => {
                        if (isCreatingCategory) {
                          e.stopPropagation()
                        }
                      }}
                    >
                      <Grid container>
                        <Grid item xs={8}>
                          <FormControl fullWidth>
                            <CategoryField name="name">
                              {({ handleChange, state }) => (
                                <TextField
                                  className="!bg-white"
                                  id="txt-category-name"
                                  label="Name"
                                  disabled={isUpdating}
                                  value={state.value}
                                  color={
                                    state.meta.errors ? "error" : "primary"
                                  }
                                  focused={state.meta.errors.length != 0}
                                  onChange={(e) => {
                                    setShowCategoryNameError(false)
                                    handleChange(e.target.value)
                                  }}
                                  size="small"
                                  inputProps={{ maxLength: 100 }}
                                />
                              )}
                            </CategoryField>
                          </FormControl>
                        </Grid>
                        <Grid item xs={2}>
                          <FormControl fullWidth>
                            <CategoryField name="prefix">
                              {({ handleChange, state }) => (
                                <TextField
                                  className="!bg-white"
                                  id="txt-category-prefix"
                                  label="Prefix"
                                  disabled={isUpdating}
                                  value={state.value}
                                  color={
                                    state.meta.errors ? "error" : "primary"
                                  }
                                  focused={state.meta.errors.length != 0}
                                  onChange={(e) => {
                                    setShowCategoryPrefixError(false)
                                    handleChange(e.target.value)
                                  }}
                                  size="small"
                                  inputProps={{ maxLength: 2 }}
                                />
                              )}
                            </CategoryField>
                          </FormControl>
                        </Grid>

                        <CategorySubscribe
                          selector={(state) => [state.canSubmit, state.values]}
                        >
                          {([canSubmit, values]) => (
                            <Grid
                              item
                              xs={1}
                              className="!flex !content-center !justify-center"
                            >
                              <IconButton
                                id="btn-category-submit"
                                aria-label="submit create category"
                                edge="end"
                                className="!text-red-500 disabled:!text-gray-500"
                                disabled={
                                  !canSubmit ||
                                  !(values as CreateCategoryRequest).name ||
                                  !(values as CreateCategoryRequest).prefix ||
                                  showCategoryNameError ||
                                  showCategoryPrefixError ||
                                  createCategoryIsPending
                                }
                                type="submit"
                              >
                                <CheckRoundedIcon />
                              </IconButton>
                            </Grid>
                          )}
                        </CategorySubscribe>
                        <Grid
                          item
                          xs={1}
                          className="!flex !content-center !justify-center"
                        >
                          <IconButton
                            id="btn-category-cancel"
                            aria-label="cancel create category"
                            onClick={() => {
                              setIsCreatingCategory(false)
                              setShowCategoryNameError(false)
                              setShowCategoryPrefixError(false)
                            }}
                            edge="end"
                          >
                            <CloseRoundedIcon className="!text-black" />
                          </IconButton>
                        </Grid>
                      </Grid>
                      <Grid container id="create-category-name-error">
                        <Grid item xs={12}>
                          {showCategoryNameError && createCategoryError && (
                            <FormHelperText className="px-4 !text-red-500">
                              {getErrorMessage(createCategoryError, "Name")}
                            </FormHelperText>
                          )}
                        </Grid>
                      </Grid>
                      <Grid container id="create-category-prefix-error">
                        <Grid item xs={12}>
                          {showCategoryPrefixError && createCategoryError && (
                            <FormHelperText className="px-4 !text-red-500">
                              {getErrorMessage(createCategoryError, "Prefix")}
                            </FormHelperText>
                          )}
                        </Grid>
                      </Grid>
                    </MenuItem>
                  )}
                </form>
              </Select>
              <FormHelperText className="!text-red-500">
                {state.meta.errors}
              </FormHelperText>
            </>
          )}
        </AssetField>
      </FormControl>
      <FormControl>
        <FormLabel>Specification:</FormLabel>
        <AssetField name="specification">
          {({ handleChange, state }) => (
            <>
              <TextField
                id="txt-specification"
                value={state.value}
                color={state.meta.errors ? "error" : "primary"}
                focused={state.meta.errors.length != 0}
                onChange={(e) => handleChange(e.target.value)}
                size="small"
                inputProps={{ maxLength: 1000 }}
                multiline
                rows={4}
              />
              <FormHelperText className="!text-red-500">
                {state.meta.errors}
              </FormHelperText>
            </>
          )}
        </AssetField>
      </FormControl>
      <FormControl>
        <FormLabel>Installed Date:</FormLabel>
        <AssetField name="installDate">
          {({ handleChange, state }) => (
            <>
              <DatePicker
                name="installDate"
                minDate={today}
                value={state.value}
                onChange={(value) => {
                  value && handleChange(value)
                }}
                format="dd/MM/yyyy"
                slotProps={{
                  textField: {
                    size: "small",
                    id: "dpk-install-date",
                    disabled: true,
                    color: state.meta.errors.length != 0 ? "error" : "primary",
                    focused: state.meta.errors.length != 0,
                  },
                  openPickerButton: {
                    id: "btn-install-date",
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
        </AssetField>
      </FormControl>
      <FormControl>
        <FormLabel>State:</FormLabel>
        <AssetField name="state">
          {({ handleChange, state }) => (
            <>
              <RadioGroup
                aria-labelledby="demo-radio-buttons-group-label"
                name="state"
                id="rdo-state"
                onChange={(e) =>
                  handleChange(e.target.value as unknown as AssetState)
                }
                value={state.value}
              >
                {!isUpdating &&
                  createAssetStateOptions.map(
                    (item: OptionItem<AssetState>) => (
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
                    )
                  )}
                {isUpdating &&
                  updateAssetStateOptions.map(
                    (item: OptionItem<AssetState>) => (
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
                    )
                  )}
              </RadioGroup>
              <FormHelperText className="!text-red-500">
                {state.meta.errors}
              </FormHelperText>
            </>
          )}
        </AssetField>
      </FormControl>
      <Container className="mt-6 !flex flex-row-reverse gap-10">
        <Link to="/asset">
          <Button
            id="btn-edit-user-close"
            variant="outlined"
            className="!border-black !text-black"
          >
            Close
          </Button>
        </Link>
        <AssetSubscribe selector={(state) => [state.canSubmit, state.values]}>
          {([canSubmit, values]) => (
            <Button
              id="btn-edit-user-ok"
              type="submit"
              variant="contained"
              className="disabled:!hover:bg-red-800 !bg-red-500 !text-white disabled:!bg-red-700 disabled:!text-gray-300"
              disabled={
                !canSubmit ||
                !(values as CreateAssetRequest).name ||
                !(values as CreateAssetRequest).categoryId ||
                !(values as CreateAssetRequest).specification ||
                !(values as CreateAssetRequest).installDate ||
                compare(values, initialData)
              }
            >
              {createAssetIsPending || updateAssetIsPending ? (
                <CircularProgress size={25} />
              ) : (
                "Save"
              )}
            </Button>
          )}
        </AssetSubscribe>
      </Container>
    </form>
  )
}
