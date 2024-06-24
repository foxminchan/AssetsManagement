import CreateUser from "@/pages/User/Create"
import { createLazyFileRoute } from "@tanstack/react-router"

export const Route = createLazyFileRoute("/_authenticated/user/new/")({
  component: () => <CreateUser />,
})
