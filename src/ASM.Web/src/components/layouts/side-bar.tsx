import logo from "@assets/logo.svg"
import { Typography } from "@mui/material"
import Box from "@mui/material/Box"

import Menu from "./menu"

export default function SideBar() {
  return (
    <div className="my-24">
      <Box className="mb-12">
        <img loading="lazy" src={logo} alt="NashTech Logo" width={200} />
        <Typography className="!text-xl !font-bold !text-red-600">
          Online Asset Management
        </Typography>
      </Box>
      <Menu />
    </div>
  )
}
