import { useEffect } from "react"
import AssignmentForm from "@components/forms/assignment-form"
import { ViewUpdateAssignmentRequest } from "@features/assignments/assignment.type"
import useGetAssignment from "@features/assignments/useGetAssignment"
import { CircularProgress, Container, Typography } from "@mui/material"
import { useNavigate, useParams } from "@tanstack/react-router"
import { format } from "date-fns"

export default function EditAssignment() {
  const params = useParams({ from: "/_authenticated/assignment/$id" })
  const { data, isError } = useGetAssignment(params?.id)
  const navigate = useNavigate()

  useEffect(() => {
    if (isError) {
      navigate({
        to: "/assignment",
      })
    }
  }, [isError])

  const formattedData = data
    ? {
        ...data,
        assignedDate: format(
          new Date(data.assignedDate),
          "yyyy-MM-dd"
        ) as unknown as Date,
        assetId: data.assetName,
        userId: data.assignedTo,
        assetName: data.assetId,
        userName: data.userId,
      }
    : null

  return (
    <Container>
      <Typography className="!text-lg !font-bold text-red-500">
        Edit Assignment
      </Typography>
      {formattedData ? (
        <AssignmentForm
          initialData={formattedData as ViewUpdateAssignmentRequest}
        />
      ) : (
        <CircularProgress className="!text-red-500" />
      )}
    </Container>
  )
}
