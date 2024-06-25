import { useMemo } from "react"
import { format } from "date-fns"
import { MRT_ColumnDef } from "material-react-table"

import { BaseEntity } from "@/types/data"

export default function UserColumns() {
  return useMemo<MRT_ColumnDef<BaseEntity>[]>(
    () => [
      {
        id: "StaffCode",
        accessorKey: "staffCode",
        header: "Staff Code",
        enableSorting: true,
        size: 120,
      },
      {
        id: "FullName",
        accessorKey: "fullName",
        header: "Full Name",
        enableSorting: true,
        minSize: 120,
      },
      {
        id: "UserName",
        accessorKey: "userName",
        header: "Username",
        enableSorting: true,
        size: 120,
      },
      {
        id: "JoinedDate",
        accessorKey: "joinedDate",
        header: "Joined Date",
        enableSorting: true,
        size: 150,
        Cell: ({ cell }) => {
          return format(cell.getValue() as Date, "dd/MM/yyyy")
        },
      },
      {
        id: "RoleType",
        accessorKey: "roleType",
        header: "Type",
        size: 90,
      },
    ],
    []
  )
}
