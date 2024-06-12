export type MenuItem = {
  label: string
  to: string
}

export type BreadcrumbItem = {
  label: string
  to: string
}

export type OptionItem<T> = {
  label: string
  value: T
}
