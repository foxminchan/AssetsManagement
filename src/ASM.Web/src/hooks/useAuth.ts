import Cookies from "js-cookie"

export const useAuth = () => {
  const signIn = (token: string, expires: number) => {
    Cookies.set(".AspNetCore.Identity.Token", token, { expires })
  }

  const signOut = () => {
    Cookies.remove(".AspNetCore.Identity.Token")
  }

  const isLogged = () => {
    return !!Cookies.get(".AspNetCore.Identity.Token")
  }

  return { signIn, signOut, isLogged }
}

export type AuthContext = ReturnType<typeof useAuth>
