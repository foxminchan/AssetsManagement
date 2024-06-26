import { useState } from "react"
import { AssetState } from "@features/assets/asset.type"
import useListAsset from "@features/assets/useListAsset"
import useListUsers from "@features/users/useListUsers"
import { RoleType } from "@features/users/user.type"
import { selectedRowAsset } from "@libs/jotai/assetAtom"
import { selectedRowUser } from "@libs/jotai/assignmentAtom"
import SearchIcon from "@mui/icons-material/Search"
import {
  Button,
  DialogActions,
  DialogContent,
  DialogTitle,
  Grid,
  IconButton,
} from "@mui/material"
import Dialog from "@mui/material/Dialog"
import { useSearch } from "@tanstack/react-router"
import { useAtom } from "jotai"

import AssetDialog from "../dialogs/asset/asset-dialog"
import UserDialog from "../dialogs/user/user-dialog"
import SearchInput from "../fields/search-dialog"

const types = ["All", RoleType.Admin, RoleType.Staff]

export default function AssignmentForm() {
  //? User Dialog
  const userParams = useSearch({
    strict: false,
  })
  const queryUserParameters = {
    orderBy: (userParams as { orderBy?: string }).orderBy ?? "staffCode",
    isDescending:
      (userParams as { isDescending?: boolean }).isDescending ?? false,
    RoleType:
      (userParams as { roleType?: string }).roleType === types[0]
        ? undefined
        : (userParams as { roleType?: string }).roleType,
    search: (userParams as { search?: string }).search ?? "",
  }
  const { data: userData, isLoading: listLoading } =
    useListUsers(queryUserParameters)
  const [userChoose] = useAtom(selectedRowUser)
  const [isUserDialogOpen, setIsUserDialogOpen] = useState(false)

  //? Asset Dialog
  const assetParams = useSearch({
    strict: false,
  })
  const queryAssetParameters = {
    orderBy: (assetParams as { orderBy?: string }).orderBy ?? "assetCode",
    isDescending:
      (assetParams as { isDescending?: boolean }).isDescending ?? false,
    search: (assetParams as { search?: string }).search ?? undefined,
    state: [AssetState.Available],
  }
  const { data: assetData, isLoading: listAssetLoading } =
    useListAsset(queryAssetParameters)
  const [assetChoose] = useAtom(selectedRowAsset)
  const [isAssetDialogOpen, setIsAssetDialogOpen] = useState(false)

  return (
    <>
      <IconButton
        onClick={() => setIsUserDialogOpen(true)}
        color="primary"
        size="small"
        id="userDialogOpen-btn"
        style={{ marginLeft: "8px" }}
      >
        <SearchIcon className="text-red-500" />
      </IconButton>

      <IconButton
        onClick={() => setIsAssetDialogOpen(true)}
        color="primary"
        size="small"
        id="assetDialogOpen-btn"
        style={{ marginLeft: "8px" }}
      >
        <SearchIcon className="text-red-500" />
      </IconButton>

      <Dialog
        open={isUserDialogOpen}
        onClose={() => setIsUserDialogOpen(false)}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="sm"
        fullWidth
      >
        <div className="w-full transform overflow-hidden rounded-lg bg-white shadow-xl transition-all sm:max-w-4xl">
          <DialogTitle
            id="alert-dialog-title"
            className="bg-white-100 flex items-center justify-between border-b p-4 text-red-500"
          >
            <span>Select User</span>
            <Grid item xs={7} className="!flex !flex-row-reverse">
              <SearchInput />
            </Grid>
          </DialogTitle>
          <DialogContent className="p-4">
            <UserDialog
              data={[...(userData?.users || [])]}
              isLoading={listLoading}
            />
          </DialogContent>
          <DialogActions className="border-t bg-gray-100 p-4">
            <Button
              disabled={!userChoose?.name}
              className="disabled:!hover:bg-red-800 !bg-red-500 !text-white disabled:!bg-red-700 disabled:!text-gray-300"
            >
              Save
            </Button>
            <Button
              onClick={() => setIsUserDialogOpen(false)}
              autoFocus
              className="!hover:bg-gray-700 rounded !bg-gray-100 px-4 py-2 font-bold !text-black"
            >
              Cancel
            </Button>
          </DialogActions>
        </div>
      </Dialog>

      <Dialog
        open={isAssetDialogOpen}
        onClose={() => setIsAssetDialogOpen(false)}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="sm"
        fullWidth
      >
        <div className="w-full transform overflow-hidden rounded-lg bg-white shadow-xl transition-all sm:max-w-4xl">
          <DialogTitle
            id="alert-dialog-title"
            className="bg-white-100 flex items-center justify-between border-b p-4 text-red-500"
          >
            <span>Select Asset</span>
            <Grid item xs={7} className="!flex !flex-row-reverse">
              <SearchInput />
            </Grid>
          </DialogTitle>
          <DialogContent className="p-4">
            <AssetDialog
              data={[...(assetData?.assets || [])]}
              isLoading={listAssetLoading}
            />
          </DialogContent>
          <DialogActions className="bg-white-100 border-t p-4">
            <Button
              disabled={!assetChoose?.name}
              className="disabled:!hover:bg-red-800 !bg-red-500 !text-white disabled:!bg-red-700 disabled:!text-gray-300"
            >
              Save
            </Button>
            <Button
              onClick={() => setIsAssetDialogOpen(false)}
              autoFocus
              className="!hover:bg-gray-700 rounded !bg-gray-100 px-4 py-2 font-bold !text-black"
            >
              Cancel
            </Button>
          </DialogActions>
        </div>
      </Dialog>
    </>
  )
}
