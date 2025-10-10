import { sendHttpRequest } from "../hooks/useHttp";
import { getAuthToken } from "../util/auth";

export async function useFriends(actionType, friendUsername, method) {
    let endpoint = "";
    if (actionType === "add") endpoint = "/Friend/SendRequest";
    else if (actionType === "accept") endpoint = "/Friend/AcceptFriendRequest";
    else if (actionType === "decline")
        endpoint = "/Friend/DeclineFriendRequest";
    else if (actionType === "cancel") endpoint = "/Friend/CancelFriendRequest";
    else console.log("Error getting endpoint");

    const response = await sendHttpRequest(endpoint, {
        method: method,
        headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + getAuthToken(),
        },
        body: JSON.stringify(friendUsername),
    });
    if (!response.ok) {
        const error = await response.json();
        return { error };
    } else {
        return response;
    }
}
