import { FC } from "react"
import { Asset, AssetState } from "@features/assets/asset.type"
import EditIcon from "@mui/icons-material/Edit"
import HighlightOffIcon from "@mui/icons-material/HighlightOff"
import { IconButton } from "@mui/material"
import { useNavigate } from "@tanstack/react-router"

import { BaseEntity } from "@/types/data"

type AssetRowActionProps = {
  data: BaseEntity
  setOpen: (id: string) => void
}

export const AssetRowAction: FC<AssetRowActionProps> = ({ data, setOpen }) => {
  const navigate = useNavigate({ from: "/asset" })
  const assetData = data as Asset

  return (
    <>
      <IconButton
        aria-label="edit"
        size="small"
        color="error"
        id="btn-edit"
        disabled={assetData.state === AssetState.Assigned}
        onClick={() =>
          navigate({ to: "/asset/$id", params: { id: assetData.id } })
        }
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
          setOpen(assetData.id)
        }}
      >
        <HighlightOffIcon />
      </IconButton>
    </>
  )
}
