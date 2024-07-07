import { useEffect, useState } from "react"
import { User } from "@features/users/user.type"
import { useRouter, useSearch } from "@tanstack/react-router"
import { MaterialReactTable, MRT_SortingState } from "material-react-table"

import DialogOptions from "./dialog-options"

type UserTableProps = {
  data: User[]
  isLoading: boolean
  isChoose?: string
}

export default function UserDialog({
  data,
  isLoading,
  isChoose,
}: Readonly<UserTableProps>) {
  const router = useRouter()
  const params = useSearch({ strict: false })
  const [sorting, setSorting] = useState<MRT_SortingState>([
    { id: "staffCode", desc: false },
  ])

  const table = DialogOptions({
    data,
    sorting,
    setSorting,
    isLoading,
    isChoose,
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
