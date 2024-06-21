import CloseIcon from "@mui/icons-material/Close"
import Dialog from "@mui/material/Dialog"
import DialogContent from "@mui/material/DialogContent"
import DialogTitle from "@mui/material/DialogTitle"
import IconButton from "@mui/material/IconButton"
import Typography from "@mui/material/Typography"

type MessageModalProps = {
  open: boolean
  title?: string
  onClose?: () => void
  message: string
}

export default function MessageModal({
  open,
  title,
  message,
  onClose,
}: Readonly<MessageModalProps>) {
  return (
    <Dialog
      open={open}
      className="relative"
      maxWidth="xs"
      fullWidth
      aria-labelledby="message-dialog-title"
      aria-describedby="message-dialog-description"
    >
      <DialogTitle
        id="message-dialog-title"
        className="flex items-center justify-between bg-gray-100 text-2xl font-bold text-red-500"
      >
        {title ?? "Message"}
        <IconButton
          id="btn-message-modal-close"
          onClick={() => onClose?.()}
          sx={{
            position: "absolute",
            right: 8,
            top: 8,
            color: (theme) => theme.palette.grey[500],
          }}
        >
          <CloseIcon />
        </IconButton>
      </DialogTitle>
      <DialogContent dividers className="flex flex-col p-4">
        <Typography
          id="message-dialog-message"
          className="break-words break-all text-gray-700"
        >
          {message}
        </Typography>
      </DialogContent>
    </Dialog>
  )
}
