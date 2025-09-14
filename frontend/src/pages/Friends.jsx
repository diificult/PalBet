import Container from "@mui/material/Container";
import FriendsList from "../components/FriendsList";
import AddFriend from "../components/AddFriend";
import { Suspense } from "react";
import { sendHttpRequest } from "../hooks/useHttp";
import { getAuthToken } from "../util/auth";
import { Await, redirect, useLoaderData } from "react-router-dom";
const requestConfig = {};

export default function Friends() {
    const { friends, friendRequests, friendRequested } = useLoaderData();
    return (
        <>
            <Container>
                <Suspense
                    fallback={<p style={{ textAlign: "center" }}>Loading...</p>}
                >
                    <Await resolve={friends}>
                        {(loadedFriends) => (
                            <FriendsList
                                friends={loadedFriends}
                                mode="friend"
                            />
                        )}
                    </Await>
                </Suspense>
            </Container>
            <Container>
                <AddFriend> </AddFriend>
            </Container>
            <Container>
                <Suspense
                    fallback={<p style={{ textAlign: "center" }}>Loading...</p>}
                >
                    <Await resolve={friendRequests}>
                        {(loadedRequests) => (
                            <FriendsList
                                friends={loadedRequests}
                                mode="request"
                            />
                        )}
                    </Await>
                </Suspense>
            </Container>
            <Container>
                <Suspense
                    fallback={<p style={{ textAlign: "center" }}>Loading...</p>}
                >
                    <Await resolve={friendRequested}>
                        {(loadedRequests) => (
                            <FriendsList
                                friends={loadedRequests}
                                mode="requested"
                            />
                        )}
                    </Await>
                </Suspense>
            </Container>
        </>
    );
}

async function loadFriends() {
    const response = await sendHttpRequest("Friends/GetFriendsList", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + getAuthToken(),
        },
    });
    if (!response.ok) {
        throw new Response(
            JSON.stringify({ message: "Could not get friends list" }),
            {
                status: 422,
            }
        );
    }

    const resData = await response.json();
    return resData;
}
async function loadFriendRequests() {
    const response = await sendHttpRequest("/Friends/GetFriendRequests", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + getAuthToken(),
        },
    });
    if (!response.ok) {
        throw new Response(
            JSON.stringify({ message: "Could not get friend request list" }),
            {
                status: 422,
            }
        );
    }

    const resData = await response.json();
    return resData;
}
async function loadFriendRequested() {
    const response = await sendHttpRequest("/Friends/GetFriendRequested", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + getAuthToken(),
        },
    });
    if (!response.ok) {
        throw new Response(
            JSON.stringify({ message: "Could not get requested friends list" }),
            {
                status: 422,
            }
        );
    }

    const resData = await response.json();
    return resData;
}
export async function loader() {
    return {
        friends: loadFriends(),
        friendRequests: loadFriendRequests(),
        friendRequested: loadFriendRequested(),
    };
}
export async function action({ request }) {
    const formData = await request.formData();
    const actionType = formData.get("action");
    const friendUsername = formData.get("friendUsername");
    let endpoint = "";
    if (actionType === "add") endpoint = "/Friend/SendRequest";
    if (actionType === "accept") endpoint = "/Friend/AcceptFriendRequest";
    if (actionType === "decline") endpoint = "/Friend/DeclineFriendRequest";
    if (actionType === "cancel") endpoint = "/Friend/CancelFriendRequest";

    const response = await sendHttpRequest(endpoint, {
        method: request.method,
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
    }
    return redirect("/friends");
}
