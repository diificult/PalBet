import {
    CelebrationOutlined,
    GroupOutlined,
    PlayArrowOutlined,
    TollOutlined,
} from "@mui/icons-material";
import { Button } from "@mui/material";

export default function NotificationItem({ notification }) {
    return (
        <div
            className={
                notification.isRead
                    ? "bg-gray-100 p-5 m-5 shadow-xl rounded-md flex justify-between"
                    : "bg-white p-5 m-5 shadow-xl rounded-md flex justify-between"
            }
        >
            {notification.notificationType === 0 && (
                <>
                    <GroupOutlined fontSize="large" />
                    <p>
                        {notification.payload.fromUserName} wants to add you as
                        a friend!
                    </p>
                    <div className="flex gap-5">
                        <Button variant="contained">Accept</Button>
                        <Button variant="text">Decline</Button>
                    </div>
                </>
            )}
            {notification.notificationType === 1 && (
                <>
                    <TollOutlined fontSize="large" />
                    <p>
                        {notification.payload.createdByUserName} has sent you a
                        new bet request - {notification.payload.betTitle}{" "}
                    </p>
                    <div className="flex gap-5">
                        <Button variant="contained">View</Button>
                        <Button variant="text">View</Button>
                    </div>
                </>
            )}
            {notification.notificationType === 2 && (
                <>
                    <PlayArrowOutlined fontSize="large" />
                    <p>{notification.payload.betTitle} is now in play</p>
                    <div className="flex gap-5">
                        <Button variant="contained">View</Button>
                    </div>
                </>
            )}
            {notification.notificationType === 3 && (
                <>
                    <CelebrationOutlined fontSize="large" />
                    <p>
                        {notification.payload.winnerUsername} has won{" "}
                        {notification.payload.betTitle}!
                    </p>
                    <div className="flex gap-5">
                        <Button variant="contained">View</Button>
                    </div>
                </>
            )}
        </div>
    );
}
