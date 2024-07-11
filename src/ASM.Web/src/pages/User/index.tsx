import { useContext, useEffect, useState } from "react"
import DataGrid from "@components/data/data-grid"
import FilterInput from "@components/fields/filter-input"
import SearchInput from "@components/fields/search-input"
import ConfirmModal from "@components/modals/confirm-modal"
import MessageModal from "@components/modals/message-modal"
import UserInfoModal from "@components/modals/user-info-modal"
import UserColumns from "@components/tables/user/columns"
import { UserRowAction } from "@components/tables/user/row-action"
import useListAssignments from "@features/assignments/useListAssignments"
import useDeleteUser from "@features/users/useDeleteUser"
import useListUsers from "@features/users/useListUsers"
import { RoleType } from "@features/users/user.type"
import { DEFAULT_PAGE_INDEX, DEFAULT_PAGE_SIZE } from "@libs/constants/default"
import { BreadcrumbsContext } from "@libs/contexts/BreadcrumbsContext"
import { featuredUserAtom } from "@libs/jotai/userAtom"
import { Button } from "@mui/material"
import { useNavigate, useRouter, useSearch } from "@tanstack/react-router"
import { useAtom } from "jotai"
import { MRT_PaginationState, MRT_SortingState } from "material-react-table"

const breadcrumbItems = [
  {
    label: "Manage User",
    to: "/user",
  },
]

const states = ["All", RoleType.Admin, RoleType.Staff]

export default function Users() {
  const navigate = useNavigate({ from: "/user" })
  const [featuredUserId, setFeaturedUserId] = useAtom(featuredUserAtom)
  const router = useRouter()
  const context = useContext(BreadcrumbsContext)
  const [open, setOpen] = useState(false)
  const [deleteModalOpen, setDeleteModalOpen] = useState<boolean>(false)
  const [undeletedUserWarningModalOpen, setUndeletedUserWarningModalOpen] =
    useState<boolean>(false)

  const [deleteUserWarningMessage, SetDeleteUserWarningMessage] =
    useState(false)
  const [selectedUserId, setSelectedUserId] = useState<string>("")
  const params = useSearch({
    strict: false,
  })

  const {
    mutate: deleteUser,
    isSuccess: isDeleteUserSuccess,
    isError: isDeleteUserError,
  } = useDeleteUser()
  const handleModalOpen = (id: string) => {
    if (
      assignmentData?.assignments.find((x) => x.userId === id) === undefined
    ) {
      setSelectedUserId(id)
      setDeleteModalOpen(true)
    } else {
      setUndeletedUserWarningModalOpen(true)
    }
  }

  useEffect(() => {
    if (isDeleteUserSuccess) {
      refetch()
      setDeleteModalOpen(false)
    }
  }, [isDeleteUserSuccess])

  useEffect(() => {
    if (isDeleteUserError) {
      SetDeleteUserWarningMessage(true)
    }
  }, [isDeleteUserError])

  const queryParameters = {
    pageIndex:
      (params as { pageIndex?: number }).pageIndex ?? DEFAULT_PAGE_INDEX,
    pageSize: DEFAULT_PAGE_SIZE,
    orderBy: (params as { orderBy?: string }).orderBy ?? "StaffCode",
    isDescending: (params as { isDescending?: boolean }).isDescending ?? false,
    roleType:
      (params as { roleType?: RoleType }).roleType === states[0]
        ? undefined
        : (params as { roleType?: RoleType }).roleType,
    search: (params as { search?: string }).search ?? undefined,
    featuredStaffId: featuredUserId.length > 0 ? featuredUserId : undefined,
  }

  const [sorting, setSorting] = useState<MRT_SortingState>([
    { id: queryParameters.orderBy, desc: queryParameters.isDescending },
  ])
  const [pagination, setPagination] = useState<MRT_PaginationState>({
    pageIndex: queryParameters.pageIndex - 1,
    pageSize: DEFAULT_PAGE_SIZE,
  })
  const [selectedType, setSelectedType] = useState<string | string[]>(
    queryParameters.roleType ?? "All"
  )
  const [keyword, setKeyword] = useState<string>(queryParameters.search ?? "")

  const {
    data,
    isLoading: listLoading,
    refetch,
  } = useListUsers(queryParameters)

  const { data: assignmentData } = useListAssignments()
  if (
    data &&
    data.pagedInfo &&
    data.pagedInfo.totalPages <= pagination.pageIndex &&
    data.pagedInfo.totalPages != 0
  ) {
    setPagination({
      pageIndex: DEFAULT_PAGE_INDEX - 1,
      pageSize: DEFAULT_PAGE_SIZE,
    })
  }

  const handleDisableUser = (id: string) =>
    Promise.resolve(deleteUser(id)).then(() => (window.location.href = "/user"))

  const resetPagination = () => {
    setPagination({
      pageIndex: DEFAULT_PAGE_INDEX - 1,
      pageSize: DEFAULT_PAGE_SIZE,
    })
  }

  const searchOnClick = () => {
    setFeaturedUserId("")
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

    if (
      queryParameters.roleType === undefined &&
      selectedType !== "" &&
      queryParameters.search === null
    ) {
      setSelectedType("All")
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
                values={states}
                label="Type"
                multiple={false}
                selected={selectedType}
                setSelected={(value) => {
                  setFeaturedUserId("")
                  setSelectedType(value as string)
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
            data: [...(data?.users || [])],
            isLoading: listLoading,
            pageCount: data?.pagedInfo.totalPages ?? 0,
            setOpen: setOpen,
            setSelectedEntityId: setSelectedUserId,
            pagination: pagination,
            setPagination: setPagination,
            sorting: sorting,
            setSorting: setSorting,
            renderRowActions: (user) => (
              <UserRowAction
                key={user.id}
                data={user}
                setOpen={handleModalOpen}
              />
            ),
          },
        }}
      />
      <ConfirmModal
        open={deleteModalOpen}
        message="Do you want to disable this user?"
        title="Are you sure?"
        buttonOkLabel="Disable"
        buttonCloseLabel="Cancel"
        onOk={() => handleDisableUser(selectedUserId)}
        onClose={() => setDeleteModalOpen(false)}
      />

      <MessageModal
        message="There are valid assignments belonging to this user. Please close all assignments before disabling user."
        title="Can not disable user"
        open={undeletedUserWarningModalOpen}
        onClose={() => setUndeletedUserWarningModalOpen(false)}
      />
      <MessageModal
        message="User has been deleted by another admin"
        title="Already Deleted User"
        open={deleteUserWarningMessage}
        onClose={() => SetDeleteUserWarningMessage(false)}
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
