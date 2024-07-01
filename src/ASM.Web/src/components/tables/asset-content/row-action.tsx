import { FC, useEffect } from "react"
import { Asset } from "@features/assets/asset.type"
import { selectedRowAsset } from "@libs/jotai/assetAtom"
import CheckCircleOutlineIcon from "@mui/icons-material/CheckCircleOutline"
import RadioButtonUncheckedIcon from "@mui/icons-material/RadioButtonUnchecked"
import { Checkbox } from "@mui/material"
import { useSetAtom } from "jotai"

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
  const setSelectedRow = useSetAtom(selectedRowAsset)

  const handleSelectRow = (id: string, name: string) => {
    setSelectedRow({ id, name })
  }
  const handleClick = (event: React.ChangeEvent<HTMLInputElement>) => {
    const checked = event.target.checked
    if (checked) {
      onSelectRow(data.id)
      handleSelectRow(data.id, data.name)
    }
  }

  useEffect(() => {
    if (isSelected) {
      handleClick({
        target: { checked: true },
      } as React.ChangeEvent<HTMLInputElement>)
    }
  }, [isSelected])

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
