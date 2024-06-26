import Assets from "@pages/Asset"
import { createLazyFileRoute } from "@tanstack/react-router"

export const Route = createLazyFileRoute("/_authenticated/asset/")({
  component: () => <Assets />,
})
