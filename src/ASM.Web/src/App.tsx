import { useAuth } from "@hooks/useAuth"
import { createTheme, ThemeProvider } from "@mui/material"
import { QueryClient, QueryClientProvider } from "@tanstack/react-query"
import { createRouter, RouterProvider } from "@tanstack/react-router"
import { Provider as JotaiProvider } from "jotai"

type AppProps = { router: ReturnType<typeof createRouter> }

export default function App({ router }: Readonly<AppProps>) {
  const queryClient = new QueryClient({
    defaultOptions: {
      queries: {
        staleTime: 5 * 60 * 1000,
      },
    },
  })

  const defaultTheme = createTheme()

  const authentication = useAuth()

  return (
    <QueryClientProvider client={queryClient}>
      <ThemeProvider theme={defaultTheme}>
        <JotaiProvider>
          <RouterProvider router={router} context={{ authentication }} />
        </JotaiProvider>
      </ThemeProvider>
    </QueryClientProvider>
  )
}
