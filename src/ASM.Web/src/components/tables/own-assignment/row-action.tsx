import { Dispatch, FC, SetStateAction } from "react"
import { Assignment, State } from "@features/assignments/assignment.type"
import { Action } from "@libs/constants/action"
import CloseIcon from "@mui/icons-material/Close"
import DoneIcon from "@mui/icons-material/Done"
import RefreshIcon from "@mui/icons-material/Refresh"
import { IconButton } from "@mui/material"
import { match } from "ts-pattern"

import { BaseEntity } from "@/types/data"

type OwnAssignmentRowActionProps = {
  data: BaseEntity
  action: Dispatch<SetStateAction<string>>
  openModal: Dispatch<SetStateAction<boolean>>
  id: Dispatch<SetStateAction<string>>
}

export const OwnAssignmentRowAction: FC<OwnAssignmentRowActionProps> = ({
  data,
  action: setAction,
  openModal: setOpenDisableConfirmMod,
  id: setId,
}) => {
  const assignmentData = data as Assignment
  const state = assignmentData.state

  const { isDoneDisabled, isCloseDisabled, isRefreshDisabled } = match(state)
    .with(State.RequestForReturning, () => ({
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
        id="btn-accept"
        disabled={isDoneDisabled}
        onClick={(event) => {
          event.stopPropagation()
          setAction(Action.Accept)
          setOpenDisableConfirmMod(true)
          setId(assignmentData.id)
        }}
      >
        <DoneIcon />
      </IconButton>
      <IconButton
        aria-label="close"
        size="small"
        color="inherit"
        id="btn-delete"
        disabled={isCloseDisabled}
        onClick={(event) => {
          event.stopPropagation()
          setAction(Action.Delete)
          setOpenDisableConfirmMod(true)
          setId(assignmentData.id)
        }}
      >
        <CloseIcon />
      </IconButton>
      <IconButton
        aria-label="refresh"
        size="small"
        color="info"
        id="btn-return"
        disabled={isRefreshDisabled}
        onClick={(event) => {
          event.stopPropagation()
          setAction(Action.Return)
          setOpenDisableConfirmMod(true)
          setId(assignmentData.id)
        }}
      >
        <RefreshIcon />
      </IconButton>
    </>
  )
}
