import { useContext, useEffect, useState } from "react"
import DataGrid from "@components/data/data-grid"
import FilterDate from "@components/fields/date-input"
import FilterInput from "@components/fields/filter-input"
import SearchInput from "@components/fields/search-input"
import AssignmentInfoModal from "@components/modals/assignment-info-modal"
import ConfirmModal from "@components/modals/confirm-modal"
import MessageModal from "@components/modals/message-modal"
import AssignmentColumns from "@components/tables/assignment/columns"
import { AssignmentRowAction } from "@components/tables/assignment/row-action"
import { State } from "@features/assignments/assignment.type"
import useDeleteAssignment from "@features/assignments/useDeleteAssignment"
import useListAssignments from "@features/assignments/useListAssignments"
import useRequestForReturningAssignment from "@features/assignments/useRequestForReturningAssignment"
import { DEFAULT_PAGE_INDEX, DEFAULT_PAGE_SIZE } from "@libs/constants/default"
import { BreadcrumbsContext } from "@libs/contexts/BreadcrumbsContext"
import { featuredAssignmentAtom } from "@libs/jotai/assignmentAtom"
import { Button } from "@mui/material"
import { useNavigate, useRouter, useSearch } from "@tanstack/react-router"
import { format } from "date-fns"
import { useAtom } from "jotai"
import { MRT_PaginationState, MRT_SortingState } from "material-react-table"

const breadcrumbItems = [
  {
    label: "Manage Assignment",
    to: "/assignment",
  },
]

const states = ["All", State.Accepted, State.WaitingForAcceptance]

