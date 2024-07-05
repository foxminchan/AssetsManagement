import { useMemo } from "react"
import { MRT_ColumnDef } from "material-react-table"

import { BaseEntity } from "@/types/data"

export default function AssetsByCategoryReportColumns() {
  return useMemo<MRT_ColumnDef<BaseEntity>[]>(
    () => [
      {
        id: "Category",
        accessorKey: "category",
        header: "Category",
        enableSorting: true,
        maxSize: 120,
      },
      {
        id: "Total",
        accessorKey: "total",
        header: "Total",
        enableSorting: true,
        maxSize: 50,
      },
      {
        id: "Assigned",
        accessorKey: "assigned",
        header: "Assigned",
        enableSorting: true,
        maxSize: 80,
      },
      {
        id: "Available",
        accessorKey: "available",
        header: "Available",
        enableSorting: true,
        maxSize: 80,
      },
      {
        id: "NotAvailable",
        accessorKey: "notAvailable",
        header: "Not Available",
        enableSorting: true,
        maxSize: 120,
      },
      {
        id: "WaitingForRecycling",
        accessorKey: "waitingForRecycling",
        header: "Waiting For Recycling",
        enableSorting: true,
        maxSize: 100,
      },
      {
        id: "Recycled",
        accessorKey: "recycled",
        header: "Recycled",
        enableSorting: true,
        maxSize: 100,
      },
    ],
    []
  )
}
