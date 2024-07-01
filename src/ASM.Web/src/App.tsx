import { BreadcrumbsProvider } from "@libs/contexts/BreadcrumbsContext"
import { useAuth } from "@libs/hooks/useAuth"
import { createTheme, ThemeProvider } from "@mui/material"
import { AdapterDateFns } from "@mui/x-date-pickers/AdapterDateFnsV3"
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider/LocalizationProvider"
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

  const defaultTheme = createTheme({
    palette: {
      primary: {
        main: "#757575",
      },
    },
  })

  const authentication = useAuth()

  return (
    <QueryClientProvider client={queryClient}>
      <ThemeProvider theme={defaultTheme}>
        <JotaiProvider>
          <LocalizationProvider dateAdapter={AdapterDateFns}>
            <BreadcrumbsProvider>
              <RouterProvider router={router} context={{ authentication }} />
            </BreadcrumbsProvider>
          </LocalizationProvider>
        </JotaiProvider>
      </ThemeProvider>
    </QueryClientProvider>
  )
}
