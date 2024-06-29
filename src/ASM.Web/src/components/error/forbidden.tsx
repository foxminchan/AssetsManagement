import { Button } from "@mui/material"
import { Link } from "@tanstack/react-router"

export default function Forbidden() {
  return (
    <section className="bg-white">
      <div className="mx-auto max-w-screen-xl px-4 py-8 lg:px-6 lg:py-16">
        <div className="mx-auto max-w-screen-sm text-center">
          <h1 className="text-primary-600 dark:text-primary-500 mb-4 text-7xl font-extrabold tracking-tight lg:text-9xl">
            403
          </h1>
          <p className="mb-4 text-3xl font-bold tracking-tight text-gray-900 md:text-4xl">
            You don't have permission to access this page.
          </p>
          <p className="mb-4 text-lg font-light text-gray-500">
            Sorry, you don't have permission to access this page. Please contact
            your administrator.{" "}
          </p>

          <Link
            to="/"
            className="bg-primary-600 hover:bg-primary-800 focus:ring-primary-300 my-4 inline-flex rounded-lg px-5 py-2.5 text-center text-sm font-medium text-black focus:outline-none focus:ring-4"
          >
            <Button
              id="btn-back-to-home"
              className="rounded !bg-red-500 !p-2 !font-bold !text-white"
            >
              Back to Homepage
            </Button>
          </Link>
        </div>
      </div>
    </section>
  )
}
