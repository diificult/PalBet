import FriendItem from "./FriendItem";

export default function FriendsList({ friends, mode }) {
    return (
        <div className="w-full">

            <ul className="space-y-3">
                {friends.length === 0 && (
                    <li className="text-sm text-gray-500">No items to show</li>
                )}
                {friends.map((friend) => (
                    <li key={friend.id} className="bg-white border border-gray-100 rounded-md p-3 shadow-sm">
                        <FriendItem friend={friend} mode={mode} />
                    </li>
                ))}
            </ul>
        </div>
    );
}
