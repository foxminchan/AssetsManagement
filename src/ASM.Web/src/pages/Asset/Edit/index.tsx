import { useContext, useEffect } from "react"
import AssetForm from "@components/forms/asset-form"
import { UpdateAssetRequest } from "@features/assets/asset.type"
import useGetAsset from "@features/assets/useGetAsset"
import { BreadcrumbsContext } from "@libs/contexts/BreadcrumbsContext"
import { CircularProgress } from "@mui/material"
import Container from "@mui/material/Container"
import Typography from "@mui/material/Typography"
import { useNavigate, useParams } from "@tanstack/react-router"

const breadcrumbItems = [
  {
    label: "Manage Asset",
    to: "/asset",
  },
  {
    label: "Edit Asset",
    to: "",
  },
]

export default function EditAsset() {
  const params = useParams({ from: "/_authenticated/asset/$id" })
  const { data, isError, isFetched } = useGetAsset(params?.id)
  const navigate = useNavigate()
  const context = useContext(BreadcrumbsContext)

  useEffect(() => {
    if (isError) {
      navigate({
        to: "/asset",
      })
    }
  }, [isError])

  useEffect(() => {
    context?.setBreadcrumbs(breadcrumbItems)
  }, [])

  const initialData = {
    id: data?.id,
    name: data?.name,
    specification: data?.specification,
    installDate: data?.installDate ? new Date(data.installDate) : null,
    state: data?.state,
    categoryId: data?.categoryId,
  }

  return (
    <Container className="!flex flex-col gap-6">
      <Typography className="!text-lg !font-bold text-red-500">
        Edit Asset
      </Typography>
      {isFetched ? (
        <AssetForm initialData={initialData as UpdateAssetRequest} />
      ) : (
        <CircularProgress className="!text-red-500" />
      )}
    </Container>
  )
}
