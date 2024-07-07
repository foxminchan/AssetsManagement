import { FC } from "react"
import { RoleType, User } from "@features/users/user.type"
import EditIcon from "@mui/icons-material/Edit"
import HighlightOffIcon from "@mui/icons-material/HighlightOff"
import { IconButton } from "@mui/material"
import { useNavigate } from "@tanstack/react-router"

import { BaseEntity } from "@/types/data"

type UserRowActionProps = {
  data: BaseEntity
  setOpen: (id: string) => void
}

export const UserRowAction: FC<UserRowActionProps> = ({ data, setOpen }) => {
  const navigate = useNavigate({ from: "/user" })

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
        onClick={(e) => {
          e.stopPropagation()
          setOpen(data.id)
        }}
      >
        <HighlightOffIcon />
      </IconButton>
    </>
  )
}
