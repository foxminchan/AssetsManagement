import { Dispatch, FC } from "react"
import { Assignment, State } from "@features/assignments/assignment.type"
import EditIcon from "@mui/icons-material/Edit"
import HighlightOffIcon from "@mui/icons-material/HighlightOff"
import RestartAltIcon from "@mui/icons-material/RestartAlt"
import { IconButton } from "@mui/material"
import { SetStateAction } from "jotai"

import { BaseEntity } from "@/types/data"

type AssignmentRowActionProps = {
  data: BaseEntity
  openModal: Dispatch<SetStateAction<boolean>>
  id: Dispatch<SetStateAction<string>>
}

export const AssignmentRowAction: FC<AssignmentRowActionProps> = ({
  data,
  openModal: setOpenDisableConfirmMod,
  id: setId,
}) => {
  const assignmentData = data as Assignment

  return (
    <>
      <IconButton
        aria-label="edit"
        size="small"
        color="error"
        id="btn-edit"
        disabled={assignmentData.state === State.Accepted}
      >
        <EditIcon />
      </IconButton>
      <IconButton
        aria-label="delete"
        size="small"
        color="error"
        id="btn-delete"
        disabled={assignmentData.state === State.Accepted}
        onClick={(event) => {
          event.stopPropagation()
          setOpenDisableConfirmMod(true)
          setId(assignmentData.id)
        }}
      >
        <HighlightOffIcon />
      </IconButton>
      <IconButton
        aria-label="delete"
        size="small"
        color="error"
        id="btn-delete"
        disabled={assignmentData.state === State.WaitingForAcceptance}
      >
        <RestartAltIcon />
      </IconButton>
    </>
  )
}
