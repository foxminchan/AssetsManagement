import { useEffect } from "react"
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
import { useRouter, useSearch } from "@tanstack/react-router"

type Props = {
  values: string[]
  label?: string
  isMultiple: boolean
  selectedType: string | string[]
  setSelectedType: (value: string | string[]) => void
}

export default function FilterInput({
  values: types,
  label,
  selectedType,
  isMultiple,
  setSelectedType,
}: Readonly<Props>) {
  const router = useRouter()
  const params = useSearch({ strict: false })

  useEffect(() => {
    ;(async () =>
      await router.navigate({
        search: {
          ...params,
          roleType: selectedType,
        },
      }))()
  }, [selectedType])

  const handleChange = (event: SelectChangeEvent<typeof selectedType>) => {
    setSelectedType(event.target.value)
  }

  return (
    <FormControl sx={{ width: 150 }}>
      {label && <InputLabel id="label-chk-type">{label}</InputLabel>}
      <Select
        fullWidth
        size="small"
        id="chk-type"
        labelId="label-chk-type"
        multiline={isMultiple}
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
        value={selectedType}
        onChange={handleChange}
        renderValue={(selected) => selected}
      >
        {types.map((type) => (
          <MenuItem key={type} value={type}>
            <Checkbox checked={type === selectedType} />
            <ListItemText primary={type} />
          </MenuItem>
        ))}
      </Select>
    </FormControl>
  )
}
