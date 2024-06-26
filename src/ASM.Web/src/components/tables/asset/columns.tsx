import { useMemo } from "react"
import { AssetState } from "@features/assets/asset.type"
import { MRT_ColumnDef } from "material-react-table"
import { match } from "ts-pattern"

import { BaseEntity } from "@/types/data"

export default function AssetColumns() {
  return useMemo<MRT_ColumnDef<BaseEntity>[]>(
    () => [
      {
        id: "AssetCode",
        accessorKey: "assetCode",
        header: "Asset Code",
        enableSorting: true,
        size: 120,
      },
      {
        id: "Name",
        accessorKey: "name",
        header: "Asset Name",
        enableSorting: true,
        minSize: 150,
      },
      {
        id: "Category",
        accessorKey: "category",
        header: "Category",
        enableSorting: true,
        minSize: 120,
      },
      {
        id: "State",
        accessorKey: "state",
        header: "State",
        enableSorting: true,
        minSize: 100,
        Cell: ({ cell }) => {
          return match(cell.getValue())
            .with(AssetState.NotAvailable, () => "Not available")
            .with(AssetState.WaitingForRecycling, () => "Waiting for recycling")
            .otherwise(() => cell.getValue()) as string
        },
      },
    ],
    []
  )
}
