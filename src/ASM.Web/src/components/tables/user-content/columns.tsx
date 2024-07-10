import { useMemo } from "react"
import { User } from "@features/users/user.type"
import { MRT_ColumnDef } from "material-react-table"

export default function Columns() {
  return useMemo<MRT_ColumnDef<User>[]>(
    () => [
      {
        id: "staffCode",
        accessorKey: "staffCode",
        header: "Staff Code",
        enableSorting: true,
        size: 100,
      },
      {
        id: "userName",
        accessorKey: "userName",
        header: "User Name",
        enableSorting: true,
        size: 120,
      },
      {
        id: "roleType",
        accessorKey: "roleType",
        header: "Type",
        size: 90,
      },
    ],
    []
  )
}
