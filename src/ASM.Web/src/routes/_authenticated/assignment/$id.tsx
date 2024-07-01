import EditAssignment from "@pages/Assignment/Edit"
import { createFileRoute } from "@tanstack/react-router"

export const Route = createFileRoute("/_authenticated/assignment/$id")({
  component: () => <EditAssignment />,
})
