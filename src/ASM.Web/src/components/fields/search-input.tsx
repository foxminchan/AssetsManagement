import { Dispatch, SetStateAction } from "react"
import SearchIcon from "@mui/icons-material/Search"
import { IconButton, InputAdornment, OutlinedInput } from "@mui/material"

type SearchInputProps = {
  keyword: string
  setKeyword: Dispatch<SetStateAction<string>>
  onClick: React.MouseEventHandler<HTMLButtonElement>
}

export default function SearchInput({
  keyword,
  setKeyword,
  onClick,
}: Readonly<SearchInputProps>) {
  return (
    <OutlinedInput
      id="txt-search"
      size="small"
      value={keyword}
      onChange={(e) => setKeyword(e.target.value)}
      endAdornment={
        <InputAdornment position="end">
          <IconButton
            id="btn-search"
            aria-label="toggle password visibility"
            onClick={onClick}
            edge="end"
          >
            <SearchIcon />
          </IconButton>
        </InputAdornment>
      }
    />
  )
}
