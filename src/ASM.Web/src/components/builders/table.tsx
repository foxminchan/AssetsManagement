import { MaterialReactTable } from "material-react-table"

import TableOptions, { TableOptionsProps } from "./table-options"

export type TableProps = {
  tableOptionsProps: TableOptionsProps
}

export default function Table({ tableOptionsProps }: Readonly<TableProps>) {
  const table = TableOptions({
    ...tableOptionsProps,
  })

  return <MaterialReactTable table={table} />
}
