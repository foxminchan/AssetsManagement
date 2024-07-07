import { useContext, useEffect, useState } from "react"
import DataGrid from "@components/data/data-grid"
import FilterInput from "@components/fields/filter-input"
import SearchInput from "@components/fields/search-input"
import AssetInfoModal from "@components/modals/asset-info-modal"
import ConfirmModal from "@components/modals/confirm-modal"
import MessageModal from "@components/modals/message-modal"
import AssetColumns from "@components/tables/asset/columns"
import { AssetRowAction } from "@components/tables/asset/row-action"
import { AssetState } from "@features/assets/asset.type"
import useDeleteAsset from "@features/assets/useDeleteAsset"
import useListAssets from "@features/assets/useListAssets"
import { Category } from "@features/categories/categories.type"
import useListCategories from "@features/categories/useListCategories"
import { DEFAULT_PAGE_INDEX, DEFAULT_PAGE_SIZE } from "@libs/constants/default"
import { BreadcrumbsContext } from "@libs/contexts/BreadcrumbsContext"
import { featuredAssetAtom } from "@libs/jotai/assetAtom"
import { Typography } from "@mui/material"
import Button from "@mui/material/Button"
import { Link, useNavigate, useRouter, useSearch } from "@tanstack/react-router"
import { useAtom } from "jotai"
import { MRT_PaginationState, MRT_SortingState } from "material-react-table"

const breadcrumbItems = [
  {
    label: "Manage Asset",
    to: "/asset",
  },
]
const states = [
  "All",
  AssetState.Assigned,
  AssetState.Available,
  AssetState.NotAvailable,
  AssetState.WaitingForRecycling,
  AssetState.Recycled,
]

