import EditAsset from "@pages/Asset/Edit"
import { createFileRoute } from "@tanstack/react-router"

export const Route = createFileRoute("/_authenticated/asset/$id")({
  component: () => <EditAsset />,
})
