import defaultimg from "../assets/default.jpg";

export default function FriendItem({ friend, mode }) {
    return (
        <div className="flex rounded-lg shadow-lg  justify-center">
            <img src={defaultimg} className="max-w-16 rounded-l-lg" />
            <strong className="self-center flex p-4">{friend.username}</strong>
            {mode === "request" && (
                <>
                    <button className="font-semibold text-blue-400 p-2.5 hover:underline">
                        Accept
                    </button>
                    <button className="font-semibold text-red-400 p-2.5 hover:underline">
                        Decline
                    </button>
                </>
            )}
            {mode === "requested" && (
                <>
                    <button className="font-semibold text-gray-400 p-2.5 hover:underline">
                        Cancel
                    </button>
                </>
            )}
        </div>
    );
}
