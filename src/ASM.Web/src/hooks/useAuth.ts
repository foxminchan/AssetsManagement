export const useAuth = () => {
  const signIn = (token: string) => {
    localStorage.setItem(".AspNetCore.Identity.Token", token)
  }

  const signOut = () => {
    localStorage.removeItem(".AspNetCore.Identity.Token")
  }

  const isLogged = () => {
    return !!localStorage.getItem(".AspNetCore.Identity.Token")
  }

  return { signIn, signOut, isLogged }
}

export type AuthContext = ReturnType<typeof useAuth>
