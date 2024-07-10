import { Dispatch, SetStateAction } from "react"
import { PaginationItem } from "@mui/material"
import {
  MRT_ColumnDef,
  MRT_PaginationState,
  MRT_SortingState,
  useMaterialReactTable,
} from "material-react-table"

import { BaseEntity } from "@/types/data"

export type TableOptionsProps = {
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  columns: MRT_ColumnDef<BaseEntity, any>[]
  data: BaseEntity[]
  disablePagination?: boolean
  disableSorting?: boolean
  isLoading: boolean
  pageCount?: number
  setOpen?: Dispatch<SetStateAction<boolean>>
  setSelectedEntityId?: Dispatch<SetStateAction<string>>
  pagination?: MRT_PaginationState
  setPagination?: Dispatch<SetStateAction<MRT_PaginationState>>
  sorting?: MRT_SortingState
  setSorting?: Dispatch<SetStateAction<MRT_SortingState>>
  actionState?: number
  renderRowActions?: (params: BaseEntity) => React.ReactNode
}

export default function TableOptions({
  columns,
  data,
  disablePagination = false,
  disableSorting = false,
  isLoading,
  pageCount,
  setOpen,
  setSelectedEntityId,
  pagination,
  setPagination,
  sorting,
  setSorting,
  actionState,
  renderRowActions,
}: Readonly<TableOptionsProps>) {
  return useMaterialReactTable({
    columns: columns,
    data,
    layoutMode: "grid",
    enableSorting: !disableSorting,
    enablePagination: !disablePagination,
    enableBottomToolbar: !disablePagination,
    enableTopToolbar: false,
    enableGrouping: false,
    enableRowActions: renderRowActions != undefined,
    enableColumnFilters: false,
    enableColumnActions: false,
    paginationDisplayMode: "pages",
    muiPaginationProps: {
      rowsPerPageOptions: undefined,
      shape: "rounded",
      showRowsPerPage: false,
      renderItem: (item) => {
        if (item?.type === "page") {
          if (item?.selected) {
            return (
              <PaginationItem
                {...item}
                className="!bg-red-500 !text-white"
                variant="text"
              />
            )
          } else {
            return (
              <PaginationItem
                {...item}
                className="!bg-white-500 !text-red-500"
                variant="outlined"
              />
            )
          }
        }
        if (
          item?.type === "previous" ||
          item?.type === "next" ||
          item.type === "start-ellipsis" ||
          item.type === "end-ellipsis"
        ) {
          return (
            <PaginationItem
              {...item}
              className="!text-red-500 disabled:!text-red-700"
              variant="outlined"
            />
          )
        }
      },
    },
    rowCount: pagination?.pageSize ?? 20,
    pageCount: pageCount,
    positionActionsColumn: "last",
    manualPagination: true,
    manualSorting: true,
    onPaginationChange: setPagination,
    onSortingChange: setSorting,
    state: {
      pagination,
      sorting,
      isLoading,
      showProgressBars: isLoading,
    },
    initialState: { density: "compact" },
    renderRowActions: ({ row }) => renderRowActions?.(row.original),
    displayColumnDefOptions: {
      "mrt-row-actions": {
        header: undefined,
        size: actionState,
      },
    },
    muiTableBodyRowProps: ({ row }) => ({
      onClick: (event: React.MouseEvent<HTMLTableRowElement>) => {
        if (!(event.target instanceof HTMLButtonElement)) {
          setOpen?.(true)
          setSelectedEntityId?.(row.original.id)
        }
      },
      sx: {
        cursor: "pointer",
      },
    }),
  })
}
