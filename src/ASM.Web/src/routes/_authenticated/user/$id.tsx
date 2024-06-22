import { createFileRoute } from "@tanstack/react-router"

export const Route = createFileRoute("/_authenticated/user/$id")({
  component: () => <div>Hello /_authenticated/user/$id!</div>,
})
