import { createRootRouteWithContext } from "@tanstack/react-router"

import { AuthContext } from "@/hooks/useAuth"
import NotFound from "@/components/error/not-found"
import MainLayout from "@/components/layouts/main-layout"

type RouterContext = {
  authentication: AuthContext
}

export const Route = createRootRouteWithContext<RouterContext>()({
  component: () => <MainLayout />,
  notFoundComponent: () => <NotFound />,
})
