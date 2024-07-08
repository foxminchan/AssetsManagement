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
        maxSize: 160,
      },
      {
        id: "Total",
        accessorKey: "total",
        header: "Total",
        enableSorting: true,
        minSize: 50,
        maxSize: 80,
      },
      {
        id: "Assigned",
        accessorKey: "assigned",
        header: "Assigned",
        enableSorting: true,
        minSize: 80,
        maxSize: 120,
      },
      {
        id: "Available",
        accessorKey: "available",
        header: "Available",
        enableSorting: true,
        minSize: 80,
        maxSize: 120,
      },
      {
        id: "NotAvailable",
        accessorKey: "notAvailable",
        header: "Not Available",
        enableSorting: true,
        minSize: 120,
        maxSize: 140,
      },
      {
        id: "WaitingForRecycling",
        accessorKey: "waitingForRecycling",
        header: "Waiting For Recycling",
        enableSorting: true,
        minSize: 180,
        maxSize: 200,
      },
      {
        id: "Recycled",
        accessorKey: "recycled",
        header: "Recycled",
        enableSorting: true,
        minSize: 100,
        maxSize: 120,
      },
    ],
    []
  )
}
