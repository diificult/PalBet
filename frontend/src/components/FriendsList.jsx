import FriendItem from "./FriendItem";
const requestConfig = {};
export default function FriendsList({ friends }) {
    console.log(friends);
    return (
        <div className="flex flex-col items-center">
            <h1>Friends</h1>
            <ul>
                {friends.map((friend) => (
                    <li key={friend.id}>
                        <FriendItem friend={friend} />
                    </li>
                ))}
            </ul>
        </div>
    );
}
