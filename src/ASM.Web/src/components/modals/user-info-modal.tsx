import useGetUser from "@features/users/useGetUser"
import { Location } from "@features/users/user.type"
import { fieldLabels, fieldOrder } from "@libs/constants/field"
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

export default function UserInfoModal({
  id,
  onClose,
  open,
  title,
}: Readonly<Props>) {
  const user = useGetUser(id)

  if (!user?.data) return null

  return (
    <MessageModal open={open} onClose={onClose} message="" title={title}>
      <CssBaseline />
      <Grid container>
        <Container className="!flex flex-col">
          {fieldOrder.map((key) => {
            if (key === "id") return null

            const value = user.data?.[key]

            const formattedValue = match(key)
              .with("dob", "joinedDate", () =>
                format(new Date(value || ""), "dd/MM/yyyy")
              )
              .with("location", () =>
                match(value)
                  .with(Location.HoChiMinh, () => "HCM")
                  .with(Location.Hanoi, () => "HN")
                  .with(Location.DaNang, () => "DN")
                  .otherwise(() => value)
              )
              .otherwise(() => value)

            return (
              <Grid container key={key} rowSpacing={3}>
                <Grid xs={4} item>
                  {fieldLabels[key]}
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
