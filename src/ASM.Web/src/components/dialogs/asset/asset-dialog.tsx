import { useEffect, useState } from "react"
import { Asset } from "@features/assets/asset.type"
import { useRouter, useSearch } from "@tanstack/react-router"
import { MaterialReactTable, MRT_SortingState } from "material-react-table"

import AssetDialogOptions from "./dialog-options"

type AssetTableProps = {
  data: Asset[]
  isLoading: boolean
}

export default function AssetDialog({
  data,
  isLoading,
}: Readonly<AssetTableProps>) {
  const router = useRouter()
  const params = useSearch({ strict: false })
  const [sorting, setSorting] = useState<MRT_SortingState>([
    { id: "assetCode", desc: false },
  ])

  const table = AssetDialogOptions({
    data,
    sorting,
    setSorting,
    isLoading,
  })

  useEffect(() => {
    ;(async () =>
      await router.navigate({
        search: {
          ...params,
          orderBy: sorting[0]?.id,
          isDescending: sorting[0]?.desc,
        },
      }))()
  }, [sorting])

  return (
    <div style={{ maxHeight: "500px", overflowY: "auto" }}>
      <MaterialReactTable table={table} />
    </div>
  )
}
