import { State } from "@features/assignments/assignment.type"
import useGetOwnAssignment from "@features/assignments/useGetOwnAssignment"
import {
  assignmentFieldLabels,
  assignmentFieldOrder,
} from "@libs/constants/field"
import {
  CircularProgress,
  Container,
  CssBaseline,
  Grid,
  Typography,
} from "@mui/material"
import { format } from "date-fns"
import { match } from "ts-pattern"

import MessageModal from "./message-modal"

type Props = {
  id: string
  open: boolean
  title?: string
  onClose: () => void
}

export default function OwnAssignmentModal({
  id,
  onClose,
  open,
  title,
}: Readonly<Props>) {
  const assignment = useGetOwnAssignment(id)

  return (
    <MessageModal open={open} onClose={onClose} message="" title={title}>
      <CssBaseline />
      <Container className="!flex flex-col">
        {assignment.isLoading ? (
          <CircularProgress
            className="!m-auto !text-red-500"
            size={24}
            color="inherit"
          />
        ) : (
          assignmentFieldOrder.map((key) => {
            if (key === "id") return null

            const value = assignment.data?.[key]

            const formattedValue = match(key)
              .with("assignedDate", () =>
                format(new Date(value || ""), "dd/MM/yyyy")
              )
              .with("state", () =>
                match(value)
                  .with(
                    State.RequestForReturning,
                    () => "Request for returning"
                  )
                  .with(
                    State.WaitingForAcceptance,
                    () => "Waiting for acceptance"
                  )
                  .otherwise(() => State.Accepted)
              )
              .otherwise(() => value)

            return (
              <Grid container key={key} rowSpacing={3}>
                <Grid xs={4} item>
                  {assignmentFieldLabels[key]}
                </Grid>
                <Grid xs item>
                  <Typography>{String(formattedValue)}</Typography>
                </Grid>
              </Grid>
            )
          })
        )}
      </Container>
    </MessageModal>
  )
}
