import { useFetcher } from "react-router-dom";
import Modal from "./UI/Model";
import { useContext, useState, useEffect } from "react";
import UserPermissionsContext from "../store/UserPermissionsContext";
export default function UserPermissions() {
    const fetcher = useFetcher();
    const modelCtx = useContext(UserPermissionsContext);
    const [canCreateBets, setCanCreateBets] = useState(false);

    useEffect(() => {
        setCanCreateBets(Boolean(modelCtx.modelData?.User?.canCreateBets));
    }, [modelCtx.modelData]);

    function handleCloseModel() {
        modelCtx.hideModel();
    }

    function handleSaveChanges(e) {
        e.preventDefault();
        const form = e.currentTarget;
        fetcher.submit(form, { method: "PUT" });

        handleCloseModel();
    }

    return (
        <Modal
            open={modelCtx.progress === "show"}
            onClose={modelCtx.progress === "close" ? handleCloseModel : null}
            className="bg-white rounded-lg shadow-lg w-full max-w-md mx-auto p-0"
        >
            <form onSubmit={handleSaveChanges}>
                <div className="px-6 py-4 border-b border-gray-200">
                    <h2 className="text-lg font-medium text-gray-800">User Permissions</h2>
                    <p className="text-sm text-gray-500">{modelCtx.modelData?.User.username}</p>
                </div>

                <div className="p-6">
                    <div className="space-y-4">
                        <div className="flex items-center justify-between">
                            <div>
                                <p className="font-medium text-gray-700">Create Bets</p>
                                <p className="text-sm text-gray-500">Can create new bets</p>
                            </div>
                            <label className="relative inline-flex items-center cursor-pointer">
                                <input
                                    type="checkbox"
                                    name="canCreateBets"
                                    value="1"
                                    className="sr-only peer"
                                    checked={canCreateBets}
                                    onChange={(e) => setCanCreateBets(e.target.checked)}
                                />
                                <div className="w-11 h-6 bg-gray-200 rounded-full peer-checked:bg-blue-600 peer-focus:ring-2 peer-focus:ring-blue-300 transition-colors"></div>
                                <div className="absolute left-1 top-1 w-4 h-4 bg-white rounded-full shadow transform peer-checked:translate-x-5 transition-transform"></div>
                            </label>
                        </div>
                    </div>
                </div>
                <div className="px-6 py-4 border-t border-gray-200 flex justify-end space-x-3">
                    <button
                        type="button"
                        onClick={handleCloseModel}
                        className="px-3 py-1.5 text-sm text-gray-600 hover:text-gray-800"
                    >
                        Cancel
                    </button>

                    <input type="hidden" name="username" value={modelCtx.modelData?.User?.username ?? ""} />
                    <input type="hidden" name="action" value="updateUserPermissions" />

                    <button
                        type="submit"
                        className="px-3 py-1.5 text-sm text-white bg-blue-600 rounded hover:bg-blue-700"
                    >
                        Save
                    </button>
                </div>
            </form>
        </Modal>
    );
}