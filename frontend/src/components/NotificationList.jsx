import NotificationItem from "./NotificationItem";
import { useFriends } from "../util/friendAction";

export default function NotificationList({ notifications }) {
    return (
        <div className="">
            {notifications.map((notification) => (
                <NotificationItem
                    key={notification.id}
                    notification={notification}
                />
            ))}
        </div>
    );
}

export async function action({ request }) {
    const formData = await request.formData();
    const actionType = formData.get("action");
    const friendUsername = formData.get("friendUsername");

    console.log(friendUsername);

    await useFriends(actionType, friendUsername, request.method);

    return redirect("/notifications");
}
