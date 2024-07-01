type AxiosRequestConfig = import("axios").AxiosRequestConfig

type ErrorValue = {
  errorCode: string
  errorMessage: string
  identifier: string
  severity: number
}

type ApiDataError = {
  type?: string
  title: string
  status?: number
  detail?: string
  value?: ErrorValue[]
}

type AppAxiosError = import("axios").AxiosError<ApiDataError, unknown>
