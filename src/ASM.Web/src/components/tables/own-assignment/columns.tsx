import { useMemo } from "react"
import { State } from "@features/assignments/assignment.type"
import { format } from "date-fns"
import { MRT_ColumnDef } from "material-react-table"
import { match } from "ts-pattern"

import { BaseEntity } from "@/types/data"

export default function OwnAssignmentColumns() {
  return useMemo<MRT_ColumnDef<BaseEntity>[]>(
    () => [
      {
        id: "AssetCode",
        accessorKey: "assetCode",
        header: "Asset Code",
        enableSorting: true,
        size: 80,
      },
      {
        id: "AssetName",
        accessorKey: "assetName",
        header: "Asset Name",
        enableSorting: true,
        minSize: 80,
        Cell: ({ cell }) => {
          const cellValue = cell.getValue() as string
          return cellValue.length > 15
            ? `${cellValue.substring(0, 15)}...`
            : cellValue
        },
      },
      {
        id: "Category",
        accessorKey: "category",
        header: "Category",
        enableSorting: true,
        size: 130,
      },
      {
        id: "AssignedDate",
        accessorKey: "assignedDate",
        header: "Assigned Date",
        enableSorting: true,
        size: 100,
        Cell: ({ cell }) => {
          return format(cell.getValue() as Date, "dd/MM/yyyy")
        },
      },
      {
        id: "State",
        accessorKey: "state",
        header: "State",
        enableSorting: true,
        maxSize: 152,
        Cell: ({ cell }) => {
          const cellValue = cell.getValue() as string
          const result = match(cellValue)
            .with(State.IsRequested, () => "Is requested")
            .with(State.WaitingForAcceptance, () => "Waiting for acceptance")
            .otherwise(() => State.Accepted)
          return result
        },
      },
    ],
    []
  )
}
