import AssignmentForm from "@components/forms/assignment-form"
import Container from "@mui/material/Container"
import Typography from "@mui/material/Typography"

export default function CreateAssignment() {
  return (
    <Container className="!flex flex-col gap-6">
      <Typography className="!text-lg !font-bold text-red-500">
        Create New Assignment
      </Typography>
      <AssignmentForm />
    </Container>
  )
}
