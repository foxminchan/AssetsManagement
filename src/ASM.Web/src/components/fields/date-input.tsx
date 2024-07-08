import { FormControl } from "@mui/material"
import { DatePicker } from "@mui/x-date-pickers"

type Props = {
  label?: string
  setSelected: (value: Date) => void
}

export default function FilterDate({ label, setSelected }: Readonly<Props>) {
  const today = new Date(Date.now())

  const handleChange = (newDate: Date) => {
    setSelected(newDate)
  }

  return (
    <FormControl>
      <DatePicker
        label={label}
        maxDate={today}
        format="dd/MM/yyyy"
        onChange={(value) => {
          value && handleChange(value)
        }}
        slotProps={{
          textField: {
            size: "small",
            id: "dpk-date-of-birth",
            disabled: true,
          },
          openPickerButton: {
            id: "btn-date-of-birth",
          },
        }}
        sx={{
          width: "180px",
        }}
      />
    </FormControl>
  )
}
