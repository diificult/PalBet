import { useSubmit } from "react-router-dom";
import defaultimg from "../assets/default.jpg";

export default function FriendItem({ friend, mode }) {
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
        <div className="flex rounded-lg shadow-lg  justify-between">
            <img src={defaultimg} className="max-w-16 rounded-l-lg" />
            <strong className="self-center flex p-4">{friend.username}</strong>
            {mode === "request" && (
                <>
                    <button
                        onClick={() => {
                            handleAction("accept", "PUT");
                        }}
                        className="font-semibold text-blue-400 p-2.5 hover:underline"
                    >
                        Accept
                    </button>
                    <button
                        onClick={() => {
                            handleAction("decline", "DELETE");
                        }}
                        className="font-semibold text-red-400 p-2.5 hover:underline"
                    >
                        Decline
                    </button>
                </>
            )}
            {mode === "requested" && (
                <>
                    <button
                        onClick={() => {
                            handleAction("cancel", "DELETE");
                        }}
                        className="font-semibold text-gray-400 p-2.5 hover:underline"
                    >
                        Cancel
                    </button>
                </>
            )}
        </div>
    );
}
