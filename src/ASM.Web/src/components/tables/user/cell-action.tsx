import { FC, useState } from "react"
import useDeleteUser from "@/features/users/useDeleteUser"
import { RoleType, User } from "@features/users/user.type"
import EditIcon from "@mui/icons-material/Edit"
import HighlightOffIcon from "@mui/icons-material/HighlightOff"
import { IconButton } from "@mui/material"
import { useNavigate } from "@tanstack/react-router"

import ConfirmModal from "@/components/modals/confirm-modal"

type CellActionProps = {
  data: User
}

export const CellAction: FC<CellActionProps> = ({ data }) => {
  const navigate = useNavigate({ from: "/user/$id" })
  const [openDisableConfirmMod, setOpenDisableConfirmMod] = useState(false)
  const { mutate: deleteUser } = useDeleteUser()
  const handleDisableUser = (id: string) =>
    Promise.resolve(deleteUser(id)).then(() => (window.location.href = "/user"))

  return (
    <>
      <IconButton
        aria-label="edit"
        size="small"
        color="error"
        id="btn-edit"
        disabled={data.roleType === RoleType.Admin}
        onClick={() => navigate({ to: "/user/$id", params: { id: data.id } })}
      >
        <EditIcon />
      </IconButton>
      <IconButton
        aria-label="delete"
        size="small"
        color="error"
        id="btn-delete"
        disabled={data.roleType === RoleType.Admin}
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
