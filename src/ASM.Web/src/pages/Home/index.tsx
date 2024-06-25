import { useContext, useEffect } from "react"
import { BreadcrumbsContext } from "@libs/contexts/BreadcrumbsContext"
import { Typography } from "@mui/material"

import { RouteItem } from "@/types/data"

const breadcrumb: RouteItem[] = [
  {
    label: "Home",
    to: "/home",
  },
]

export default function Home() {
  const context = useContext(BreadcrumbsContext)

  useEffect(() => {
    context?.setBreadcrumbs(breadcrumb)
  }, [])

  return (
    <>
      <Typography
        variant="h5"
        component="h1"
        gutterBottom
        fontWeight={500}
        className="!text-red-500"
      >
        My Assignment
      </Typography>
      <div></div>
    </>
  )
}
