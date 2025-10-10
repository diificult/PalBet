import Container from "@mui/material/Container";
import FriendsList from "../components/FriendsList";
import AddFriend from "../components/AddFriend";
import { Suspense } from "react";
import { sendHttpRequest } from "../hooks/useHttp";
import { getAuthToken } from "../util/auth";
import {
    Await,
    redirect,
    useActionData,
    useLoaderData,
} from "react-router-dom";
import { useFriends } from "../util/friendAction";
const requestConfig = {};

export default function Friends() {
    const { friends, friendRequests, friendRequested } = useLoaderData();
    const data = useActionData();
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
    const response = await sendHttpRequest("/Friend/GetFriendsList", {
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
    const response = await sendHttpRequest("/Friend/GetFriendRequests", {
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
    const response = await sendHttpRequest("/Friend/GetFriendRequested", {
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

    const response = await useFriends(
        actionType,
        friendUsername,
        request.method
    );
    if (response.error) {
        return response;
    }
    return null;
}
