import { useContext, useEffect, useState } from "react"
import DataGrid from "@components/data/data-grid"
import AssetsByCategoryReportColumns from "@components/tables/report/columns"
import useExportAssetsByCategoryReport from "@features/report/useExportAssetsByCategoryReport"
import useGetAssetsByCategoryReport from "@features/report/useGetAssetsByCategoryReport"
import { DEFAULT_PAGE_SIZE } from "@libs/constants/default"
import { BreadcrumbsContext } from "@libs/contexts/BreadcrumbsContext"
import { Button } from "@mui/material"
import { useRouter, useSearch } from "@tanstack/react-router"
import saveAs from "file-saver"
import { MRT_PaginationState, MRT_SortingState } from "material-react-table"

import { RouteItem } from "@/types/data"

const breadcrumb: RouteItem[] = [
  {
    label: "Report",
    to: "/report",
  },
]

export default function Report() {
  const context = useContext(BreadcrumbsContext)
  const router = useRouter()
  useEffect(() => {
    context?.setBreadcrumbs(breadcrumb)
  }, [])

  const params = useSearch({
    strict: false,
  })

  const queryParameters = {
    orderBy: (params as { orderBy?: string }).orderBy ?? "Category",
    isDescending: (params as { isDescending?: boolean }).isDescending ?? false,
  }

  const [sorting, setSorting] = useState<MRT_SortingState>([
    { id: queryParameters.orderBy, desc: queryParameters.isDescending },
  ])
  const [pagination] = useState<MRT_PaginationState>({
    pageIndex: 1,
    pageSize: DEFAULT_PAGE_SIZE,
  })
  const { data, isLoading: listLoading } =
    useGetAssetsByCategoryReport(queryParameters)

  const { data: Report, isLoading: reportLoading } =
    useExportAssetsByCategoryReport(queryParameters)

  useEffect(() => {
    sorting[0] = {
      id: queryParameters.orderBy,
      desc: queryParameters.isDescending,
    }
  }, [params])

  useEffect(() => {
    ;(async () =>
      await router.navigate({
        search: {
          ...queryParameters,
          orderBy: sorting[0]?.id,
          isDescending: sorting[0]?.desc,
        },
      }))()
  }, [sorting])

  return (
    <DataGrid
      title="Report"
      buttonComponents={[
        {
          id: "btn-export-report",
          component: (
            <Button
              variant="contained"
              color="error"
              sx={{
                fontSize: "0.75rem",
                padding: "4px 8px",
                minWidth: "auto",
              }}
              disabled={reportLoading}
              onClick={() => saveAs(Report || new Blob(), "Report.xlsx")}
            >
              {reportLoading ? "Exporting..." : "Export"}
            </Button>
          ),
        },
      ]}
      tableProps={{
        tableOptionsProps: {
          columns: AssetsByCategoryReportColumns(),
          data: data || [],
          isLoading: listLoading,
          sorting: sorting,
          disablePagination: true,
          setSorting: setSorting,
          actionState: 120,
          pageCount: 0,
          pagination: pagination,
          setPagination: function (): void {
            throw new Error("Function not implemented.")
          },
        },
      }}
    />
  )
}
