import { Button, Input, ToggleButton, ToggleButtonGroup } from "@mui/material";
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
        <>
            <Form method={method} className="p-5 m-5">
                <div className="font-extrabold p-10 m-5">
                    <label className="font-extrabold">Group Name</label>
                    <Input  
                        id="name"
                        type="text"
                        name="name"
                        required
                        variant="outline"
                        className="w-96 h-10 p-2 m-4 "
                    ></Input>
                </div>

                <div className="font-extrabold p-10 m-5">
                    <label className="font-extrabold">Default Coins per User (optional) </label>
                    <Input
                        id="coins"
                        name="coins"
                        type="number"
                        min="0"
                    ></Input>
                </div>
                <input
                    type="hidden"
                    name="friends"
                    value={JSON.stringify(friends)}
                />
                <div className="font-extrabold p-10 m-5">
                    <label className="font-extrabold">Select Friend(s)</label>
                    <Suspense
                        fallback={
                            <p style={{ textAlign: "center" }}>Loading...</p>
                        }
                    >
                        <Await resolve={friendsList}>
                            {(loadedFriends) => (
                                <>
                                    {loadedFriends.length === 0 && (
                                        <p>
                                            You currently have no friends added.
                                            <Link to="/friends">
                                                Click here
                                            </Link>
                                             to add some!
                                        </p>
                                    )}

                                    <ToggleButtonGroup
                                        value={friends}
                                        onChange={handleFriends}
                                        fullWidth
                                    >
                                        {loadedFriends.map((friend) => (
                                            <ToggleButton
                                                value={friend.username}
                                                key={friend.username}
                                            >
                                                <UserItem
                                                    friend={friend}
                                                    mode="selector"
                                                />
                                            </ToggleButton>
                                        ))}
                                    </ToggleButtonGroup>
                                </>
                            )}
                        </Await>
                    </Suspense>
                </div>
                <Button
                    variant="contained"
                    type="submit"
                    disabled={isSubmitting}
                >
                    Submit
                </Button>
            </Form>
        </>
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
 
 