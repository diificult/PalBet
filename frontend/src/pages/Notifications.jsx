import { Await, useLoaderData } from "react-router-dom";
import { sendHttpRequest } from "../hooks/useHttp";
import { getAuthToken } from "../util/auth";
import { Suspense } from "react";
import NotificationList from "../components/NotificationList";

export default function NotificationPage() {
    const { notifications } = useLoaderData();
    return (
        <>
            <Suspense fallback={<p>Loading Notifications</p>}>
                <Await resolve={notifications}>
                    {(loadedNotifications) => (
                        <NotificationList notifications={loadedNotifications} />
                    )}
                </Await>
            </Suspense>
        </>
    );
}

async function loadNotifications() {
    const response = await sendHttpRequest("/Notification/GetNotifications", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + getAuthToken(),
        },
    });
    if (response.status === 204) {
        return {};
    }
    if (!response.ok) {
        throw new Response(
            JSON.stringify({
                message:
                    "Error retrieving notifications | " +
                    response.status +
                    " : " +
                    response.statusText,
            }),
            {
                status: 422,
            }
        );
    }
    const resData = await response.json();
    return resData;
}

export function loader() {
    return {
        notifications: loadNotifications(),
    };
}
