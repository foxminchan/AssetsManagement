import { AxiosRequestConfig } from "axios"

const axiosConfigs: { [key: string]: AxiosRequestConfig } = {
  development: {
    baseURL: `${import.meta.env.VITE_API_URL}/api`,
    timeout: 10000,
  },
  production: {
    baseURL: `${import.meta.env.VITE_API_URL}/api`,
    timeout: 10000,
  },
  test: {
    baseURL: `${import.meta.env.VITE_API_URL}/api`,
    timeout: 10000,
  },
}

const getAxiosConfig = (): AxiosRequestConfig => {
  const nodeEnv: string | undefined = import.meta.env.MODE
  return axiosConfigs[nodeEnv as keyof typeof axiosConfigs]
}

const axiosConfig = getAxiosConfig()

export default axiosConfig
