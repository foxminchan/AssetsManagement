import { useState } from "react"
import { AssetState } from "@features/assets/asset.type"
import useGetAsset from "@features/assets/useGetAsset"
import useGetListAssignments from "@features/assignments/useGetListAssignments"
import { Location } from "@features/users/user.type"
import { DEFAULT_PAGE_INDEX, DEFAULT_PAGE_SIZE } from "@libs/constants/default"
import { assetFieldLabels, assetFieldOrder } from "@libs/constants/field"
import { Container, CssBaseline, Grid, Typography } from "@mui/material"
import { format } from "date-fns"
import { MRT_PaginationState, MRT_SortingState } from "material-react-table"
import { match } from "ts-pattern"

import Table from "../builders/table"
import AssignmentColumns from "../tables/detail-asset-table/columns"
import MessageModal from "./message-modal"

type Props = {
  id: string
  open: boolean
  title?: string
  onClose: () => void
}

export default function AssetInfoModal({
  id,
  onClose,
  open,
  title,
}: Readonly<Props>) {
  const queryParameters = {
    pageIndex: DEFAULT_PAGE_INDEX,
    pageSize: DEFAULT_PAGE_SIZE,
    assetId: id,
  }

  const asset = useGetAsset(id)
  const { data, isLoading } = useGetListAssignments(queryParameters)
  const columns = AssignmentColumns()

  const [sorting, setSorting] = useState<MRT_SortingState>([
    { id: "assetCode", desc: true },
  ])
  const [pagination, setPagination] = useState<MRT_PaginationState>({
    pageIndex: queryParameters.pageIndex - 1,
    pageSize: DEFAULT_PAGE_SIZE,
  })

  if (!asset?.data) return null

  return (
    <MessageModal open={open} onClose={onClose} message="" title={title}>
      <CssBaseline />
      <Grid container>
        <Container className="!flex flex-col">
          {assetFieldOrder.map((key) => {
            if (key === "id") return null

            const value = asset.data?.[key]

            const formattedValue = match(key)
              .with("installDate", () =>
                format(new Date(value || ""), "dd/MM/yyyy")
              )
              .with("state", () =>
                match(value)
                  .with(AssetState.NotAvailable, () => "Not available")
                  .with(
                    AssetState.WaitingForRecycling,
                    () => "Waiting for recycling"
                  )
                  .otherwise(() => value)
              )
              .with("location", () =>
                match(value)
                  .with(Location.HoChiMinh, () => "HCM")
                  .with(Location.Hanoi, () => "HN")
                  .with(Location.DaNang, () => "DN")
                  .otherwise(() => value)
              )
              .otherwise(() => value)

            return (
              <Grid container key={key}>
                <Grid xs={4} item>
                  {assetFieldLabels[key]}
                </Grid>
                <Grid xs item>
                  <Typography>{String(formattedValue)}</Typography>
                </Grid>
              </Grid>
            )
          })}
          <Grid container>
            <Grid xs={4} className="min-h-fit" item>
              Assignments
            </Grid>
            <Grid xs={8} className="!max-h-40 !overflow-scroll" item>
              <Table
                tableOptionsProps={{
                  columns: columns,
                  data: [...(data?.assignments || [])],
                  isLoading: isLoading,
                  pageCount: data?.pagedInfo.totalPages ?? 0,
                  disablePagination: true,
                  pagination: pagination,
                  setPagination: setPagination,
                  sorting: sorting,
                  setSorting: setSorting,
                  disableSorting: true,
                }}
              />
            </Grid>
          </Grid>
        </Container>
      </Grid>
    </MessageModal>
  )
}
