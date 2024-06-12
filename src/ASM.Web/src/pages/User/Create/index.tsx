import UserForm from "@components/forms/user-form"
import Container from "@mui/material/Container"
import Typography from "@mui/material/Typography"

export default function CreateUser() {
  return (
    <Container className="!flex flex-col gap-6">
      <Typography className="!text-lg !font-bold text-red-500">
        Create User
      </Typography>
      <UserForm />
    </Container>
  )
}
