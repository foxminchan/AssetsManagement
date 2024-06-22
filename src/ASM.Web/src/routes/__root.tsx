import { AuthContext } from "@/libs/hooks/useAuth"
import NotFound from "@components/error/not-found"
import MainLayout from "@components/layouts/main-layout"
import { createRootRouteWithContext } from "@tanstack/react-router"

type RouterContext = {
  authentication: AuthContext
}

export const Route = createRootRouteWithContext<RouterContext>()({
  component: () => <MainLayout />,
  notFoundComponent: () => <NotFound />,
})