export default function Assets() {
  const navigate = useNavigate({ from: "/asset" })
  const [featuredAssetId, setFeaturedAssetId] = useAtom(featuredAssetAtom)
  const router = useRouter()
  const context = useContext(BreadcrumbsContext)
  const [open, setOpen] = useState(false)
  const [selectedAssetId, setSelectedAssetId] = useState<string>("")
  const params = useSearch({
    strict: false,
  })
  const { data: listCategories } = useListCategories()
  const categories = [
    "All",
    ...(listCategories
      ? listCategories.categories.map((item: Category) => item.name)
      : []),
  ]

  const queryParameters = {
    pageIndex:
      (params as { pageIndex?: number }).pageIndex ?? DEFAULT_PAGE_INDEX,
    pageSize: DEFAULT_PAGE_SIZE,
    orderBy: (params as { orderBy?: string }).orderBy ?? "staffCode",
    isDescending: (params as { isDescending?: boolean }).isDescending ?? false,
    state:
      (params as { state?: AssetState }).state === states[0]
        ? undefined
        : (params as { state?: AssetState[] }).state,
    categories:
      (params as { categories?: string }).categories === categories[0]
        ? undefined
        : (params as { categories?: string[] }).categories,
    search: (params as { search?: string }).search ?? undefined,
    featuredAssetId: featuredAssetId.length > 0 ? featuredAssetId : undefined,
  }

  const [sorting, setSorting] = useState<MRT_SortingState>([
    { id: queryParameters.orderBy, desc: queryParameters.isDescending },
  ])
  const [pagination, setPagination] = useState<MRT_PaginationState>({
    pageIndex: queryParameters.pageIndex - 1,
    pageSize: DEFAULT_PAGE_SIZE,
  })
  const [selectedState, setSelectedState] = useState<string[]>(
    queryParameters.state
      ? [...queryParameters.state]
      : [AssetState.Assigned, AssetState.Available, AssetState.NotAvailable]
  )

  const [selectedCategories, setSelectedCategories] = useState<string[]>(
    queryParameters.categories ? [...queryParameters.categories] : ["All"]
  )
  const [keyword, setKeyword] = useState<string>(queryParameters.search ?? "")

  const { data, isLoading: listLoading } = useListAssets(queryParameters)

  if (
    data &&
    data.pagedInfo &&
    data.pagedInfo.totalPages <= pagination.pageIndex &&
    data.pagedInfo.totalPages !== 0
  ) {
    setPagination({
      pageIndex: DEFAULT_PAGE_INDEX - 1,
      pageSize: DEFAULT_PAGE_SIZE,
    })
  }

  const [deleteModalOpen, setDeleteModalOpen] = useState<boolean>(false)
  const [deleteErrorModalOpen, setDeleteErrorModalOpen] =
    useState<boolean>(false)

  const resetPagination = () => {
    setPagination({
      pageIndex: DEFAULT_PAGE_INDEX - 1,
      pageSize: DEFAULT_PAGE_SIZE,
    })
  }

  const searchOnClick = () => {
    setFeaturedAssetId("")
    resetPagination()
  }

  useEffect(() => {
    // Update sorting and pagination based on query parameters
    // when navigating back to this page, to match previous state
    sorting[0] = {
      id: queryParameters.orderBy,
      desc: queryParameters.isDescending,
    }
    pagination.pageIndex = queryParameters.pageIndex - 1

    if (queryParameters.state === undefined && selectedState[0] !== "All") {
      setSelectedState([
        AssetState.Assigned,
        AssetState.Available,
        AssetState.NotAvailable,
      ])
      setSelectedCategories(["All"])
      setKeyword("")
    }
  }, [params])

  useEffect(() => {
    ;(async () =>
      await router.navigate({
        search: {
          ...queryParameters,
          pageIndex: pagination.pageIndex + 1,
          pageSize: pagination.pageSize,
          orderBy: sorting[0]?.id,
          isDescending: sorting[0]?.desc,
          state:
            selectedState.length !== 0 && selectedState[0] !== "All"
              ? selectedState
              : undefined,
          categories:
            selectedCategories.length !== 0 && selectedCategories[0] !== "All"
              ? selectedCategories
              : undefined,
          search: keyword !== "" ? keyword : undefined,
        },
      }))()
  }, [pagination, sorting, selectedCategories, selectedState])

  useEffect(() => {
    if (selectedState.length !== 0) {
      resetPagination()
    }
  }, [selectedState])

  useEffect(() => {
    context?.setBreadcrumbs(breadcrumbItems)
  }, [])

  const handleModalOpen = (id: string) => {
    setSelectedAssetId(id)
    setDeleteModalOpen(true)
  }

  const {
    mutate: deleteAsset,
    isSuccess: isDeleteAssetSuccess,
    isError: isDeleteAssetError,
  } = useDeleteAsset()

  useEffect(() => {
    if (isDeleteAssetSuccess && selectedAssetId == featuredAssetId) {
      setFeaturedAssetId("")
    } else if (isDeleteAssetError) setDeleteErrorModalOpen(true)
    setDeleteModalOpen(false)
  }, [isDeleteAssetSuccess, isDeleteAssetError])

  return (
    <>
      <DataGrid
        title="Asset List"
        filterComponents={[
          {
            id: "filterStateInput",
            component: (
              <FilterInput
                values={states}
                label="State"
                multiple={true}
                selected={selectedState}
                setSelected={(value) => {
                  setFeaturedAssetId("")
                  setSelectedState(value as string[])
                }}
              />
            ),
          },
          {
            id: "filterCategoryInput",
            component: (
              <FilterInput
                values={categories}
                label="Category"
                multiple={true}
                selected={selectedCategories}
                setSelected={(value) => {
                  setFeaturedAssetId("")
                  setSelectedCategories(value as string[])
                }}
              />
            ),
          },
        ]}
        searchComponents={[
          {
            id: "search",
            component: (
              <SearchInput
                keyword={keyword}
                setKeyword={setKeyword}
                onClick={searchOnClick}
              />
            ),
          },
        ]}
        buttonComponents={[
          {
            id: "createNewAsset",
            component: (
              <Button
                variant="contained"
                color="error"
                onClick={() => navigate({ to: "/asset/new" })}
                fullWidth
              >
                Create Asset
              </Button>
            ),
          },
        ]}
        tableProps={{
          tableOptionsProps: {
            columns: AssetColumns(),
            data: [...(data?.assets || [])],
            isLoading: listLoading,
            pageCount: data?.pagedInfo.totalPages ?? 0,
            setOpen: setOpen,
            setSelectedEntityId: setSelectedAssetId,
            pagination: pagination,
            setPagination: setPagination,
            sorting: sorting,
            setSorting: setSorting,
            renderRowActions: (user) => (
              <AssetRowAction
                key={user.id}
                data={user}
                setOpen={handleModalOpen}
              />
            ),
          },
        }}
      />
      {selectedAssetId && (
        <AssetInfoModal
          open={open}
          onClose={() => setOpen(false)}
          id={selectedAssetId}
          title={"Detailed Asset Information"}
        />
      )}
      <ConfirmModal
        open={deleteModalOpen}
        message="Do you want to delete this asset?"
        title="Are you sure?"
        buttonOkLabel="Delete"
        buttonCloseLabel="Cancel"
        onOk={() => deleteAsset(selectedAssetId)}
        onClose={() => setDeleteModalOpen(false)}
      />
      <MessageModal
        message=""
        title="Cannot Delete Asset"
        open={deleteErrorModalOpen}
        onClose={() => setDeleteErrorModalOpen(false)}
      >
        <Typography>
          Cannot delete the asset because it belongs to one or more historical
          assignments.
        </Typography>
        <Typography>
          If the asset is not able to be used anymore, please update its state
          in{" "}
          <Link
            className="!text-blue-600 !underline"
            to={`/asset/${selectedAssetId}`}
          >
            Edit Asset page
          </Link>
        </Typography>
      </MessageModal>
    </>
  )
}
