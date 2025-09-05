import defaultimg from "../assets/default.jpg";

export default function FriendItem({ friend }) {
    return (
        <div className="flex rounded-lg shadow-lg max-w-64 object-center">
            <img src={defaultimg} className="max-w-16 rounded-l-lg" />
            <strong className="self-center flex p-4">{friend.username}</strong>
        </div>
    );
}
