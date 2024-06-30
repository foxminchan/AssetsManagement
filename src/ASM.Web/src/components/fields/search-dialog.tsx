import SearchIcon from "@mui/icons-material/Search"
import { IconButton, InputAdornment, OutlinedInput } from "@mui/material"
import { useRouter, useSearch } from "@tanstack/react-router"

export default function SearchInput() {
  const router = useRouter()
  const params = useSearch({ strict: false })

  const handleOnClick = async (keyword: string) => {
    await router.navigate({
      search: {
        ...params,
        search: keyword,
      },
    })
  }

  return (
    <OutlinedInput
      id="txt-search"
      size="small"
      endAdornment={
        <InputAdornment position="end">
          <IconButton
            id="btn-search"
            aria-label="toggle password visibility"
            onClick={() =>
              handleOnClick(
                (document.getElementById("txt-search") as HTMLInputElement)
                  .value
              )
            }
            edge="end"
          >
            <SearchIcon />
          </IconButton>
        </InputAdornment>
      }
    />
  )
}
