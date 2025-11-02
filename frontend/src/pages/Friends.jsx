import UsersList from "../components/UsersList";
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
            <div className="w-full">
                <div className="flex items-center justify-between mb-6">
                    <h1 className="text-2xl font-semibold ">Friends</h1>
                </div>

                <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
                   
                    <div className="lg:col-span-2 space-y-6">
                        <div className="bg-white border border-gray-200 rounded-lg shadow-sm p-4">
                            <h2 className="text-lg font-medium mb-3">Your Friends</h2>
                            <div className="border-t border-gray-100 pt-3">
                                <Suspense fallback={<p className="text-center text-gray-500">Loading...</p>}>
                                    <Await resolve={friends}>
                                        {(loadedFriends) => (
                                            <UsersList Users={loadedFriends} mode="friend" />
                                        )}
                                    </Await>
                                </Suspense>
                            </div>
                        </div>

                        <div className="bg-white border border-gray-200 rounded-lg shadow-sm p-4">
                            <h2 className="text-lg font-medium mb-3">Friend Requests</h2>
                            <div className="border-t border-gray-100 pt-3">
                                <Suspense fallback={<p className="text-center text-gray-500">Loading...</p>}>
                                    <Await resolve={friendRequests}>
                                        {(loadedRequests) => (
                                            <UsersList Users={loadedRequests} mode="request" />
                                        )}
                                    </Await>
                                </Suspense>
                            </div>
                        </div>

                        <div className="bg-white border border-gray-200 rounded-lg shadow-sm p-4">
                            <h2 className="text-lg font-medium mb-3">Sent Requests</h2>
                            <div className="border-t border-gray-100 pt-3">
                                <Suspense fallback={<p className="text-center text-gray-500">Loading...</p>}>
                                    <Await resolve={friendRequested}>
                                        {(loadedRequests) => (
                                            <UsersList Users={loadedRequests} mode="requested" />
                                        )}
                                    </Await>
                                </Suspense>
                            </div>
                        </div>
                    </div>

                    <aside className="space-y-6">
                        <div className="bg-white border border-gray-200 rounded-lg shadow-sm p-4">
                            <h3 className="text-lg font-medium mb-2">Add a Friend</h3>
                        
                            <AddFriend />
                        </div>
                    </aside>
                </div>
            </div>
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
