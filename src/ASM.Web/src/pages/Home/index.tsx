import { useContext, useEffect, useState } from "react"
import { BreadcrumbsContext } from "@/context/BreadcrumbsContext"
import logo from "@assets/logo.svg"

import { BreadcrumbItem } from "@/types/data"
import AlertModal from "@/components/modals/alert-modal"

const breadcrumb: BreadcrumbItem[] = [
  {
    label: "Home",
    to: "/",
  },
]

export default function Home() {
  const [open, setOpen] = useState(false)
  const context = useContext(BreadcrumbsContext)

  useEffect(() => {
    context?.setBreadcrumbs(breadcrumb)
  }, [])

  return (
    <div className="mx-auto max-w-screen-md p-4 text-center">
      <div className="flex items-center justify-center">
        <a href="https://www.nashtechglobal.com/" target="_blank">
          <img
            loading="lazy"
            src={logo}
            className="logo will-change-filter transition-filter hover:filter-drop-shadow-[0_0_2em_#646cffaa] react:hover:filter-drop-shadow-[0_0_2em_#61dafbaa] motion-reduce:animate-infinite motion-reduce:animate-duration-20s motion-reduce:animate-linear h-16 p-4 duration-300 motion-reduce:animate-spin"
            alt="NashTech logo"
          />
        </a>
      </div>
      <h1 className="text-3xl font-bold underline">Vite + React</h1>
      <div className="card p-8">
        <button
          className="rounded bg-blue-500 px-4 py-2 font-bold text-white hover:bg-blue-700"
          onClick={() => setOpen(true)}
        >
          Open modal
        </button>
        <p>
          Edit <code>src/App.tsx</code> and save to test HMR
        </p>
      </div>
      <p className="read-the-docs text-gray-500">
        Click on the Vite and React logos to learn more
      </p>
      <AlertModal
        open={open}
        message="This is alert modal"
        onClose={() => setOpen(false)}
      ></AlertModal>
    </div>
  )
}
