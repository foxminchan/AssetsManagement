import { useMemo } from "react"
import { format } from "date-fns"
import { MRT_ColumnDef } from "material-react-table"

import { BaseEntity } from "@/types/data"

export default function AssignmentColumns() {
  const formatState = (state: string) => {
    return state.replace(/([A-Z])/g, " $1").trim()
  }
  return useMemo<MRT_ColumnDef<BaseEntity>[]>(
    () => [
      {
        id: "No",
        accessorKey: "no",
        header: "No.",
        enableSorting: true,
        maxSize: 70,
      },
      {
        id: "AssetCode",
        accessorKey: "assetCode",
        header: "Asset Code",
        enableSorting: true,
        minSize: 100,
        maxSize: 120,
      },
      {
        id: "Name",
        accessorKey: "assetName",
        header: "Asset Name",
        enableSorting: true,
        minSize: 150,
        maxSize: 200,
      },
      {
        id: "AssignedTo",
        accessorKey: "assignedTo",
        header: "Assigned To",
        enableSorting: true,
        minSize: 120,
        maxSize: 150,
      },
      {
        id: "AssignedBy",
        accessorKey: "assignedBy",
        header: "Assigned By",
        enableSorting: true,
        minSize: 120,
        maxSize: 150,
      },
      {
        id: "AssignedDate",
        accessorKey: "assignedDate",
        header: "Assigned Date",
        enableSorting: true,
        minSize: 120,
        maxSize: 150,
        Cell: ({ cell }) => {
          return format(cell.getValue() as Date, "dd/MM/yyyy")
        },
      },
      {
        id: "State",
        accessorKey: "state",
        header: "State",
        enableSorting: true,
        maxSize: 170,
        Cell: ({ cell }) => {
          return formatState(cell.getValue() as string)
        },
      },
    ],
    []
  )
}
