import { atom } from "jotai"

type AssetChoose = {
  id: string
  name: string
}

const initialUserChoose: AssetChoose = {
  id: "",
  name: "",
}

export const selectedRowAsset = atom<AssetChoose | null>(initialUserChoose)
