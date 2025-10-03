import NotificationItem from "./NotificationItem";

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
