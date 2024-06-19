import Login from "@/pages/Auth"
import { createFileRoute, redirect } from "@tanstack/react-router"

export const Route = createFileRoute("/login")({
  beforeLoad: async ({ context }) => {
    const { isLogged } = context.authentication
    if (isLogged()) {
      throw redirect({
        to: "/",
      })
    }
  },
  component: () => <Login />,
})
