import { RoleType } from "@features/users/user.type"
import { menu } from "@libs/constants/menu"
import { userInfo } from "@libs/jotai/userInfoAtom"
import List from "@mui/material/List"
import ListItem from "@mui/material/ListItem"
import ListItemButton from "@mui/material/ListItemButton"
import Paper from "@mui/material/Paper"
import Typography from "@mui/material/Typography"
import { Link } from "@tanstack/react-router"
import { useAtom } from "jotai"
import { match } from "ts-pattern"

import { RouteItem } from "@/types/data"

const activeProps = {
  className: "!text-white !bg-red-500",
}

const isAdmin = (
  claims: { type: string; value: string }[] | undefined
): boolean =>
  !!claims?.some(
    (claim) => claim.type === "AuthRole" && claim.value === RoleType.Admin
  )

export default function Menu() {
  const [value] = useAtom(userInfo)

  const admin = isAdmin(value?.claims)

  return (
    <Paper className="!bg-gray-200">
      <List>
        {match(admin)
          .with(true, () =>
            menu.map((item: RouteItem) => (
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
            ))
          )
          .with(false, () => (
            <ListItem disablePadding>
              <Link
                className="w-full text-black"
                to="/home"
                activeProps={activeProps}
              >
                <ListItemButton>
                  <Typography className="!text-lg !font-bold">Home</Typography>
                </ListItemButton>
              </Link>
            </ListItem>
          ))
          .exhaustive()}
      </List>
    </Paper>
  )
}
