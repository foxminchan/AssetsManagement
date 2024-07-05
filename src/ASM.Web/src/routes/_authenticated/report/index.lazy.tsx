import Report from "@pages/Report"
import { createLazyFileRoute } from "@tanstack/react-router"

export const Route = createLazyFileRoute("/_authenticated/report/")({
  component: () => <Report />,
})
