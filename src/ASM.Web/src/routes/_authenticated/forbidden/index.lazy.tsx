import { createLazyFileRoute } from "@tanstack/react-router"

import Forbidden from "@/components/error/forbidden"

export const Route = createLazyFileRoute("/_authenticated/forbidden/")({
  component: () => <Forbidden />,
})
