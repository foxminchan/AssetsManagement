import { Dispatch, FC } from "react"
import { Assignment, State } from "@features/assignments/assignment.type"
import EditIcon from "@mui/icons-material/Edit"
import HighlightOffIcon from "@mui/icons-material/HighlightOff"
import RestartAltIcon from "@mui/icons-material/RestartAlt"
import { IconButton } from "@mui/material"
import { useNavigate } from "@tanstack/react-router"
import { SetStateAction } from "jotai"

import { BaseEntity } from "@/types/data"

type AssignmentRowActionProps = {
  data: BaseEntity
  setOpenDisableConfirmMod: Dispatch<SetStateAction<boolean>>
  setOpenReturnConfirmMod: Dispatch<SetStateAction<boolean>>
  id: Dispatch<SetStateAction<string>>
}

export const AssignmentRowAction: FC<AssignmentRowActionProps> = ({
  data,
  setOpenDisableConfirmMod,
  setOpenReturnConfirmMod,
  id: setId,
}) => {
  const assignmentData = data as Assignment
  const navigate = useNavigate({ from: "/assignment" })
  return (
    <>
      <IconButton
        aria-label="edit"
        size="small"
        color="error"
        id="btn-edit"
        disabled={assignmentData.state === State.Accepted}
        onClick={() =>
          navigate({ to: "/assignment/$id", params: { id: data.id } })
        }
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
        aria-label="return"
        size="small"
        color="error"
        id="btn-return"
        disabled={assignmentData.state !== State.Accepted}
        onClick={(event) => {
          event.stopPropagation()
          setOpenReturnConfirmMod(true)
          setId(assignmentData.id)
        }}
      >
        <RestartAltIcon />
      </IconButton>
    </>
  )
}
