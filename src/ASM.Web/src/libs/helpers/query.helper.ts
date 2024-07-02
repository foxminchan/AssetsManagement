/**
 * Generates a query string from an object's key-value pairs.
 * @param options - The object containing parameters for the query string.
 * @returns The query string (e.g., "key1=value1&key2=value2").
 */
// eslint-disable-next-line @typescript-eslint/no-explicit-any
export function buildQueryString(options?: Record<string, any>): string {
  if (!options) {
    return ""
  }

  return Object.entries(options)
    .filter(([, value]) => value !== undefined && value !== null)
    .map(([key, value]) =>
      (key === "state" && Array.isArray(value)) || key === "categories"
        ? (value as string[])
            .map(
              (item) => `${encodeURIComponent(key)}=${encodeURIComponent(item)}`
            )
            .join("&")
        : `${encodeURIComponent(key)}=${encodeURIComponent(value)}`
    )
    .join("&")
}
