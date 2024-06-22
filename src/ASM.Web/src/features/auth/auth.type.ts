import { atom } from "jotai"

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

export enum AccountStatus {
  FirstTime = "FirstTime",
  Active = "Active",
  Deactivated = "Deactivated",
  None = "",
}

export type AuthUser = {
  id: string
  accountStatus: AccountStatus
  claims: Claim[]
}

const initialUser: AuthUser = {
  id: "",
  accountStatus: AccountStatus.None,
  claims: [],
}

export const userInfo = atom<AuthUser | null>(initialUser)

export type Claim = {
  type: string
  value: string
}

export type UpdatePasswordRequest = {
  id: string
  oldPassword: string
  newPassword: string
}
