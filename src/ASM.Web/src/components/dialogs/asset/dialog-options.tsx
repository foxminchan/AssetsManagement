import { Dispatch, SetStateAction, useState } from "react"
import { Asset } from "@features/assets/asset.type"
import { MRT_SortingState, useMaterialReactTable } from "material-react-table"

import { CellAction } from "../../tables/asset-content/cell-action"
import Columns from "../../tables/asset-content/columns"

type AssetTableProps = {
  data: Asset[]
  sorting: MRT_SortingState
  setSorting: Dispatch<SetStateAction<MRT_SortingState>>
  isLoading: boolean
}

export default function AssetDialogOptions({
  data,
  sorting,
  setSorting,
  isLoading,
}: Readonly<AssetTableProps>) {
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
