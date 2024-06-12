import CreateUser from "@/pages/User/Create"
import { createFileRoute } from "@tanstack/react-router"

export const Route = createFileRoute("/user/new")({
  component: () => <CreateUser />,
})
