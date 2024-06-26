import { createContext, ReactNode, useMemo, useState } from "react"

import { RouteItem } from "@/types/data"

export const BreadcrumbsContext = createContext<BreadcrumbsContextProps | null>(
  null
)

type BreadcrumbsProviderProps = {
  children: ReactNode
}

type BreadcrumbsContextProps = {
  breadcrumbs: RouteItem[]
  setBreadcrumbs: React.Dispatch<React.SetStateAction<RouteItem[]>>
}

export const BreadcrumbsProvider = ({ children }: BreadcrumbsProviderProps) => {
  const [breadcrumbs, setBreadcrumbs] = useState<RouteItem[]>([
    { label: "Home", to: "/" },
  ])

  return useMemo(
    () => (
      <BreadcrumbsContext.Provider
        value={{
          breadcrumbs,
          setBreadcrumbs,
        }}
      >
        {children}
      </BreadcrumbsContext.Provider>
    ),
    [breadcrumbs, setBreadcrumbs]
  )
}
