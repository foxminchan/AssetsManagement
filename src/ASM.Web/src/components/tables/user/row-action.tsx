import { FC, useState } from "react"
import useDeleteUser from "@/features/users/useDeleteUser"
import ConfirmModal from "@components/modals/confirm-modal"
import { RoleType, User } from "@features/users/user.type"
import EditIcon from "@mui/icons-material/Edit"
import HighlightOffIcon from "@mui/icons-material/HighlightOff"
import { IconButton } from "@mui/material"
import { useNavigate } from "@tanstack/react-router"

import { BaseEntity } from "@/types/data"

type UserRowActionProps = {
  data: BaseEntity
}

export const UserRowAction: FC<UserRowActionProps> = ({ data }) => {
  const navigate = useNavigate({ from: "/user/$id" })
  const [openDisableConfirmMod, setOpenDisableConfirmMod] = useState(false)
  const { mutate: deleteUser } = useDeleteUser()
  const handleDisableUser = (id: string) =>
    Promise.resolve(deleteUser(id)).then(() => (window.location.href = "/user"))

  const userData = data as User
  return (
    <>
      <IconButton
        aria-label="edit"
        size="small"
        color="error"
        id="btn-edit"
        disabled={userData.roleType === RoleType.Admin}
        onClick={() => navigate({ to: "/user/$id", params: { id: data.id } })}
      >
        <EditIcon />
      </IconButton>
      <IconButton
        aria-label="delete"
        size="small"
        color="error"
        id="btn-delete"
        disabled={userData.roleType === RoleType.Admin}
        onClick={(event) => {
          event.stopPropagation()
          setOpenDisableConfirmMod(!openDisableConfirmMod)
        }}
      >
        <HighlightOffIcon />
      </IconButton>
      <ConfirmModal
        open={openDisableConfirmMod}
        message="Do you want to disable this user?"
        title="Are you sure?"
        buttonOkLabel="Disable"
        buttonCloseLabel="Cancel"
        onOk={() => handleDisableUser(data.id)}
        onClose={() => setOpenDisableConfirmMod(false)}
      />
    </>
  )
}
