import { menu } from "@libs/constants/menu"
import List from "@mui/material/List"
import ListItem from "@mui/material/ListItem"
import ListItemButton from "@mui/material/ListItemButton"
import Paper from "@mui/material/Paper"
import Typography from "@mui/material/Typography"
import { Link } from "@tanstack/react-router"

import { MenuItem } from "@/types/data"

const activeProps = {
  className: "!text-white !bg-red-500",
}

export default function Menu() {
  return (
    <Paper className="!bg-gray-200">
      <List>
        {menu.map((item: MenuItem) => (
          <ListItem key={item.label} disablePadding>
            <Link
              className="w-full text-black"
              to={item.to}
              activeProps={activeProps}
            >
              <ListItemButton>
                <Typography className="!text-lg !font-bold">
                  {item.label}
                </Typography>
              </ListItemButton>
            </Link>
          </ListItem>
        ))}
      </List>
    </Paper>
  )
}
