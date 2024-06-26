import EditUser from "@/pages/User/Edit"
import { createFileRoute } from "@tanstack/react-router"

export const Route = createFileRoute("/_authenticated/user/$id")({
  component: () => <EditUser />,
})
