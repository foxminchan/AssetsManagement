import { atom } from "jotai"

type UserChoose = {
  id: string
  name: string
}

const initialUserChoose: UserChoose = {
  id: "",
  name: "",
}

export const assignmentAtoms = atom("")
export const selectedRowUser = atom<UserChoose | null>(initialUserChoose)
export const submitUser = atom<UserChoose | null>(initialUserChoose)
