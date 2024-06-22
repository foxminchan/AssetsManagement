import { Dispatch, SetStateAction } from "react"
import { User } from "@features/users/user.type"
import {
  MRT_PaginationState,
  MRT_SortingState,
  useMaterialReactTable,
} from "material-react-table"

import { CellAction } from "./cell-action"
import Columns from "./columns"

type UserTableProps = {
  data: User[]
  pageCount: number
  setOpen: Dispatch<SetStateAction<boolean>>
  setSelectedUserId: Dispatch<SetStateAction<string>>
  pagination: MRT_PaginationState
  setPagination: Dispatch<SetStateAction<MRT_PaginationState>>
  sorting: MRT_SortingState
  setSorting: Dispatch<SetStateAction<MRT_SortingState>>
  isLoading: boolean
}

export default function TableOptions({
  data,
  pageCount,
  setOpen,
  setSelectedUserId,
  pagination,
  setPagination,
  sorting,
  setSorting,
  isLoading,
}: Readonly<UserTableProps>) {
  return useMaterialReactTable({
    columns: Columns(),
    data,
    layoutMode: "grid",
    enableTopToolbar: false,
    enableGrouping: false,
    enableRowActions: true,
    enableColumnFilters: false,
    enableColumnActions: false,
    paginationDisplayMode: "pages",
    muiPaginationProps: {
      rowsPerPageOptions: undefined,
      shape: "rounded",
      variant: "outlined",
      color: "secondary",
      showRowsPerPage: false,
    },
    rowCount: pageCount,
    positionActionsColumn: "last",
    manualPagination: true,
    manualSorting: true,
    onPaginationChange: setPagination,
    onSortingChange: setSorting,
    state: {
      pagination,
      sorting,
      showProgressBars: isLoading,
    },
    initialState: { density: "compact" },
    renderRowActions: ({ row }) => [
      <CellAction key={row.original.id} data={row.original} />,
    ],
    displayColumnDefOptions: {
      "mrt-row-actions": {
        header: undefined,
      },
    },
    muiTableBodyRowProps: ({ row }) => ({
      onClick: () => {
        setOpen(true)
        setSelectedUserId(row.original.id)
      },
      sx: {
        cursor: "pointer",
      },
    }),
  })
}
