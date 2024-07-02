import { Dispatch, SetStateAction, useState } from "react"
import { User } from "@features/users/user.type"
import { MRT_SortingState, useMaterialReactTable } from "material-react-table"

import { CellAction } from "../../tables/user-content/cell-action"
import Columns from "../../tables/user-content/columns"

type UserTableProps = {
  data: User[]
  sorting: MRT_SortingState
  setSorting: Dispatch<SetStateAction<MRT_SortingState>>
  isLoading: boolean
}

export default function DialogOptions({
  data,
  sorting,
  setSorting,
  isLoading,
}: Readonly<UserTableProps>) {
  const [selectedRowId, setSelectedRowId] = useState(String)
  const handleSelectRow = (id: string) => {
    setSelectedRowId(id)
  }
  return useMaterialReactTable({
    renderRowActions: ({ row }) => [
      <CellAction
        key={row.original.id}
        data={row.original}
        isSelected={row.original.id === selectedRowId}
        onSelectRow={() => handleSelectRow(row.original.id)}
      />,
    ],
    columns: Columns(),
    data, // Ensure this contains all the data you want to display
    layoutMode: "grid",
    enablePagination: false,
    enableBottomToolbar: false,
    enableTopToolbar: false,
    enableGrouping: false,
    enableRowActions: true,
    enableColumnFilters: false,
    enableColumnActions: false,
    muiPaginationProps: {
      rowsPerPageOptions: [], // Set to an empty array to remove options
      showRowsPerPage: false, // Ensure this is set to false
      count: -1, // Set count to -1 if applicable to disable count display
    },
    manualPagination: false, // Set to false to disable manual pagination
    manualSorting: true,
    onSortingChange: setSorting,
    state: {
      sorting,
      showProgressBars: isLoading,
    },
    initialState: {
      density: "compact",
    },
    displayColumnDefOptions: {
      "mrt-row-actions": {
        header: undefined,
      },
    },
  })
}