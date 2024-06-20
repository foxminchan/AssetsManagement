import { useEffect, useState } from "react"
import useLogin from "@/features/auth/useLogin"
import logo from "@assets/logo.svg"
import Box from "@mui/material/Box"
import Button from "@mui/material/Button"
import Container from "@mui/material/Container"
import CssBaseline from "@mui/material/CssBaseline"
import TextField from "@mui/material/TextField"
import Typography from "@mui/material/Typography"
import { useForm } from "@tanstack/react-form"
import { match } from "ts-pattern"
import { useAuth } from "@/hooks/useAuth"
import { IconButton, InputAdornment } from "@mui/material"
import VisibilityOffIcon from '@mui/icons-material/VisibilityOff';
import VisibilityIcon from '@mui/icons-material/Visibility';

type FormValues = {
  username: string
  password: string
}

export default function Login() {
  const auth = useAuth()
  const { mutate: login, isSuccess, error, isError, data } = useLogin()
  const [modify, setModify] = useState('');
  const [showPassword, setShowPassword] = useState(false);

  const handleClickShowPassword = () => {
    setShowPassword(!showPassword);
  };
  const { Field, Subscribe, handleSubmit } = useForm<FormValues>({
    defaultValues: {
      username: "",
      password: "",
    },

    onSubmit: async ({ value }) => {
      login({
        email: value.username,
        password: value.password,
      })
    },
  })

  const getErrorMessage = (error: AppAxiosError) => {

    return match(error)
      .with(
        { response: { data: { detail: "LockedOut" } } },
        () => "Your account has been disabled."
      )
      .otherwise(() => "Invalid user name or password.")
  }

  useEffect(() => {
    if (isSuccess) {
      auth.signIn(data.accessToken)
      window.location.href = "/"
    } else {
      setModify('InError');
    }
  }, [isSuccess, isError, error])

  return (
    <Container component="main" maxWidth="xs">
      <CssBaseline />
      <Box
        sx={{
          marginTop: 8,
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
        }}
      >
        <img loading="lazy" src={logo} alt="logo" width={150} />
        <Typography component="h1" variant="h5">
          Sign in
        </Typography>
        <form
          onSubmit={(e) => {
            e.preventDefault()
            e.stopPropagation()
            handleSubmit()
          }}
        >
          <Field name="username">
            {({ state, handleChange, handleBlur }) => (
              <TextField
                margin="normal"
                fullWidth
                id="username"
                defaultValue={state.value}
                onChange={(e) => {
                  handleChange(e.target.value);
                  setModify('');
                }}
                onBlur={handleBlur}
                label="Username"
                autoComplete="username"
                autoFocus

              />
            )}
          </Field>
          <Field name="password">
            {({ state, handleChange, handleBlur }) => (
                <TextField
                  margin="normal"
                  fullWidth
                  id="password"
                  defaultValue={state.value}
                  onChange={(e) => {
                    handleChange(e.target.value);
                    setModify('');
                  }}
                  onBlur={handleBlur}
                  label="Password"
                  type={showPassword ? 'text' : 'password'}
                  autoComplete="current-password"

                  InputProps={{
                    endAdornment: (
                      <InputAdornment position="end">
                        <IconButton
                          aria-label="toggle password visibility"
                          onClick={handleClickShowPassword}
                        >
                          {showPassword ? <VisibilityIcon /> : <VisibilityOffIcon />}
                        </IconButton>
                      </InputAdornment>
                    ),
                  }}
                />
            )}
          </Field>
          {error && modify && (
            <Typography variant="body2" color="error" align="center">
              {getErrorMessage(error)}
            </Typography>
          )}
          <Subscribe
            selector={(state) => [
              state.canSubmit,
              state.isSubmitting,
              state.values,
            ]}
          >
            {([canSubmit, isSubmitting, values]) => (
              <Button
                type="submit"
                fullWidth
                variant="contained"
                sx={{ mt: 3, mb: 2 }}
                className="disabled:!hover:bg-red-800 !bg-red-500 !text-white disabled:!bg-red-700 disabled:!text-gray-300"
                disabled={
                  !canSubmit ||
                  !(values as FormValues).username ||
                  !(values as FormValues).password
                }
              >
                {isSubmitting ? "Singing In..." : "Sign In"}
              </Button>
            )}
          </Subscribe>
        </form>
      </Box>
    </Container>
  )
}
