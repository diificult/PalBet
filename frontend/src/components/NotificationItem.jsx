import {
    CelebrationOutlined,
    GroupOutlined,
    PlayArrowOutlined,
    TollOutlined,
} from "@mui/icons-material";
import { Button } from "@mui/material";
import { useSubmit } from "react-router-dom";

export default function NotificationItem({ notification }) {
    const submit = useSubmit();

    function handleAction(actionType, method) {
        submit(
            {
                friendUsername: notification.payload.fromUserName,
                action: actionType,
            },
            {
                method: method,
            }
        );
    }
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
                        <Button
                            variant="contained"
                            onClick={() => {
                                handleAction("accept", "PUT");
                            }}
                        >
                            Accept
                        </Button>
                        <Button
                            variant="text"
                            onClick={() => {
                                handleAction("decline", "PUT");
                            }}
                        >
                            Decline
                        </Button>
                    </div>
                </>
            )}
            {notification.notificationType === 1 && (
                <>
                    <TollOutlined fontSize="large" />
                    <p>
                        {notification.payload.createdByUserName} has sent you a
                        new bet request - {notification.payload.betTitle}
                    </p>
                    <div className="flex gap-5">
                        <Button variant="contained" href="/bets">
                            View
                        </Button>
                    </div>
                </>
            )}
            {notification.notificationType === 2 && (
                <>
                    <PlayArrowOutlined fontSize="large" />
                    <p>{notification.payload.betTitle} is now in play</p>
                    <div className="flex gap-5">
                        <Button variant="contained" href="/bets">
                            View
                        </Button>
                    </div>
                </>
            )}
            {notification.notificationType === 3 && (
                <>
                    <CelebrationOutlined fontSize="large" />
                    <p>
                        {notification.payload.winnerUsername} has won
                        {notification.payload.betTitle}!
                    </p>
                    <div className="flex gap-5">
                        <Button variant="contained" href="/bets">
                            View
                        </Button>
                    </div>
                </>
            )}
        </div>
    );
}
