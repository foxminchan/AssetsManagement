import { useContext, useEffect, useState } from "react"
import DataGrid from "@components/data/data-grid"
import ConfirmModal from "@components/modals/confirm-modal"
import ErrorModal from "@components/modals/error-modal"
import OwnAssignmentModal from "@components/modals/own-assignment-modal"
import OwnAssignmentColumns from "@components/tables/own-assignment/columns"
import { OwnAssignmentRowAction } from "@components/tables/own-assignment/row-action"
import useAcceptAssignment from "@features/assignments/useAcceptAssignment"
import useDeleteAssignment from "@features/assignments/useDeleteAssignment"
import useListOwnAssignments from "@features/assignments/useListOwnAssignments"
import useRequestForReturningAssignment from "@features/assignments/useRequestForReturningAssignment"
import { Action } from "@libs/constants/action"
import { DEFAULT_PAGE_SIZE } from "@libs/constants/default"
import { BreadcrumbsContext } from "@libs/contexts/BreadcrumbsContext"
import { useRouter, useSearch } from "@tanstack/react-router"
import { MRT_PaginationState, MRT_SortingState } from "material-react-table"
import { match } from "ts-pattern"

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
  const [openErrorModal, setOpenErrorModal] = useState(false)
  const [selectedAssignmentId, setSelectedAssignmentId] = useState<string>("")
  const [action, setAction] = useState<string>("")
  const [openDisableConfirmMod, setOpenDisableConfirmMod] = useState(false)
  const { mutate: acceptAssignment, error: acceptAssignmentError } =
    useAcceptAssignment()
  const {
    mutate: requestForReturningAssignment,
    error: requestForReturningAssignmentError,
  } = useRequestForReturningAssignment()
  const { mutate: deleteAssignment, error: deleteAssignmentError } =
    useDeleteAssignment()
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

  const handleAssignmentAction = (id: string, action: string) => {
    match(action)
      .with(Action.Accept, () => {
        acceptAssignment(id)
      })
      .with(Action.Delete, () => {
        deleteAssignment(id)
      })
      .otherwise(() => {
        requestForReturningAssignment(id)
      })

    setOpenDisableConfirmMod(false)
  }

  useEffect(() => {
    if (
      acceptAssignmentError ||
      deleteAssignmentError ||
      requestForReturningAssignmentError
    ) {
      setOpenErrorModal(true)
    }
  }, [
    acceptAssignmentError,
    deleteAssignmentError,
    requestForReturningAssignmentError,
  ])

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
              <OwnAssignmentRowAction
                key={assignment.id}
                data={assignment}
                action={setAction}
                openModal={setOpenDisableConfirmMod}
                id={setSelectedAssignmentId}
              />
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
      <ConfirmModal
        open={openDisableConfirmMod}
        message={match(action)
          .with(Action.Accept, () => "Do you want to accept this assignment?")
          .with(Action.Delete, () => "Do you want to decline this assignment?")
          .otherwise(
            () => "Do you want to create a returning request for this asset?"
          )}
        title="Are you sure?"
        buttonOkLabel={match(action)
          .with(Action.Accept, () => Action.Accept)
          .with(Action.Delete, () => "Decline")
          .otherwise(() => "Yes")}
        buttonCloseLabel="Cancel"
        onOk={() => handleAssignmentAction(selectedAssignmentId, action)}
        onClose={() => setOpenDisableConfirmMod(false)}
      />
      <ErrorModal
        open={openErrorModal}
        actionName="Reload"
        onOK={() => {
          setOpenErrorModal(false)
          window.location.reload()
        }}
      />
    </>
  )
}
