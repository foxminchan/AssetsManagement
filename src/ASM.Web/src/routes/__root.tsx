import { createRootRouteWithContext, Outlet } from "@tanstack/react-router"

import { AuthContext } from "@/hooks/useAuth"

type RouterContext = {
  authentication: AuthContext
}

export const Route = createRootRouteWithContext<RouterContext>()({
  component: () => <Outlet />,
})
