import { AccountStatus, AuthUser } from "@/features/auth/auth.type"
import { atom } from "jotai"

const initialUser: AuthUser = {
  id: "",
  accountStatus: AccountStatus.None,
  claims: [],
}

export const userInfo = atom<AuthUser | null>(initialUser)
