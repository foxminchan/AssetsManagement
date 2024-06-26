import { useMemo } from "react"
import { format } from "date-fns"
import { MRT_ColumnDef } from "material-react-table"

import { BaseEntity } from "@/types/data"

export default function AssignmentColumns() {
  return useMemo<MRT_ColumnDef<BaseEntity>[]>(
    () => [
      {
        id: "AssignedDate",
        accessorKey: "assignedDate",
        header: "Date",
        enableSorting: true,
        size: 90,
        Cell: ({ cell }) => {
          return format(cell.getValue() as Date, "dd/MM/yyyy")
        },
      },
      {
        id: "AssignedTo",
        accessorKey: "assignedTo",
        header: "Assigned to",
        enableSorting: true,
        size: 120,
      },
      {
        id: "AssignedBy",
        accessorKey: "assignedBy",
        header: "Assigned by",
        enableSorting: true,
        size: 120,
      },
      {
        id: "ReturnedDate",
        accessorKey: "returnedDate",
        header: "Returned Date",
        enableSorting: true,
        size: 120,
      },
    ],
    []
  )
}
