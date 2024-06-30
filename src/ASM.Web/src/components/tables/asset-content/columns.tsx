import { useMemo } from "react"
import { Asset } from "@features/assets/asset.type"
import { MRT_ColumnDef } from "material-react-table"

export default function Columns() {
  return useMemo<MRT_ColumnDef<Asset>[]>(
    () => [
      {
        id: "assetCode",
        accessorKey: "assetCode",
        header: "Asset Code",
        enableSorting: true,
        size: 100,
      },
      {
        id: "name",
        accessorKey: "name",
        header: "Asset Name",
        enableSorting: true,
        size: 150,
      },
      {
        id: "category",
        accessorKey: "category",
        header: "Category",
        size: 100,
      },
    ],
    []
  )
}
