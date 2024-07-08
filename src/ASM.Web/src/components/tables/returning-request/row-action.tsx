import { Dispatch, FC, SetStateAction } from "react"
import {
  ReturningRequest,
  ReturningRequestState,
} from "@features/returning-requests/returning-request.type"
import CloseIcon from "@mui/icons-material/Close"
import DoneIcon from "@mui/icons-material/Done"
import { IconButton } from "@mui/material"

import { BaseEntity } from "@/types/data"

type ReturningRequestRowActionProps = {
  data: BaseEntity
  setCompleteRequestModalOpen: Dispatch<SetStateAction<boolean>>
  setCancelRequestModalOpen: Dispatch<SetStateAction<boolean>>
  setSelectedReturningRequestId: Dispatch<SetStateAction<string>>
}

export const ReturningRequestRowAction: FC<ReturningRequestRowActionProps> = ({
  data,
  setCompleteRequestModalOpen,
  setCancelRequestModalOpen,
  setSelectedReturningRequestId,
}) => {
  const requestData = data as ReturningRequest

  return (
    <>
      <IconButton
        aria-label="done"
        size="small"
        color="error"
        id="btn-complete-request"
        disabled={requestData.state === ReturningRequestState.Completed}
        onClick={(event) => {
          event.stopPropagation()
          setCompleteRequestModalOpen(true)
          setSelectedReturningRequestId(requestData.id)
        }}
      >
        <DoneIcon />
      </IconButton>
      <IconButton
        aria-label="close"
        size="small"
        color="inherit"
        id="btn-cancel-request"
        disabled={requestData.state === ReturningRequestState.Completed}
        onClick={(event) => {
          event.stopPropagation()
          setCancelRequestModalOpen(true)
          setSelectedReturningRequestId(requestData.id)
        }}
      >
        <CloseIcon />
      </IconButton>
    </>
  )
}
