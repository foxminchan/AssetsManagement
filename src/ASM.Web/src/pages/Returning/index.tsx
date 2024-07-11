import { useContext, useEffect, useState } from "react"
import DataGrid from "@components/data/data-grid"
import FilterDate from "@components/fields/date-input"
import FilterInput from "@components/fields/filter-input"
import SearchInput from "@components/fields/search-input"
import ConfirmModal from "@components/modals/confirm-modal"
import MessageModal from "@components/modals/message-modal"
import ReturningRequestColumns from "@components/tables/returning-request/columns"
import { ReturningRequestRowAction } from "@components/tables/returning-request/row-action"
import { ReturningRequestState } from "@features/returning-requests/returning-request.type"
import useCancelRequest from "@features/returning-requests/useCancelRequest"
import useCompleteRequest from "@features/returning-requests/useCompleteRequest"
import useListReturningRequests from "@features/returning-requests/useListReturningRequests"
import { DEFAULT_PAGE_INDEX, DEFAULT_PAGE_SIZE } from "@libs/constants/default"
import { BreadcrumbsContext } from "@libs/contexts/BreadcrumbsContext"
import { useRouter, useSearch } from "@tanstack/react-router"
import { format } from "date-fns"
import { MRT_PaginationState, MRT_SortingState } from "material-react-table"
import { match } from "ts-pattern"

const breadcrumbItems = [
  {
    label: "Request for Returning",
    to: "/returning",
  },
]
const states = ["All", ReturningRequestState.Completed, "Waiting for returning"]
const transformState = (value: string | string[]): string | undefined => {
  return match(value)
    .with(ReturningRequestState.Completed, (v) => v)
    .with(
      "Waiting for returning",
      () => ReturningRequestState.WaitingForReturning
    )
    .otherwise(() => undefined)
}

export default function ReturningRequests() {
  const [notFoundMessage, setNotFoundMessage] = useState(false)
  const router = useRouter()
  const context = useContext(BreadcrumbsContext)
  const [selectedReturningRequestId, setSelectedReturningRequestId] =
    useState<string>("")

  const {
    mutate: completeReturn,
    isSuccess: completeReturnSuccess,
    isError: completeReturnError,
  } = useCompleteRequest()
  const {
    mutate: cancelReturn,
    isSuccess: cancelReturnSuccess,
    isError: cancelReturnError,
  } = useCancelRequest()
  const params = useSearch({
    strict: false,
  })

  useEffect(() => {
    if (completeReturnError || cancelReturnError) {
      setNotFoundMessage(true)
    }
  }, [completeReturnError, cancelReturnError])

  const handleCompleteReturnAction = () => {
    completeReturn(selectedReturningRequestId)
    setCompleteRequestModalOpen(false)
  }

  const handleCancelReturnAction = () => {
    cancelReturn(selectedReturningRequestId)
    setCancelRequestModalOpen(false)
  }

  useEffect(() => {
    if (completeReturnSuccess) {
      refetch()
    }
  }, [completeReturnSuccess])

  useEffect(() => {
    if (cancelReturnSuccess) {
      refetch()
    }
  }, [cancelReturnSuccess])

  const queryParameters = {
    pageIndex:
      (params as { pageIndex?: number }).pageIndex ?? DEFAULT_PAGE_INDEX,
    pageSize: DEFAULT_PAGE_SIZE,
    orderBy: (params as { orderBy?: string }).orderBy ?? "AssetCode",
    isDescending: (params as { isDescending?: boolean }).isDescending ?? false,
    state:
      (params as { state?: ReturningRequestState }).state === states[0]
        ? undefined
        : (params as { state?: ReturningRequestState }).state,
    returnedDate: (params as { returnedDate?: Date }).returnedDate ?? undefined,
    search: (params as { search?: string }).search ?? undefined,
  }

  const [sorting, setSorting] = useState<MRT_SortingState>([
    { id: queryParameters.orderBy, desc: queryParameters.isDescending },
  ])
  const [pagination, setPagination] = useState<MRT_PaginationState>({
    pageIndex: queryParameters.pageIndex - 1,
    pageSize: DEFAULT_PAGE_SIZE,
  })
  const [selectedState, setSelectedState] = useState<string | string[]>(
    queryParameters.state ?? "All"
  )
  const [selectedDate, setSelectedDate] = useState<Date>()
  const [keyword, setKeyword] = useState<string>(queryParameters.search ?? "")

  const {
    data,
    isLoading: listLoading,
    refetch,
  } = useListReturningRequests(queryParameters)

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

  const [cancelRequestModalOpen, setCancelRequestModalOpen] =
    useState<boolean>(false)
  const [completeRequestModalOpen, setCompleteRequestModalOpen] =
    useState<boolean>(false)

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

    if (
      queryParameters.state === undefined &&
      queryParameters.search === null
    ) {
      setSelectedState("All")
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
            selectedState !== "" ? transformState(selectedState) : undefined,
          returnedDate: selectedDate
            ? format(selectedDate, "yyyy-MM-dd")
            : undefined,
          search: keyword !== "" ? keyword : undefined,
        },
      }))()
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

  return (
    <>
      <DataGrid
        title="State"
        filterComponents={[
          {
            id: "filterStateInput",
            component: (
              <FilterInput
                values={states}
                label="State"
                multiple={false}
                selected={selectedState}
                setSelected={setSelectedState}
              />
            ),
          },
          {
            id: "filterReturnedDateInput",
            component: (
              <FilterDate label="Returned Date" setSelected={setSelectedDate} />
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
        tableProps={{
          tableOptionsProps: {
            columns: ReturningRequestColumns(),
            data: [...(data?.returningRequests || [])],
            isLoading: listLoading,
            pageCount: data?.pagedInfo.totalPages ?? 0,
            setSelectedEntityId: setSelectedReturningRequestId,
            pagination: pagination,
            setPagination: setPagination,
            sorting: sorting,
            setSorting: setSorting,
            renderRowActions: (request) => (
              <ReturningRequestRowAction
                key={request.id}
                data={request}
                setCompleteRequestModalOpen={setCompleteRequestModalOpen}
                setCancelRequestModalOpen={setCancelRequestModalOpen}
                setSelectedReturningRequestId={setSelectedReturningRequestId}
              />
            ),
          },
        }}
      />
      <ConfirmModal
        open={completeRequestModalOpen}
        message="Do you want to mark this returning request as 'Completed'?"
        title="Are you sure?"
        buttonOkLabel="Yes"
        buttonCloseLabel="No"
        onOk={() => handleCompleteReturnAction()}
        onClose={() => setCompleteRequestModalOpen(false)}
      />
      <ConfirmModal
        open={cancelRequestModalOpen}
        message="Do you want to cancel this returning request?"
        title="Are you sure?"
        buttonOkLabel="Yes"
        buttonCloseLabel="No"
        onOk={() => handleCancelReturnAction()}
        onClose={() => setCancelRequestModalOpen(false)}
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
