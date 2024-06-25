import UserForm from "@components/forms/user-form"
import useGetUser from "@features/users/useGetUser"
import { UpdateUserRequest } from "@features/users/user.type"
import Container from "@mui/material/Container"
import Typography from "@mui/material/Typography"
import { useParams } from "@tanstack/react-router"
import { format } from "date-fns"

export default function EditUser() {
  const params = useParams({ from: "/_authenticated/user/$id" })
  const { data } = useGetUser(params?.id)

  const formattedData = data
    ? {
        ...data,
        dob: format(new Date(data.dob), "yyyy-MM-dd"),
        joinedDate: format(new Date(data.joinedDate), "yyyy-MM-dd"),
      }
    : null

  return (
    <Container className="!flex flex-col gap-6">
      <Typography className="!text-lg !font-bold text-red-500">
        Edit User
      </Typography>
      {formattedData && (
        <UserForm initialData={formattedData as UpdateUserRequest} />
      )}
    </Container>
  )
}
