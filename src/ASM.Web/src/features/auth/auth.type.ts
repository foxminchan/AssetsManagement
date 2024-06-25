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

export type Claim = {
  type: string
  value: string
}

export type UpdatePasswordRequest = {
  id: string
  oldPassword: string
  newPassword: string
}
