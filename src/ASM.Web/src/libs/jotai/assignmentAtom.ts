import { atom } from "jotai"

type UserChoose = {
  id: string
  name: string
}

const initialUserChoose: UserChoose = {
  id: "",
  name: "",
}

export const selectedRowUser = atom<UserChoose | null>(initialUserChoose)
