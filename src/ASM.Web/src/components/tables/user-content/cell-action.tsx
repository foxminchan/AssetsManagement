import { FC } from "react"
import { User } from "@features/users/user.type"
import { selectedRowUser } from "@libs/jotai/assignmentAtom"
import CheckCircleOutlineIcon from "@mui/icons-material/CheckCircleOutline"
import RadioButtonUncheckedIcon from "@mui/icons-material/RadioButtonUnchecked"
import { Checkbox } from "@mui/material"
import { useAtom } from "jotai"

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
  const [selectedRow, setSelectedRow] = useAtom(selectedRowUser)

  const handleSelectRow = (id: string, name: string) => {
    setSelectedRow({ id, name })
    console.log(selectedRow)
  }
  const handleClick = () => {
    onSelectRow(data.id)
    handleSelectRow(data.id, data.fullName)
  }

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
