import { FC } from "react"
import { Assignment, State } from "@/features/assignments/assignment.type"
import CloseIcon from "@mui/icons-material/Close"
import DoneIcon from "@mui/icons-material/Done"
import RefreshIcon from "@mui/icons-material/Refresh"
import { IconButton } from "@mui/material"
import { match } from "ts-pattern"

import { BaseEntity } from "@/types/data"

type OwnAssignmentRowActionProps = {
  data: BaseEntity
}

export const OwnAssignmentRowAction: FC<OwnAssignmentRowActionProps> = ({
  data,
}) => {
  const assignmentData = data as Assignment
  const state = assignmentData.state

  const { isDoneDisabled, isCloseDisabled, isRefreshDisabled } = match(state)
    .with(State.IsRequested, () => ({
      isDoneDisabled: true,
      isCloseDisabled: true,
      isRefreshDisabled: true,
    }))
    .with(State.Accepted, () => ({
      isDoneDisabled: true,
      isCloseDisabled: true,
      isRefreshDisabled: false,
    }))
    .otherwise(() => ({
      isDoneDisabled: false,
      isCloseDisabled: false,
      isRefreshDisabled: true,
    }))

  return (
    <>
      <IconButton
        aria-label="done"
        size="small"
        color="error"
        id="btn-done"
        disabled={isDoneDisabled}
      >
        <DoneIcon />
      </IconButton>
      <IconButton
        aria-label="close"
        size="small"
        color="inherit"
        id="btn-close"
        disabled={isCloseDisabled}
      >
        <CloseIcon />
      </IconButton>
      <IconButton
        aria-label="refresh"
        size="small"
        color="primary"
        id="btn-refresh"
        disabled={isRefreshDisabled}
      >
        <RefreshIcon />
      </IconButton>
    </>
  )
}
