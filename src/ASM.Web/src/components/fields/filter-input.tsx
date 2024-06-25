import FilterAltIcon from "@mui/icons-material/FilterAlt"
import {
  Checkbox,
  FormControl,
  InputLabel,
  ListItemText,
  MenuItem,
  OutlinedInput,
  Select,
  SelectChangeEvent,
} from "@mui/material"

type Props = {
  values: string[]
  label?: string
  multiple: boolean
  selected: string | string[]
  setSelected: (value: string | string[]) => void
}

export default function FilterInput({
  values,
  label,
  multiple,
  selected,
  setSelected,
}: Readonly<Props>) {
  const handleChange = (event: SelectChangeEvent<typeof selected>) => {
    const {
      target: { value },
    } = event

    if (multiple) {
      if (Array.isArray(value) && value.at(-1) === "All") {
        setSelected(["All"])
      } else if (Array.isArray(value) && value.at(0) === "All") {
        setSelected(value.slice(1))
      } else {
        setSelected(typeof value === "string" ? value.split(",") : value)
      }
    } else {
      setSelected(value)
    }
  }

  return (
    <FormControl className="w-full">
      {label && (
        <InputLabel id="label-chk-type" size="small">
          {label}
        </InputLabel>
      )}
      <Select
        label={label}
        fullWidth
        size="small"
        id="chk-type"
        labelId="label-chk-type"
        multiple={multiple}
        input={
          <OutlinedInput
            label={label ?? undefined}
            id="txt-filter"
            endAdornment={<FilterAltIcon />}
            inputProps={{
              "aria-label": "weight",
            }}
          />
        }
        IconComponent={() => null}
        value={selected}
        onChange={handleChange}
        renderValue={(selected) =>
          Array.isArray(selected) ? selected.join(", ") : selected
        }
      >
        {values.map((value) => (
          <MenuItem key={value} value={value}>
            <Checkbox
              checked={selected !== undefined && selected.indexOf(value) !== -1}
            />
            <ListItemText primary={value} />
          </MenuItem>
        ))}
      </Select>
    </FormControl>
  )
}
