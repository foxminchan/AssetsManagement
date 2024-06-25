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
  buttonOkDisabled?: boolean
  buttonCloseShow?: boolean
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
  buttonOkDisabled,
  buttonCloseShow = true,
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
          className="disabled:!hover:bg-red-800 !bg-red-500 !text-white disabled:!bg-red-700 disabled:!text-gray-300"
          variant="contained"
          disabled={buttonOkDisabled ?? false}
        >
          {buttonOkLabel ?? "Save changes"}
        </Button>
        {buttonCloseShow && (
          <Button
            id="btn-input-modal-cancel"
            onClick={() => onClose?.()}
            variant="outlined"
            className="!border-black !text-black"
          >
            {buttonCloseLabel ?? "Cancel"}
          </Button>
        )}
      </DialogActions>
    </Dialog>
  )
}
