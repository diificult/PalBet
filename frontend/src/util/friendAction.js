import { sendHttpRequest } from "../hooks/useHttp";
import { getAuthToken } from "../util/auth";

export async function useFriends(actionType, friendUsername, method) {
    let endpoint = "";
    console.log(actionType);
    if (actionType === "add") endpoint = "/Friend/SendRequest";
    if (actionType === "accept") endpoint = "/Friend/AcceptFriendRequest";
    if (actionType === "decline") endpoint = "/Friend/DeclineFriendRequest";
    if (actionType === "cancel") endpoint = "/Friend/CancelFriendRequest";
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
        throw new Response(
            JSON.stringify({ message: "Could not modify friend status" }),
            {
                status: 422,
            }
        );
    } else {
        console.log("updated correctly!");
    }
}
