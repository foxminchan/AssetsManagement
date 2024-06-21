import { atomWithStorage } from "jotai/utils"

export type LoginRequest = {
  email: string
  password: string
}

export type LoginResponse = {
  tokenType: string
  accessToken: string
  expiresIn: number
  refreshToken: string
}

export type AuthUser = {
  id: string
  accountStatus: string
  claims: Claim[]
}

const initialUser: AuthUser = {
  id: "",
  accountStatus: "",
  claims: [],
}

export const userInfo = atomWithStorage<AuthUser | null>(
  "user-info",
  initialUser
)

export type Claim = {
  type: string
  value: string
}
