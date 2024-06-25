import { useContext, useEffect, useState } from "react"
import { userAtoms } from "@/libs/jotai/userAtoms"
import FilterInput from "@components/fields/filter-input"
import SearchInput from "@components/fields/search-input"
import UserInfoModal from "@components/modals/user-info-modal"
import UserTable from "@components/tables/user/table"
import useGetUser from "@features/users/useGetUser"
import useListUsers from "@features/users/useListUsers"
import { RoleType } from "@features/users/user.type"
import { DEFAULT_PAGE_INDEX, DEFAULT_PAGE_SIZE } from "@libs/constants/default"
import { BreadcrumbsContext } from "@libs/contexts/BreadcrumbsContext"
import { Button, Grid, Typography } from "@mui/material"
import { useNavigate, useSearch } from "@tanstack/react-router"
import { useAtomValue } from "jotai"

const breadcrumbItems = [
  {
    label: "Manage User",
    to: "/user",
  },
]

const types = ["All", RoleType.Admin, RoleType.Staff]

export default function Users() {
  const context = useContext(BreadcrumbsContext)
  const [open, setOpen] = useState(false)
  const [selectedUserId, setSelectedUserId] = useState<string>("")
  const params = useSearch({
    strict: false,
  })
  const navigate = useNavigate({ from: "/user" })

  const queryParameters = {
    pageIndex:
      (params as { pageIndex?: number }).pageIndex ?? DEFAULT_PAGE_INDEX,
    pageSize: DEFAULT_PAGE_SIZE,
    orderBy: (params as { orderBy?: string }).orderBy ?? "staffCode",
    isDescending: (params as { isDescending?: boolean }).isDescending ?? false,
    RoleType:
      (params as { roleType?: string }).roleType === types[0]
        ? undefined
        : (params as { roleType?: string }).roleType,
    search: (params as { search?: string }).search ?? undefined,
  }

  const { data, isLoading: listLoading } = useListUsers(queryParameters)
  const [selectedType, setSelectedType] = useState<string | string[]>("")

  const userId = useAtomValue(userAtoms)
  const { data: user, isLoading: userLoading } =
    userId !== "" ? useGetUser(userId) : { data: undefined, isLoading: false }

  useEffect(() => {
    context?.setBreadcrumbs(breadcrumbItems)
  }, [])

  return (
    <>
      <Typography
        variant="h5"
        component="h1"
        gutterBottom
        fontWeight={500}
        className="!text-red-500"
      >
        User List
      </Typography>
      <Grid container rowSpacing={2} className="!mb-4">
        <Grid item xs={3}>
          <FilterInput
            values={types}
            label="Type"
            isMultiple={false}
            selectedType={selectedType}
            setSelectedType={setSelectedType}
          />
        </Grid>
        <Grid item xs={7} className="!flex !flex-row-reverse">
          <SearchInput />
        </Grid>
        <Grid item xs className="!flex !justify-end">
          <Button
            id="btn-create-user"
            variant="contained"
            color="error"
            onClick={() => navigate({ to: "/user/new" })}
          >
            Create User
          </Button>
        </Grid>
      </Grid>
      <UserTable
        data={
          user && queryParameters.pageIndex === 1
            ? [
                user,
                ...(data?.users.filter((x) => x.id !== user.id).slice(0, -1) ||
                  []),
              ]
            : [...(data?.users || [])]
        }
        isLoading={listLoading && userLoading}
        pageCount={data?.pagedInfo.totalPages ?? 0}
        setSelectedUserId={setSelectedUserId}
        setOpen={setOpen}
      />
      {selectedUserId && (
        <UserInfoModal
          open={open}
          onClose={() => setOpen(false)}
          id={selectedUserId}
        />
      )}
    </>
  )
}
