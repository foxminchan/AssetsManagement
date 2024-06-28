import Button from "@mui/material/Button"
import Dialog from "@mui/material/Dialog"
import DialogActions from "@mui/material/DialogActions"
import DialogContent from "@mui/material/DialogContent"
import DialogTitle from "@mui/material/DialogTitle"
import Typography from "@mui/material/Typography"

type ConfirmModalProps = {
  open: boolean
  message: string
  title?: string
  onOk?: () => void
  onClose?: () => void
  buttonOkLabel?: string
  buttonCloseLabel?: string
}

export default function ConfirmModal({
  open,
  onClose,
  onOk,
  title,
  message,
  buttonOkLabel,
  buttonCloseLabel,
}: Readonly<ConfirmModalProps>) {
  return (
    <Dialog
      aria-labelledby="confirm-modal-title"
      aria-describedby="confirm-modal-description"
      className="relative"
      maxWidth="xs"
      fullWidth
      open={open}
    >
      <DialogTitle
        id="confirm-dialog-title"
        className="rounded-t-lg bg-gray-100 px-6 py-4 text-2xl font-bold text-red-500"
      >
        {title ?? "Are you sure?"}
      </DialogTitle>
      <DialogContent id="confirm-dialog-message" dividers>
        <Typography className="!break-words text-gray-700">
          {message}
        </Typography>
      </DialogContent>
      <DialogActions>
        <Button
          id="btn-confirm-dialog-ok"
          onClick={() => onOk?.()}
          variant="contained"
          className="!bg-red-500 !text-white"
        >
          {buttonOkLabel ?? "Save changes"}
        </Button>
        <Button
          id="btn-confirm-dialog-close"
          onClick={() => onClose?.()}
          variant="outlined"
          className="!border-black !text-black"
        >
          {buttonCloseLabel ?? "Close"}
        </Button>
      </DialogActions>
    </Dialog>
  )
}
