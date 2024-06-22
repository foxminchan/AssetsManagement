export const useShowHint = (
  setter: React.Dispatch<React.SetStateAction<boolean>>
) => {
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
