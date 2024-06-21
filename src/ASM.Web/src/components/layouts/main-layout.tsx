import { CssBaseline } from "@mui/material"
import Container from "@mui/material/Container"
import Grid from "@mui/material/Grid"
import { Outlet } from "@tanstack/react-router"

import { useAuth } from "@/hooks/useAuth"
import NavBar from "@/components/layouts/nav-bar"
import SideBar from "@/components/layouts/side-bar"

export default function MainLayout() {
  const auth = useAuth()
  return auth.isLogged() ? (
    <div className="App">
      <CssBaseline />
      <NavBar />
      <Container fixed className="my-6">
        <Grid container rowSpacing={3}>
          <Grid item xs={3}>
            <SideBar />
          </Grid>
          <Grid item xs>
            <Container className="my-20">
              <Outlet />
            </Container>
          </Grid>
        </Grid>
      </Container>
    </div>
  ) : (
    <Outlet />
  )
}
