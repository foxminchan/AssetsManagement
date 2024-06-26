import { useContext, useEffect, useState } from "react"
import { AccountStatus } from "@features/auth/auth.type"
import useGetMe from "@features/auth/useGetMe"
import { BreadcrumbsContext } from "@libs/contexts/BreadcrumbsContext"
import { useAuth } from "@libs/hooks/useAuth"
import { userInfo } from "@libs/jotai/userInfoAtom"
import KeyboardArrowDownIcon from "@mui/icons-material/KeyboardArrowDown"
import NavigateNextIcon from "@mui/icons-material/NavigateNext"
import { Menu, MenuItem } from "@mui/material"
import AppBar from "@mui/material/AppBar"
import Breadcrumbs from "@mui/material/Breadcrumbs"
import Button from "@mui/material/Button"
import Link from "@mui/material/Link"
import Toolbar from "@mui/material/Toolbar"
import { useAtom } from "jotai"

import { RouteItem } from "@/types/data"

import ChangePasswordModal from "../modals/change-password-modal"
import ConfirmModal from "../modals/confirm-modal"

export default function NavBar() {
  const context = useContext(BreadcrumbsContext)
  const [anchorElement, setAnchorElement] = useState<null | HTMLElement>(null)
  const [openLogoutConfirmModal, setOpenLogoutConfirmModal] = useState(false)
  const [openChangePasswordModal, setOpenChangePasswordModal] = useState(false)
  const auth = useAuth()
  const [value, setValue] = useAtom(userInfo)
  const open = Boolean(anchorElement)
  const { data } = useGetMe()

  useEffect(() => {
    if (data && auth.isLogged()) {
      try {
        const userInfo = data
        setValue(userInfo)
        if (userInfo.accountStatus == AccountStatus.FirstTime) {
          setOpenChangePasswordModal(true)
        }
      } catch (error) {
        console.error("Failed to fetch user data:", error)
        setValue(null)
      }
    }
  }, [data])

  const handleClick = (
    setter: React.Dispatch<React.SetStateAction<boolean>>,
    open: boolean
  ) => {
    handleClose()
    setter(open)
  }

  const handleLogout = () => {
    auth.signOut()
    setValue(null)
    window.location.reload()
  }

  const handleClickDropDown = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorElement(event.currentTarget)
  }

  const handleClose = () => {
    setAnchorElement(null)
  }

  return (
    <AppBar position="relative" className="!bg-red-600">
      <Toolbar className="mx-40">
        <Breadcrumbs
          separator={<NavigateNextIcon fontSize="small" />}
          aria-label="breadcrumb"
          className="!text-white"
          sx={{ flexGrow: 1 }}
        >
          {context?.breadcrumbs?.map((item: RouteItem) => {
            return (
              <Link underline="hover" key="1" color="inherit" href={item.to}>
                {item.label}
              </Link>
            )
          })}
        </Breadcrumbs>

        {auth.isLogged() && value ? (
          <div className="bg-red-600">
            <Button
              id="btn-profile"
              aria-controls={open ? "menu-profile" : undefined}
              aria-haspopup="true"
              aria-expanded={open ? "true" : undefined}
              variant="contained"
              disableElevation
              onClick={handleClickDropDown}
              endIcon={<KeyboardArrowDownIcon />}
              className="!bg-red-600 text-white transition duration-300 hover:bg-red-700"
            >
              {value.claims?.find((x) => x.type == "UserName")?.value ??
                "Loading"}
            </Button>
            <Menu
              id="menu-profile"
              anchorEl={anchorElement}
              open={open}
              onClose={handleClose}
              classes={{ paper: "rounded-lg shadow-lg" }}
            >
              <MenuItem
                id="btn-change-password"
                onClick={() => handleClick(setOpenChangePasswordModal, true)}
                className="hover:bg-red-100"
              >
                Change Password
              </MenuItem>
              <MenuItem
                id="btn-logout"
                onClick={() => handleClick(setOpenLogoutConfirmModal, true)}
                className="hover:bg-red-100"
              >
                Log Out
              </MenuItem>
            </Menu>
          </div>
        ) : (
          <Link href="/" underline="hover" color="inherit">
            Login
          </Link>
        )}
        <ConfirmModal
          open={openLogoutConfirmModal}
          message="Do you want to log out?"
          title="Are you sure?"
          buttonOkLabel="Log out"
          buttonCloseLabel="Cancel"
          onOk={() => handleLogout()}
          onClose={() => handleClick(setOpenLogoutConfirmModal, false)}
        />
        <ChangePasswordModal
          open={openChangePasswordModal}
          onClose={() => handleClick(setOpenChangePasswordModal, false)}
          user={value}
          FirstTime={value?.accountStatus === AccountStatus.FirstTime}
        />
      </Toolbar>
    </AppBar>
  )
}
