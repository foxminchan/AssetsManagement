import { FC } from "react"
import { Assignment, State } from "@features/assignments/assignment.type"
import EditIcon from "@mui/icons-material/Edit"
import HighlightOffIcon from "@mui/icons-material/HighlightOff"
import RestartAltIcon from "@mui/icons-material/RestartAlt"
import { IconButton } from "@mui/material"

import { BaseEntity } from "@/types/data"

type AssignmentRowActionProps = {
  data: BaseEntity
}

export const AssignmentRowAction: FC<AssignmentRowActionProps> = ({ data }) => {
  const userData = data as Assignment
  return (
    <>
      <IconButton
        aria-label="edit"
        size="small"
        color="error"
        id="btn-edit"
        disabled={userData.state === State.Accepted}
      >
        <EditIcon />
      </IconButton>
      <IconButton
        aria-label="delete"
        size="small"
        color="error"
        id="btn-delete"
        disabled={userData.state === State.Accepted}
      >
        <HighlightOffIcon />
      </IconButton>
      <IconButton
        aria-label="delete"
        size="small"
        color="error"
        id="btn-delete"
        disabled={userData.state === State.WaitingForAcceptance}
      >
        <RestartAltIcon />
      </IconButton>
    </>
  )
}