export default function Assignments() {
  const navigate = useNavigate({ from: "/assignment" })
  const router = useRouter()
  const [featuredAssignmentId, setFeaturedAssignmentId] = useAtom(
    featuredAssignmentAtom
  )
  const context = useContext(BreadcrumbsContext)
  const [open, setOpen] = useState(false)
  const [notFoundMessage, setNotFoundMessage] = useState(false)
  const [selectedAssignmentId, setSelectedAssignmentId] = useState<string>("")
  const params = useSearch({
    strict: false,
  })

  const queryParameters = {
    pageIndex:
      (params as { pageIndex?: number }).pageIndex ?? DEFAULT_PAGE_INDEX,
    pageSize: DEFAULT_PAGE_SIZE,
    orderBy: (params as { orderBy?: string }).orderBy ?? "assetCode",
    isDescending: (params as { isDescending?: boolean }).isDescending ?? false,
    state:
      (params as { state?: State }).state === states[0]
        ? undefined
        : (params as { state?: State }).state,
    assignedDate: (params as { assignedDate?: Date }).assignedDate ?? undefined,
    search: (params as { search?: string }).search ?? undefined,
    featuredAssignmentId:
      featuredAssignmentId.length > 0 ? featuredAssignmentId : undefined,
  }

  const [sorting, setSorting] = useState<MRT_SortingState>([
    { id: queryParameters.orderBy, desc: queryParameters.isDescending },
  ])
  const [pagination, setPagination] = useState<MRT_PaginationState>({
    pageIndex: queryParameters.pageIndex - 1,
    pageSize: DEFAULT_PAGE_SIZE,
  })
  const [selectedState, setSelectedState] = useState<string | string[]>(
    queryParameters.state ?? ""
  )
  const [selectedDate, setSelectedDate] = useState<Date>()
  const [keyword, setKeyword] = useState<string>(queryParameters.search ?? "")
  const [openDisableConfirmMod, setOpenDisableConfirmMod] = useState(false)
  const [openReturnConfirmMod, setOpenReturnConfirmMod] = useState(false)

  const { data, isLoading: listLoading } = useListAssignments(queryParameters)

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

  const resetPagination = () => {
    setPagination({
      pageIndex: DEFAULT_PAGE_INDEX - 1,
      pageSize: DEFAULT_PAGE_SIZE,
    })
  }

  const searchOnClick = () => {
    setFeaturedAssignmentId("")
    resetPagination()
  }

  const {
    mutate: deleteAssignment,
    isSuccess: deleteAssignmentSuccess,
    isError: deleteAssignmentFail,
  } = useDeleteAssignment()
  const { mutate: returnAssignment, isSuccess: returnAssignmentSuccess } =
    useRequestForReturningAssignment()

  const handleAssignmentAction = (id: string) => {
    deleteAssignment(id)
    setOpenDisableConfirmMod(false)
  }

  const handleReturningAssignmentAction = (id: string) => {
    returnAssignment(id)
    setOpenReturnConfirmMod(false)
  }

  useEffect(() => {
    sorting[0] = {
      id: queryParameters.orderBy,
      desc: queryParameters.isDescending,
    }
    pagination.pageIndex = queryParameters.pageIndex - 1
    if (queryParameters.state === undefined && selectedState !== "All") {
      setSelectedState("All")
      setKeyword("")
    }
  }, [params])

  useEffect(() => {
    if (deleteAssignmentFail) {
      setNotFoundMessage(true)
    }
  }, [deleteAssignmentFail])

  useEffect(() => {
    ;(async () => {
      await router.navigate({
        search: {
          ...queryParameters,
          pageIndex: pagination.pageIndex + 1,
          pageSize: pagination.pageSize,
          orderBy: sorting[0]?.id,
          isDescending: sorting[0]?.desc,
          state: selectedState !== "" ? selectedState : undefined,
          assignedDate: selectedDate
            ? format(selectedDate, "yyyy-MM-dd")
            : undefined,
          search: keyword !== "" ? keyword : undefined,
        },
      })
    })()
  }, [pagination, sorting])

  useEffect(() => {
    if (selectedState !== "") {
      resetPagination()
    }
  }, [selectedState])

  useEffect(() => {
    if (selectedDate) {
      resetPagination()
    }
  }, [selectedDate])

  useEffect(() => {
    context?.setBreadcrumbs(breadcrumbItems)
  }, [])

  useEffect(() => {
    if (
      (deleteAssignmentSuccess || returnAssignmentSuccess) &&
      selectedAssignmentId == featuredAssignmentId
    ) {
      setFeaturedAssignmentId("")
    }
  }, [deleteAssignmentSuccess, returnAssignmentSuccess])

  return (
    <>
      <DataGrid
        title="Assignment List"
        filterComponents={[
          {
            id: "filterStateInput",
            component: (
              <FilterInput
                values={states}
                label="State"
                multiple={false}
                selected={selectedState}
                setSelected={(value) => {
                  setFeaturedAssignmentId("")
                  setSelectedState(value as string)
                }}
              />
            ),
          },
          {
            id: "filterDateInput",
            component: (
              <FilterDate label="Assigned Date" setSelected={setSelectedDate} />
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
            id: "createNewAssignment",
            component: (
              <Button
                variant="contained"
                color="error"
                sx={{
                  fontSize: "0.75rem",
                  padding: "4px 8px",
                  minWidth: "auto",
                }}
                onClick={() => navigate({ to: "/assignment/new" })}
              >
                Create Assignment
              </Button>
            ),
          },
        ]}
        tableProps={{
          tableOptionsProps: {
            columns: AssignmentColumns(),
            data: [...(data?.assignments || [])],
            isLoading: listLoading,
            pageCount: data?.pagedInfo.totalPages ?? 0,
            setOpen: setOpen,
            setSelectedEntityId: setSelectedAssignmentId,
            pagination: pagination,
            setPagination: setPagination,
            sorting: sorting,
            setSorting: setSorting,
            renderRowActions: (assignment) => (
              <AssignmentRowAction
                key={assignment.id}
                data={assignment}
                setOpenDisableConfirmMod={setOpenDisableConfirmMod}
                setOpenReturnConfirmMod={setOpenReturnConfirmMod}
                id={setSelectedAssignmentId}
              />
            ),
          },
        }}
      />
      {selectedAssignmentId && (
        <AssignmentInfoModal
          open={open}
          onClose={() => setOpen(false)}
          id={selectedAssignmentId}
          title={"Detailed Assignment Information"}
        />
      )}
      <ConfirmModal
        open={openDisableConfirmMod}
        message="Do you want to delete this assignment?"
        title="Are you sure?"
        buttonOkLabel="Delete"
        buttonCloseLabel="Cancel"
        onOk={() => handleAssignmentAction(selectedAssignmentId)}
        onClose={() => setOpenDisableConfirmMod(false)}
      />
      <ConfirmModal
        open={openReturnConfirmMod}
        message="Do you want to create returning request for this asset?"
        title="Are you sure?"
        buttonOkLabel="Yes"
        buttonCloseLabel="No"
        onOk={() => handleReturningAssignmentAction(selectedAssignmentId)}
        onClose={() => setOpenReturnConfirmMod(false)}
      />
      <MessageModal
        title="Error"
        message="That assignment has been removed by another admin"
        open={notFoundMessage}
        onClose={() => setNotFoundMessage(false)}
      />
    </>
  )
}
