import Button from "@mui/material/Button"
import Dialog from "@mui/material/Dialog"
import DialogActions from "@mui/material/DialogActions"
import DialogContent from "@mui/material/DialogContent"
import DialogTitle from "@mui/material/DialogTitle"
import Typography from "@mui/material/Typography"

type AlertModalProps = {
  open: boolean
  title?: string
  onClose?: () => void
  message: string
}

export default function AlertModal({
  open,
  title,
  message,
  onClose,
}: Readonly<AlertModalProps>) {
  return (
    <Dialog
      open={open}
      className="relative"
      maxWidth="xs"
      fullWidth
      aria-labelledby="alert-dialog-title"
      aria-describedby="alert-dialog-description"
    >
      <DialogTitle
        id="alert-dialog-title"
        className="flex items-center justify-between bg-gray-100 text-2xl font-bold text-red-500"
      >
        {title ?? "Message"}
      </DialogTitle>
      <DialogContent dividers className="flex flex-col p-4">
        <Typography
          id="alert-dialog-message"
          className="break-words break-all text-gray-700"
        >
          {message}
        </Typography>
      </DialogContent>
      <DialogActions>
        <Button
          id="btn-confirm-dialog-close"
          onClick={() => onClose?.()}
          variant="outlined"
          className="!border-black !text-black"
        >
          Close
        </Button>
      </DialogActions>
    </Dialog>
  )
}
