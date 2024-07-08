import ReturningRequests from "@pages/Returning"
import { createLazyFileRoute } from "@tanstack/react-router"

export const Route = createLazyFileRoute("/_authenticated/returning/")({
  component: () => <ReturningRequests />,
})
