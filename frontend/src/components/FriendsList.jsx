import { redirect } from "react-router-dom";
import { sendHttpRequest } from "../hooks/useHttp";
import FriendItem from "./FriendItem";
const requestConfig = {};
export default function FriendsList({ friends, mode }) {
    console.log(friends);
    return (
        <div className="flex flex-col items-center">
            <p className="font-extrabold text-3xl p-5">
                {mode === "friend"
                    ? "Friend"
                    : mode === "request"
                    ? "Friend Requests"
                    : mode === "requested"
                    ? "Sent Requests"
                    : ""}
            </p>
            <ul>
                {friends.map((friend) => (
                    <li key={friend.id}>
                        <FriendItem friend={friend} mode={mode} />
                    </li>
                ))}
            </ul>
        </div>
    );
}
