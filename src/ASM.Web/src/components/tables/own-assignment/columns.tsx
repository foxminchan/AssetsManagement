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
        maxSize: 120,
      },
      {
        id: "AssetName",
        accessorKey: "assetName",
        header: "Asset Name",
        enableSorting: true,
        minSize: 80,
        maxSize: 170,
      },
      {
        id: "Category",
        accessorKey: "category",
        header: "Category",
        enableSorting: true,
        minSize: 80,
        maxSize: 130,
      },
      {
        id: "AssignedDate",
        accessorKey: "assignedDate",
        header: "Assigned Date",
        enableSorting: true,
        minSize: 100,
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
        minSize: 80,
        maxSize: 120,
        Cell: ({ cell }) => {
          const cellValue = cell.getValue() as string
          const result = match(cellValue)
            .with(State.RequestForReturning, () => "Request for returning")
            .with(State.WaitingForAcceptance, () => "Waiting for acceptance")
            .otherwise(() => State.Accepted)
          return result
        },
      },
    ],
    []
  )
}
