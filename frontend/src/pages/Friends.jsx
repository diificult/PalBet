import Container from "@mui/material/Container";
import FriendsList from "../components/FriendsList";
import AddFriend from "../components/AddFriend";
import { Suspense } from "react";
import { sendHttpRequest } from "../hooks/useHttp";
import { getAuthToken } from "../util/auth";
import { Await, useLoaderData } from "react-router-dom";
const requestConfig = {};

export default function Friends() {
    const { friends } = useLoaderData();
    return (
        <>
            <Container>
                <Suspense
                    fallback={<p style={{ textAlign: "center" }}>Loading...</p>}
                >
                    <Await resolve={friends}>
                        {(loadedFriends) => (
                            <FriendsList friends={loadedFriends} />
                        )}
                    </Await>
                </Suspense>
            </Container>
            <Container>
                <AddFriend> </AddFriend>
            </Container>
        </>
    );
}

async function loadFriends() {
    const response = await sendHttpRequest("/GetFriendsList", {
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

export async function loader() {
    return {
        friends: loadFriends(),
    };
}
