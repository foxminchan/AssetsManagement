import { Dispatch, SetStateAction } from "react"

export const useShowHint = (setter: Dispatch<SetStateAction<boolean>>) => {
  const handleMouseDown = () => {
    setter(true)
  }

  const handleMouseUp = () => {
    setter(false)
  }

  return {
    onMouseDown: handleMouseDown,
    onMouseUp: handleMouseUp,
    onMouseLeave: handleMouseUp,
  }
}
