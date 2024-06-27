import { useContext, useEffect, useState } from "react"
import DataGrid from "@components/data/data-grid"
import FilterInput from "@components/fields/filter-input"
import SearchInput from "@components/fields/search-input"
import UserInfoModal from "@components/modals/user-info-modal"
import UserColumns from "@components/tables/user/columns"
import { UserRowAction } from "@components/tables/user/row-action"
import useGetUser from "@features/users/useGetUser"
import useListUsers from "@features/users/useListUsers"
import { RoleType } from "@features/users/user.type"
import { DEFAULT_PAGE_INDEX, DEFAULT_PAGE_SIZE } from "@libs/constants/default"
import { BreadcrumbsContext } from "@libs/contexts/BreadcrumbsContext"
import { userAtoms } from "@libs/jotai/userAtoms"
import { Button } from "@mui/material"
import { useNavigate, useRouter, useSearch } from "@tanstack/react-router"
import { useAtomValue } from "jotai"
import { MRT_PaginationState, MRT_SortingState } from "material-react-table"

const breadcrumbItems = [
  {
    label: "Manage User",
    to: "/user",
  },
]

const types = ["All", RoleType.Admin, RoleType.Staff]

export default function Users() {
  const navigate = useNavigate({ from: "/user" })
  const router = useRouter()
  const context = useContext(BreadcrumbsContext)
  const [open, setOpen] = useState(false)
  const [selectedUserId, setSelectedUserId] = useState<string>("")
  const params = useSearch({
    strict: false,
  })

  const queryParameters = {
    pageIndex:
      (params as { pageIndex?: number }).pageIndex ?? DEFAULT_PAGE_INDEX,
    pageSize: DEFAULT_PAGE_SIZE,
    orderBy: (params as { orderBy?: string }).orderBy ?? "staffCode",
    isDescending: (params as { isDescending?: boolean }).isDescending ?? false,
    roleType:
      (params as { roleType?: RoleType }).roleType === types[0]
        ? undefined
        : (params as { roleType?: RoleType }).roleType,
    search: (params as { search?: string }).search ?? undefined,
  }

  const [sorting, setSorting] = useState<MRT_SortingState>([
    { id: queryParameters.orderBy, desc: queryParameters.isDescending },
  ])
  const [pagination, setPagination] = useState<MRT_PaginationState>({
    pageIndex: queryParameters.pageIndex - 1,
    pageSize: DEFAULT_PAGE_SIZE,
  })
  const [selectedType, setSelectedType] = useState<string | string[]>(
    queryParameters.roleType ?? ""
  )
  const [keyword, setKeyword] = useState<string>(queryParameters.search ?? "")

  const { data, isLoading: listLoading } = useListUsers(queryParameters)

  if (
    data &&
    data.pagedInfo &&
    data.pagedInfo.totalPages <= pagination.pageIndex
  ) {
    setPagination({
      pageIndex: DEFAULT_PAGE_INDEX - 1,
      pageSize: DEFAULT_PAGE_SIZE,
    })
  }

  const userId = useAtomValue(userAtoms)
  const { data: user, isLoading: userLoading } = useGetUser(userId)

  const resetPagination = () => {
    setPagination({
      pageIndex: DEFAULT_PAGE_INDEX - 1,
      pageSize: DEFAULT_PAGE_SIZE,
    })
  }

  const searchOnClick = () => {
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
          roleType: selectedType !== "" ? selectedType : undefined,
          search: keyword !== "" ? keyword : undefined,
        },
      }))()
  }, [pagination, sorting])

  useEffect(() => {
    if (selectedType !== "") {
      resetPagination()
    }
  }, [selectedType])

  useEffect(() => {
    context?.setBreadcrumbs(breadcrumbItems)
  }, [])

  return (
    <>
      <DataGrid
        title="User List"
        filterComponents={[
          {
            id: "filterInput",
            component: (
              <FilterInput
                values={types}
                label="Type"
                multiple={false}
                selected={selectedType}
                setSelected={setSelectedType}
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
            id: "createNewUser",
            component: (
              <Button
                variant="contained"
                color="error"
                onClick={() => navigate({ to: "/user/new" })}
              >
                Create User
              </Button>
            ),
          },
        ]}
        tableProps={{
          tableOptionsProps: {
            columns: UserColumns(),
            data:
              user && queryParameters.pageIndex === 1
                ? [user, ...(data?.users.filter((x) => x.id !== user.id) || [])]
                : [...(data?.users || [])],
            isLoading: listLoading || userLoading,
            pageCount: data?.pagedInfo.totalPages ?? 0,
            setOpen: setOpen,
            setSelectedEntityId: setSelectedUserId,
            pagination: pagination,
            setPagination: setPagination,
            sorting: sorting,
            setSorting: setSorting,
            renderRowActions: (user) => (
              <UserRowAction key={user.id} data={user} />
            ),
          },
        }}
      />
      {selectedUserId && (
        <UserInfoModal
          open={open}
          onClose={() => setOpen(false)}
          id={selectedUserId}
          title={"Detailed User Information"}
        />
      )}
    </>
  )
}
