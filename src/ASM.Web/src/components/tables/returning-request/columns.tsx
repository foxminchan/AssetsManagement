import { useMemo } from "react"
import { ReturningRequestState } from "@features/returning-requests/returning-request.type"
import { MRT_ColumnDef } from "material-react-table"

import { BaseEntity } from "@/types/data"

export default function ReturningRequestColumns() {
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
        id: "RequestedBy",
        accessorKey: "requestedBy",
        header: "Requested by",
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
      },
      {
        id: "AcceptBy",
        accessorKey: "acceptedBy",
        header: "Accepted by",
        enableSorting: true,
        minSize: 120,
        maxSize: 150,
      },
      {
        id: "ReturnedDate",
        accessorKey: "returnedDate",
        header: "Returned Date",
        enableSorting: true,
        minSize: 120,
        maxSize: 150,
      },
      {
        id: "State",
        accessorKey: "state",
        header: "State",
        enableSorting: true,
        minSize: 120,
        Cell: ({ renderedCellValue }) => {
          if (
            renderedCellValue?.valueOf() ===
            ReturningRequestState.WaitingForReturning
          ) {
            return "Waiting for returning"
          }
          return renderedCellValue
        },
      },
    ],
    []
  )
}
