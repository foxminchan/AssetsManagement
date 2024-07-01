import CreateAsset from "@pages/Asset/Create"
import { createLazyFileRoute } from "@tanstack/react-router"

export const Route = createLazyFileRoute("/_authenticated/asset/new/")({
  component: () => <CreateAsset />,
})
