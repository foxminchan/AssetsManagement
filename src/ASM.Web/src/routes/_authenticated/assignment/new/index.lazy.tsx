import CreateAssignment from "@pages/Assignment/Create"
import { createLazyFileRoute } from "@tanstack/react-router"

export const Route = createLazyFileRoute("/_authenticated/assignment/new/")({
  component: () => <CreateAssignment />,
})
