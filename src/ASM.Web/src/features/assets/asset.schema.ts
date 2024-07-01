import { z } from "zod"

import { AssetState } from "./asset.type"

export const assetSchema = z.object({
  name: z.string(),
  specification: z.string(),
  installDate: z.date(),
  state: z.nativeEnum(AssetState),
  categoryId: z.string(),
})
