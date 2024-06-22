import { Dispatch, useEffect, useState } from "react"
import { User } from "@features/users/user.type"
import { DEFAULT_PAGE_SIZE } from "@libs/constants/default"
import { useRouter, useSearch } from "@tanstack/react-router"
import { SetStateAction } from "jotai"
import {
  MaterialReactTable,
  MRT_PaginationState,
  MRT_SortingState,
} from "material-react-table"

import TableOptions from "./table-options"

type UserTableProps = {
  data: User[]
  pageCount: number
  isLoading: boolean
  setOpen: Dispatch<SetStateAction<boolean>>
  setSelectedUserId: Dispatch<SetStateAction<string>>
}

export default function UserTable({
  data,
  pageCount,
  setOpen,
  setSelectedUserId,
  isLoading,
}: Readonly<UserTableProps>) {
  const router = useRouter()
  const params = useSearch({ strict: false })
  const [sorting, setSorting] = useState<MRT_SortingState>([
    { id: "staffCode", desc: false },
  ])
  const [pagination, setPagination] = useState<MRT_PaginationState>({
    pageIndex: 1,
    pageSize: DEFAULT_PAGE_SIZE,
  })

  const table = TableOptions({
    data,
    pageCount,
    setOpen,
    setSelectedUserId,
    pagination,
    setPagination,
    sorting,
    setSorting,
    isLoading,
  })

  useEffect(() => {
    ;(async () =>
      await router.navigate({
        search: {
          ...params,
          pageIndex: pagination.pageIndex + 1,
          pageSize: pagination.pageSize,
          orderBy: sorting[0]?.id,
          isDescending: sorting[0]?.desc,
        },
      }))()
  }, [pagination, sorting])

  return <MaterialReactTable table={table} />
}
