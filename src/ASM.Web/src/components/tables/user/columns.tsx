import { useMemo } from "react"
import { User } from "@features/users/user.type"
import { format } from "date-fns"
import { MRT_ColumnDef } from "material-react-table"

export default function Columns() {
  return useMemo<MRT_ColumnDef<User>[]>(
    () => [
      {
        id: "staffCode",
        accessorKey: "staffCode",
        header: "Staff Code",
        enableSorting: true,
        size: 120,
      },
      {
        id: "fullName",
        accessorKey: "fullName",
        header: "Full Name",
        enableSorting: true,
        size: 120,
      },
      {
        id: "userName",
        accessorKey: "userName",
        header: "Username",
        enableSorting: true,
        size: 120,
      },
      {
        id: "joinedDate",
        accessorKey: "joinedDate",
        header: "Joined Date",
        enableSorting: true,
        size: 150,
        Cell: ({ cell }) => {
          return format(cell.getValue() as Date, "dd/MM/yyyy")
        },
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
