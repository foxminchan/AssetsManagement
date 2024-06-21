import React from "react"
import { Container } from "@mui/material"
import Button from "@mui/material/Button"
import Dialog from "@mui/material/Dialog"
import DialogActions from "@mui/material/DialogActions"
import DialogContent from "@mui/material/DialogContent"
import DialogTitle from "@mui/material/DialogTitle"
import Typography from "@mui/material/Typography"

type InputModalProps = {
  open: boolean
  message?: string
  title?: string
  onOk?: () => void
  onClose?: () => void
  buttonOkLabel?: string
  buttonCloseLabel?: string
  children?: React.ReactNode
}

export default function InputModal({
  open,
  onClose,
  onOk,
  title,
  message,
  buttonOkLabel,
  buttonCloseLabel,
  children,
}: Readonly<InputModalProps>) {
  return (
    <Dialog
      aria-labelledby="input-modal-title"
      aria-describedby="input-modal-description"
      className="relative"
      fullWidth
      maxWidth="xs"
      open={open}
    >
      <DialogTitle
        id="input-dialog-title"
        className="rounded-t-lg bg-gray-100 px-6 py-4 text-2xl font-bold text-red-500"
      >
        {title ?? "Input modal"}
      </DialogTitle>

      <DialogContent dividers>
        {message && (
          <Container>
            <Typography className="break-words break-all text-gray-700">
              {message}
            </Typography>
          </Container>
        )}
        <Container className="mt-3">{children}</Container>
      </DialogContent>

      <DialogActions>
        <Button
          id="btn-input-modal-save"
          onClick={() => onOk?.()}
          className="!bg-red-500 !text-white"
          variant="contained"
        >
          {buttonOkLabel ?? "Save changes"}
        </Button>

        <Button
          id="btn-input-modal-cancel"
          onClick={() => onClose?.()}
          variant="outlined"
          className="!border-gray-400 !text-gray-400"
        >
          {buttonCloseLabel ?? "Cancel"}
        </Button>
      </DialogActions>
    </Dialog>
  )
}
