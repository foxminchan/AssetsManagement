import { Dispatch, SetStateAction, useEffect, useState } from "react"
import Columns from "@components/tables/user-content/columns"
import { CellAction } from "@components/tables/user-content/row-action"
import { User } from "@features/users/user.type"
import { MRT_SortingState, useMaterialReactTable } from "material-react-table"

type UserTableProps = {
  data: User[]
  sorting: MRT_SortingState
  setSorting: Dispatch<SetStateAction<MRT_SortingState>>
  isLoading: boolean
  isChoose?: string
}

export default function DialogOptions({
  data,
  sorting,
  setSorting,
  isLoading,
  isChoose,
}: Readonly<UserTableProps>) {
  const [selectedRowId, setSelectedRowId] = useState(String)
  const handleSelectRow = (id: string) => {
    setSelectedRowId(id)
  }

  useEffect(() => {
    setSelectedRowId(isChoose as string)
  }, [isChoose])

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
    data,
    layoutMode: "grid",
    enablePagination: false,
    enableBottomToolbar: false,
    enableTopToolbar: false,
    enableGrouping: false,
    enableRowActions: true,
    enableColumnFilters: false,
    enableColumnActions: false,
    muiPaginationProps: {
      rowsPerPageOptions: [],
      showRowsPerPage: false,
      count: -1,
    },
    manualPagination: false,
    manualSorting: true,
    onSortingChange: setSorting,
    state: {
      sorting,
      showProgressBars: isLoading,
    },
    muiTableBodyRowProps: ({ row }) => ({
      onClick: (event: React.MouseEvent<HTMLTableRowElement>) => {
        if (!(event.target instanceof HTMLButtonElement)) {
          handleSelectRow(row.original.id)
        }
      },
      sx: {
        cursor: "pointer",
      },
    }),
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
