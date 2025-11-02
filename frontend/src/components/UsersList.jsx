import UserItem from "./UserItem";

export default function UsersList({ Users, mode }) {
    return (
        <div className="w-full">

            <ul className="space-y-3">
                {Users.length === 0 && (
                    <li className="text-sm text-gray-500">No items to show</li>
                )}
                {Users.map((user) => (
                    <li key={user.id} className="bg-white border border-gray-100 rounded-md p-3 shadow-sm">
                        <UserItem friend={user} mode={mode} />
                    </li>
                ))}
            </ul>
        </div>
    );
}
