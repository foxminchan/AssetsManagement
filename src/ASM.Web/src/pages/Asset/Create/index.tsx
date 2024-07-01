import { useContext, useEffect } from "react"
import AssetForm from "@components/forms/asset-form"
import { BreadcrumbsContext } from "@libs/contexts/BreadcrumbsContext"
import Container from "@mui/material/Container"
import Typography from "@mui/material/Typography"

const breadcrumbItems = [
  {
    label: "Manage Asset",
    to: "/asset",
  },
  {
    label: "Create New Asset",
    to: "/asset/new",
  },
]

export default function CreateAsset() {
  const context = useContext(BreadcrumbsContext)

  useEffect(() => {
    context?.setBreadcrumbs(breadcrumbItems)
  }, [])

  return (
    <Container className="!flex flex-col gap-6">
      <Typography className="!text-lg !font-bold text-red-500">
        Create New Asset
      </Typography>
      <AssetForm />
    </Container>
  )
}
