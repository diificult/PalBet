import { Await, useLoaderData } from "react-router-dom";
import { sendHttpRequest } from "../hooks/useHttp";
import { getAuthToken } from "../util/auth";
import { Suspense, useEffect, useState } from "react";
import NotificationList from "../components/NotificationList";
import * as signalR from "@microsoft/signalr";

export function useNotificationsLive(setNotifications) {
  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:7130/NotificationHub", { accessTokenFactory: () => getAuthToken()  })
      .withAutomaticReconnect()
      .build();

    connection.start();

    connection.on("ReceiveNotification", (notification) => {
      setNotifications(prev => {
        if (prev.some(n => n.id === notification.id)) return prev;
        return [notification, ...prev];
      });
    });

    return () => connection.stop();
  }, []);
}

export default function NotificationPage() {
    const { notifications } = useLoaderData();
    const [notificationsList, setNotificationsList] = useState(notifications);

    console.log("Initial notifications:", Promise.resolve(notifications));
  useEffect(() => {
    if (notifications) {
      Promise.resolve(notifications).then(data => {
        setNotificationsList(data);
      });
    }
  }, [notifications]);
    console.log("Notifications list state:", notificationsList);


    useNotificationsLive(setNotificationsList);
    console.log("Notifications list after live updates:", notificationsList);

    return (
        <>
            <Suspense fallback={<p>Loading Notifications</p>}>
                <Await resolve={notifications}>
                    {() => (
                        <NotificationList notifications={notificationsList} />
                    )}
                </Await>
            </Suspense>
        </>
    );
}

async function loadNotifications() {
    const response = await sendHttpRequest("/Notification", {
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
