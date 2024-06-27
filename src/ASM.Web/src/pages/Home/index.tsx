import { useContext, useEffect, useState } from "react"
import DataGrid from "@components/data/data-grid"
import OwnAssignmentModal from "@components/modals/own-assignment-modal"
import OwnAssignmentColumns from "@components/tables/own-assignment/columns"
import { OwnAssignmentRowAction } from "@components/tables/own-assignment/row-action"
import useListOwnAssignments from "@features/assignments/useListOwnAssignments"
import { DEFAULT_PAGE_SIZE } from "@libs/constants/default"
import { BreadcrumbsContext } from "@libs/contexts/BreadcrumbsContext"
import { useRouter, useSearch } from "@tanstack/react-router"
import { MRT_PaginationState, MRT_SortingState } from "material-react-table"

import { RouteItem } from "@/types/data"

const breadcrumb: RouteItem[] = [
  {
    label: "Home",
    to: "/home",
  },
]

export default function Home() {
  const context = useContext(BreadcrumbsContext)
  const router = useRouter()
  const [open, setOpen] = useState(false)
  const [selectedAssignmentId, setSelectedAssignmentId] = useState<string>("")
  const params = useSearch({
    strict: false,
  })

  const queryParameters = {
    orderBy: (params as { orderBy?: string }).orderBy ?? "AssetCode",
    isDescending: (params as { isDescending?: boolean }).isDescending ?? false,
  }

  useEffect(() => {
    context?.setBreadcrumbs(breadcrumb)
  }, [])

  const [sorting, setSorting] = useState<MRT_SortingState>([
    { id: queryParameters.orderBy, desc: queryParameters.isDescending },
  ])
  const [pagination] = useState<MRT_PaginationState>({
    pageIndex: 1,
    pageSize: DEFAULT_PAGE_SIZE,
  })
  const { data, isLoading: listLoading } =
    useListOwnAssignments(queryParameters)

  useEffect(() => {
    // Update sorting and pagination based on query parameters
    // when navigating back to this page, to match previous state
    sorting[0] = {
      id: queryParameters.orderBy,
      desc: queryParameters.isDescending,
    }
  }, [params])

  useEffect(() => {
    ;(async () =>
      await router.navigate({
        search: {
          ...queryParameters,
          orderBy: sorting[0]?.id,
          isDescending: sorting[0]?.desc,
        },
      }))()
  }, [sorting])

  return (
    <>
      <DataGrid
        title="My Assignments"
        tableProps={{
          tableOptionsProps: {
            columns: OwnAssignmentColumns(),
            data: data || [],
            isLoading: listLoading,
            setOpen: setOpen,
            setSelectedEntityId: setSelectedAssignmentId,
            sorting: sorting,
            disablePagination: true,
            setSorting: setSorting,
            actionState: 120,
            renderRowActions: (assignment) => (
              <OwnAssignmentRowAction key={assignment.id} data={assignment} />
            ),
            pageCount: 0,
            pagination: pagination,
            setPagination: function (): void {
              throw new Error("Function not implemented.")
            },
          },
        }}
      />
      {selectedAssignmentId && (
        <OwnAssignmentModal
          open={open}
          onClose={() => setOpen(false)}
          id={selectedAssignmentId}
          title={"Detailed Assignment Information"}
        />
      )}
    </>
  )
}
