import { useSubmit } from "react-router-dom";
import defaultimg from "../assets/default.jpg";
import UserItemDropDown from "./UserItemDropDown";

export default function UserItem({ friend, mode }) {
    const submit = useSubmit();

    function handleAction(actionType, method) {
        submit(
            { friendUsername: friend.username, action: actionType },
            {
                method: method,
            }
        );
    }

   return (
        <div className="flex items-center justify-between w-full">
            <div className="flex items-center gap-3">
                <img
                    src={defaultimg}
                    className="w-12 h-12 object-cover rounded-md"
                />
                <div>
                    <div className="font-medium text-gray-900">
                        {friend.username}
                        {friend.role && friend.role === "ADMIN" && (
                            <p className="inline-flex items-center px-2 py-0.5 bg-yellow-100 text-yellow-800 text-xs font-medium rounded-md ml-2">Admin </p>
                        )}
                    </div>
                </div>
            </div>

            <div className="flex items-center gap-2">
                {mode === "request" && (
                    <>
                        <button
                            onClick={() => handleAction("accept", "PUT")}
                            className="inline-flex items-center px-3 py-1.5 bg-green-600 hover:bg-green-700 text-white text-sm font-medium rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-green-500"
                        >
                            Accept
                        </button>

                        <button
                            onClick={() => handleAction("decline", "DELETE")}
                            className="inline-flex items-center px-3 py-1.5 bg-white border border-red-300 text-red-600 text-sm font-medium rounded-md hover:bg-red-50 focus:outline-none focus:ring-2 focus:ring-red-300"
                        >
                            Decline
                        </button>
                    </>
                )}

                {mode === "requested" && (
                    <button
                        onClick={() => handleAction("cancel", "DELETE")}
                        className="inline-flex items-center px-3 py-1.5 bg-gray-100 text-gray-700 text-sm font-medium rounded-md hover:bg-gray-200 focus:outline-none focus:ring-2 focus:ring-gray-200"
                    >
                        Cancel
                    </button>
                )}

                {mode === "friend" && (
                    <button
                        onClick={() => handleAction("remove", "DELETE")}
                        className="inline-flex items-center px-3 py-1.5 bg-white border border-gray-200 text-gray-700 text-sm font-medium rounded-md hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-gray-200"
                    >
                        Remove
                    </button>
                )}
                {mode === "groupMember" && (
                    <>
                    <p>{friend.balance} coins</p>
                    <UserItemDropDown />
</>
                    )}
            </div>
        </div>
    );
}
