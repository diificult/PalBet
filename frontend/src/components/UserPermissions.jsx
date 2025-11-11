import { useSubmit } from "react-router-dom";
import Modal from "./UI/Model";
import { useContext } from "react";
import UserPermissionsContext from "../store/UserPermissionsContext";
export default function UserPermissions() 
{

    
        const submit = useSubmit();
    const modelCtx = useContext(UserPermissionsContext);
    function handleCloseModel() {
        modelCtx.hideModel();
    }
    console.log("Rendering UserPermissions with model data:", modelCtx.modelData?.User.canCreateBets);


    return (
        <Modal 
            open={modelCtx.progress === "show"}
            onClose={modelCtx.progress === "close" ? handleCloseModel : null}
        >
            <div className="bg-white rounded-lg shadow-lg w-full max-w-md mx-auto">
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
                                    className="sr-only peer"
                                    checked={modelCtx.modelData?.User.canCreateBets}
                                />
                              
                            </label>
                        </div>
                    </div>
                </div>
                <div className="px-6 py-4 border-t border-gray-200 flex justify-end space-x-3">
                    <button 
                        onClick={handleCloseModel}
                        className="px-3 py-1.5 text-sm text-gray-600 hover:text-gray-800"
                    >
                        Cancel
                    </button>
                    <button 
                        className="px-3 py-1.5 text-sm text-white bg-blue-600 rounded hover:bg-blue-700"
                    >
                        Save
                    </button>
                </div>
            </div>
        </Modal>
    )
}