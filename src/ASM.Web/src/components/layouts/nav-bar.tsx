import { useContext, useEffect, useState } from "react"
import { BreadcrumbsContext } from "@/context/BreadcrumbsContext"
import authService from "@/features/auth/auth.service"
import { AccountStatus, userInfo } from "@/features/auth/auth.type"
import KeyboardArrowDownIcon from "@mui/icons-material/KeyboardArrowDown"
import NavigateNextIcon from "@mui/icons-material/NavigateNext"
import { Menu, MenuItem } from "@mui/material"
import AppBar from "@mui/material/AppBar"
import Breadcrumbs from "@mui/material/Breadcrumbs"
import Button from "@mui/material/Button"
import Link from "@mui/material/Link"
import Toolbar from "@mui/material/Toolbar"
import { useAtom } from "jotai"

import { BreadcrumbItem } from "@/types/data"
import { useAuth } from "@/hooks/useAuth"

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

  useEffect(() => {
    const fetchUserData = async () => {
      if (auth.isLogged()) {
        try {
          const userData = await authService.getUser()
          setValue(userData)
          if (userData.accountStatus == AccountStatus.FirstTime) {
            setOpenChangePasswordModal(true)
          }
        } catch (error) {
          console.error("Failed to fetch user data:", error)
          setValue(null)
        }
      }
    }

    fetchUserData()
  }, [])

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
          {context?.breadcrumbs?.map((item: BreadcrumbItem) => {
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
              {value.claims?.find((x) => x.type == "UserName")?.value ||
                "No Claim"}
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
          <p>Log in</p>
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
