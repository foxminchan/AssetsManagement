import { FC, useEffect } from "react"
import { User } from "@features/users/user.type"
import { selectedRowUser } from "@libs/jotai/assignmentAtom"
import CheckCircleOutlineIcon from "@mui/icons-material/CheckCircleOutline"
import RadioButtonUncheckedIcon from "@mui/icons-material/RadioButtonUnchecked"
import { Checkbox } from "@mui/material"
import { useSetAtom } from "jotai"

type CellActionProps = {
  data: User
  isSelected: boolean
  onSelectRow: (id: string) => void
}

export const CellAction: FC<CellActionProps> = ({
  data,
  isSelected,
  onSelectRow,
}) => {
  const setSelectedRow = useSetAtom(selectedRowUser)

  const handleSelectRow = (id: string, name: string) => {
    setSelectedRow({ id, name })
  }

  const handleClick = (event: React.ChangeEvent<HTMLInputElement>) => {
    const checked = event.target.checked
    if (checked) {
      onSelectRow(data.id)
      handleSelectRow(data.id, data.userName)
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
      checkedIcon={<CheckCircleOutlineIcon className="text-red-400" />}
      onChange={handleClick}
    />
  )
}
