import HttpService from "@libs/services/http.service"

class AssetService extends HttpService {
  constructor() {
    super()
  }

  deleteAsset(id: string): Promise<void> {
    return this.delete(`/assets/${id}`)
  }
}

export default new AssetService()
