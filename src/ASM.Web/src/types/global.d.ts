type AxiosRequestConfig = import("axios").AxiosRequestConfig

type ApiDataError = {
  type?: string
  title: string
  status?: number
  detail?: string
}

type AppAxiosError = import("axios").AxiosError<ApiDataError, unknown>
