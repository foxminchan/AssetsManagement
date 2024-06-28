import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Typography,
} from "@mui/material"

type ErrorModalProps = {
  open: boolean
  actionName: string
  onOK?: () => void
}

export default function ErrorModal({
  open,
  actionName,
  onOK,
}: Readonly<ErrorModalProps>) {
  return (
    <Dialog
      open={open}
      className="relative"
      maxWidth="sm"
      fullWidth
      aria-labelledby="message-dialog-title"
      aria-describedby="message-dialog-description"
    >
      <DialogTitle
        id="message-dialog-title"
        className="flex items-center justify-between bg-gray-100 text-2xl font-bold text-red-500"
      >
        Error
      </DialogTitle>
      <DialogContent dividers className="flex flex-col p-4">
        <Typography
          id="alert-dialog-message"
          className="break-words break-all text-gray-700"
        >
          An error has occurred with the previous operation
        </Typography>
      </DialogContent>
      <DialogActions>
        <Button
          id="btn-confirm-dialog-close"
          onClick={() => onOK?.()}
          variant="outlined"
          className="!bg-red-500 !text-white"
        >
          {actionName}
        </Button>
      </DialogActions>
    </Dialog>
  )
}
