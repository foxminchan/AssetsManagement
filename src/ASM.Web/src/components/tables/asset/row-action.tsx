import { FC, useState } from "react"
import ConfirmModal from "@components/modals/confirm-modal"
import { Asset, AssetState } from "@features/assets/asset.type"
import EditIcon from "@mui/icons-material/Edit"
import HighlightOffIcon from "@mui/icons-material/HighlightOff"
import { IconButton } from "@mui/material"

import { BaseEntity } from "@/types/data"

type AssetRowActionProps = {
  data: BaseEntity
}

export const AssetRowAction: FC<AssetRowActionProps> = ({ data }) => {
  const [openDisableConfirmMod, setOpenDisableConfirmMod] = useState(false)
  const assetData = data as Asset

  return (
    <>
      <IconButton
        aria-label="edit"
        size="small"
        color="error"
        id="btn-edit"
        disabled={assetData.state === AssetState.Assigned}
      >
        <EditIcon />
      </IconButton>
      <IconButton
        aria-label="delete"
        size="small"
        color="error"
        id="btn-delete"
        disabled={assetData.state === AssetState.Assigned}
        onClick={(event) => {
          event.stopPropagation()
          setOpenDisableConfirmMod(!openDisableConfirmMod)
        }}
      >
        <HighlightOffIcon />
      </IconButton>
      <ConfirmModal
        open={openDisableConfirmMod}
        message="Do you want to disable this user?"
        title="Are you sure?"
        buttonOkLabel="Disable"
        buttonCloseLabel="Cancel"
        onClose={() => setOpenDisableConfirmMod(false)}
      />
    </>
  )
}
