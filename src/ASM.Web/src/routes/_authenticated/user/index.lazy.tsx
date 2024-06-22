import Users from "@pages/User"
import { createLazyFileRoute } from "@tanstack/react-router"

export const Route = createLazyFileRoute("/_authenticated/user/")({
  component: () => <Users />,
})
