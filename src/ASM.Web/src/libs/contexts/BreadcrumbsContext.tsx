import { createContext, ReactNode, useMemo, useState } from "react"

import { BreadcrumbItem } from "@/types/data"

export const BreadcrumbsContext = createContext<BreadcrumbsContextProps | null>(
  null
)

type BreadcrumbsProviderProps = {
  children: ReactNode
}

type BreadcrumbsContextProps = {
  breadcrumbs: BreadcrumbItem[]
  setBreadcrumbs: React.Dispatch<React.SetStateAction<BreadcrumbItem[]>>
}

export const BreadcrumbsProvider = ({ children }: BreadcrumbsProviderProps) => {
  const [breadcrumbs, setBreadcrumbs] = useState<BreadcrumbItem[]>([
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
