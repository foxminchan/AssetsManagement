import { FC } from "react"
import { RoleType, User } from "@features/users/user.type"
import EditIcon from "@mui/icons-material/Edit"
import HighlightOffIcon from "@mui/icons-material/HighlightOff"
import { IconButton } from "@mui/material"
import { useNavigate } from "@tanstack/react-router"

type CellActionProps = {
  data: User
}

export const CellAction: FC<CellActionProps> = ({ data }) => {
  const navigate = useNavigate({ from: "/user/$id" })
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
        onClick={() => alert("Delete")}
      >
        <HighlightOffIcon />
      </IconButton>
    </>
  )
}
