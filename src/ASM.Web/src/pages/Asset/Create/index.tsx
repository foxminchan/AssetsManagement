import AssetForm from "@components/forms/asset-form"
import Container from "@mui/material/Container"
import Typography from "@mui/material/Typography"

export default function CreateAsset() {
  return (
    <Container className="!flex flex-col gap-6">
      <Typography className="!text-lg !font-bold text-red-500">
        Create New Asset
      </Typography>
      <AssetForm />
    </Container>
  )
}
