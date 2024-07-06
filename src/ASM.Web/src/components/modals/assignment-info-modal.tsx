import { State } from "@features/assignments/assignment.type"
import useGetAssignment from "@features/assignments/useGetAssignment"
import {
  assignmentFieldLabels,
  assignmentFieldOrder,
} from "@libs/constants/field"
import { Container, CssBaseline, Grid, Typography } from "@mui/material"
import { format } from "date-fns"
import { match } from "ts-pattern"

import MessageModal from "./message-modal"

type Props = {
  id: string
  open: boolean
  title?: string
  onClose: () => void
}

export default function AssignmentInfoModal({
  id,
  onClose,
  open,
  title,
}: Readonly<Props>) {
  const assignment = useGetAssignment(id)

  if (!assignment?.data) return null

  return (
    <MessageModal open={open} onClose={onClose} message="" title={title}>
      <CssBaseline />
      <Grid container>
        <Container className="!flex flex-col">
          {assignmentFieldOrder.map((key) => {
            if (key === "id") return null

            const value = assignment.data?.[key]

            const formattedValue = match(key)
              .with("assignedDate", () =>
                format(
                  typeof value === "string" || typeof value === "number"
                    ? new Date(value)
                    : new Date(),
                  "dd/MM/yyyy"
                )
              )
              .with("state", () =>
                match(value)
                  .with(
                    State.RequestForReturning,
                    () => "Request for returning"
                  )
                  .with(
                    State.WaitingForAcceptance,
                    () => "Waiting For Acceptance"
                  )
                  .with(State.Accepted, () => "Accepted")
                  .otherwise(() => value)
              )
              .otherwise(() => value)

            return (
              <Grid container key={key}>
                <Grid xs={4} item>
                  {assignmentFieldLabels[key]}
                </Grid>
                <Grid xs item>
                  <Typography>{String(formattedValue)}</Typography>
                </Grid>
              </Grid>
            )
          })}
        </Container>
      </Grid>
    </MessageModal>
  )
}
