import Assignments from "@pages/Assignment"
import { createLazyFileRoute } from "@tanstack/react-router"

export const Route = createLazyFileRoute("/_authenticated/assignment/")({
  component: () => <Assignments />,
})
