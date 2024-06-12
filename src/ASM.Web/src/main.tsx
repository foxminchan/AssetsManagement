import React from "react"
import ReactDOM from "react-dom/client"

import "./styles/tailwind.css"

import { createRouter } from "@tanstack/react-router"

import App from "./App.tsx"
import { routeTree } from "./routeTree.gen.ts"

const router = createRouter({ routeTree })

declare module "@tanstack/react-router" {
  interface Register {
    router: typeof router
  }
}

ReactDOM.createRoot(document.getElementById("root")!).render(
  <React.StrictMode>
    <App router={router} />
  </React.StrictMode>
)
