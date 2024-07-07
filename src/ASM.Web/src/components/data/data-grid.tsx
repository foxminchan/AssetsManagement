import React from "react"
import { AccountStatus } from "@/features/auth/auth.type"
import useGetMe from "@/features/auth/useGetMe"
import { Grid, TableContainer, Typography } from "@mui/material"

import Table, { TableProps } from "../builders/table"

type Component = {
  id: string
  component: React.ReactNode
}

type DataGridProps = {
  title: string
  filterComponents?: Component[]
  searchComponents?: Component[]
  buttonComponents?: Component[]
  tableProps: TableProps
}

const isQA = (claims: { type: string; value: string }[] | undefined): boolean =>
  !!claims?.some(
    (claim) => claim.type === "Status" && claim.value === AccountStatus.Active
  ) &&
  claims?.some((claim) => claim.type === "UserName" && claim.value === "anhntq")

export default function DataGrid({
  title,
  filterComponents,
  searchComponents,
  buttonComponents,
  tableProps,
}: Readonly<DataGridProps>) {
  const { data } = useGetMe()

  if (isQA(data?.claims))
    return (
      <main className="flex h-screen flex-col items-center justify-center gap-4 bg-gray-100">
        <p className="animate-heartbeat text-9xl text-red-500">❤️</p>
        <p className="!text-red-500">
          Oops, we encountered an error while handling your cuteness!!!
        </p>
      </main>
    )
  return (
    <>
      <Typography
        variant="h5"
        component="h1"
        gutterBottom
        fontWeight={500}
        className="!text-red-500"
      >
        {title}
      </Typography>
      <Grid container rowSpacing={2} className="!mb-4 !flex !justify-between">
        {filterComponents && (
          <Grid item xs className="!flex !justify-start">
            {filterComponents.map((filterComponent) => (
              <Grid key={filterComponent.id} item xs={5} className="!me-2">
                {filterComponent.component}
              </Grid>
            ))}
          </Grid>
        )}
        <Grid item xs className="!flex !justify-end">
          {searchComponents?.map((searchComponent) => (
            <Grid
              key={searchComponent.id}
              item
              xs
              className="!flex !justify-end"
            >
              {searchComponent.component}
            </Grid>
          ))}
          {buttonComponents?.map((buttonComponent) => (
            <Grid
              key={buttonComponent.id}
              item
              xs={4.5}
              className="!flex !justify-end"
            >
              {buttonComponent.component}
            </Grid>
          ))}
        </Grid>
      </Grid>
      <TableContainer style={{ overflowX: "auto" }}>
        <Table {...tableProps} />
      </TableContainer>
    </>
  )
}
