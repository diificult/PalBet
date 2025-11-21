import { Suspense, useState } from "react";
import { Await, Form, Link, redirect, useLoaderData, useNavigate, useNavigation } from "react-router-dom";
import { sendHttpRequest } from "../hooks/useHttp";
import { getAuthToken } from "../util/auth";
import UserItem from "./UserItem";

export default function CreateGroupForm({method}) {

        const { friendsList } = useLoaderData();
    const [friends, setFriends] = useState(() => []);

    const handleFriends = (event, newFormats) => {
        setFriends(newFormats);
    };

    const navigation = useNavigation();
    const isSubmitting = navigation.state === "submitting";


    return (
        <Form method={method} className="space-y-8 p-6">
            <div>
                <label htmlFor="name" className="block font-semibold mb-2">Group Name</label>
                <input
                    id="name"
                    type="text"
                    name="name"
                    required
                    className="w-full border border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                />
            </div>
            <div>
                <label htmlFor="coins" className="block font-semibold mb-2">Default Coins per User (optional)</label>
                <input
                    id="coins"
                    name="coins"
                    type="number"
                    min="0"
                    className="w-full border border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                />
            </div>
            <input
                type="hidden"
                name="friends"
                value={JSON.stringify(friends)}
            />
            <div>
                <label className="block font-semibold mb-2">Select Friend(s)</label>
                <Suspense fallback={<p className="text-center">Loading...</p>}>
                    <Await resolve={friendsList}>
                        {(loadedFriends) => (
                            <>
                                {loadedFriends.length === 0 && (
                                    <p>
                                        You currently have no friends added.{' '}
                                        <Link to="/friends" className="text-blue-600 underline">Click here</Link> to add some!
                                    </p>
                                )}
                                <div className="flex flex-wrap gap-2">
                                    {loadedFriends.map((friend) => (
                                        <label key={friend.username} className={`flex items-center border rounded px-3 py-2 cursor-pointer transition ${friends.includes(friend.username) ? 'bg-blue-100 border-blue-400' : 'bg-white border-gray-300'}`}>
                                            <input
                                                type="checkbox"
                                                className="mr-2 accent-blue-600"
                                                checked={friends.includes(friend.username)}
                                                onChange={e => {
                                                    if (e.target.checked) {
                                                        setFriends([...friends, friend.username]);
                                                    } else {
                                                        setFriends(friends.filter(f => f !== friend.username));
                                                    }
                                                }}
                                            />
                                            <UserItem friend={friend} mode="selector" />
                                        </label>
                                    ))}
                                </div>
                            </>
                        )}
                    </Await>
                </Suspense>
            </div>
            <button
                type="submit"
                disabled={isSubmitting}
                className="w-full py-2 px-4 bg-blue-600 text-white rounded font-semibold hover:bg-blue-700 transition"
            >
                {isSubmitting ? 'Submitting...' : 'Create Group'}
            </button>
        </Form>
    );
 }

 async function loadFriends() {
     const response = await sendHttpRequest("/friend/GetFriendsList", {
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
         friendsList: loadFriends(),
     };
 }

 export async function action({ request }) {
    const formData = await request.formData();
    let groupModel = {
        GroupName: formData.get("name"),
        DefaultCoinBalance: formData.get("coins"),
        GroupUsernames: JSON.parse(formData.get("friends")),
    };

    console.log(groupModel);

    const response = await sendHttpRequest("/group/CreateGroup", {
        method: request.method,
        headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + getAuthToken(),
        },
        body: JSON.stringify(groupModel),
    });

        if (!response.ok) {
        throw new Response(
            JSON.stringify({ message: "Could not create group" }),
            {
                status: 422,
            }
        );
    }

    return redirect("/groups");



     }
 
 