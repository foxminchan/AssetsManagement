import { FC } from "react"
import { Asset } from "@features/assets/asset.type"
import { selectedRowAsset } from "@libs/jotai/assetAtom"
import CheckCircleOutlineIcon from "@mui/icons-material/CheckCircleOutline"
import RadioButtonUncheckedIcon from "@mui/icons-material/RadioButtonUnchecked"
import { Checkbox } from "@mui/material"
import { useAtom } from "jotai"

type CellActionProps = {
  data: Asset
  isSelected: boolean
  onSelectRow: (id: string) => void
}

export const CellAction: FC<CellActionProps> = ({
  data,
  isSelected,
  onSelectRow,
}) => {
  const [selectedRow, setSelectedRow] = useAtom(selectedRowAsset)

  const handleSelectRow = (id: string, name: string) => {
    setSelectedRow({ id, name })
  }
  const handleClick = (event: React.ChangeEvent<HTMLInputElement>) => {
    const checked = event.target.checked
    if (checked) {
      onSelectRow(data.id)
      handleSelectRow(data.id, data.name)
      console.log(selectedRow)
    } else {
      console.log("Un checked")
    }
  }

  return (
    <Checkbox
      aria-label="choose"
      size="small"
      color="success"
      id="btn-choose"
      checked={isSelected}
      icon={<RadioButtonUncheckedIcon />}
      checkedIcon={<CheckCircleOutlineIcon className="text-red-500" />}
      onChange={handleClick}
    />
  )
}
