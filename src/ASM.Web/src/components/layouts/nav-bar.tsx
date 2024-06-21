import { useContext } from "react"
import { BreadcrumbsContext } from "@/contexts/BreadcrumbsContext"
import NavigateNextIcon from "@mui/icons-material/NavigateNext"
import AppBar from "@mui/material/AppBar"
import Breadcrumbs from "@mui/material/Breadcrumbs"
import Button from "@mui/material/Button"
import Link from "@mui/material/Link"
import Toolbar from "@mui/material/Toolbar"

import { BreadcrumbItem } from "@/types/data"

export default function NavBar() {
  const context = useContext(BreadcrumbsContext)

  return (
    <AppBar position="relative" className="!bg-red-600">
      <Toolbar className="mx-40">
        <Breadcrumbs
          separator={<NavigateNextIcon fontSize="small" />}
          aria-label="breadcrumb"
          className="!text-white"
          sx={{ flexGrow: 1 }}
        >
          {context?.breadcrumbs?.map((item: BreadcrumbItem) => {
            return (
              <Link underline="hover" key="1" color="inherit" href={item.to}>
                {item.label}
              </Link>
            )
          })}
        </Breadcrumbs>

        <Button id="btn-login" color="inherit">
          Login
        </Button>
      </Toolbar>
    </AppBar>
  )
}
