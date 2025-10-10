import { Button, Input, ToggleButton, ToggleButtonGroup } from "@mui/material";
import {
    Await,
    Form,
    Link,
    redirect,
    useActionData,
    useLoaderData,
    useNavigation,
} from "react-router-dom";
import FriendItem from "./FriendItem";
import { Suspense, useState } from "react";
import { sendHttpRequest } from "../hooks/useHttp";
import { getAuthToken } from "../util/auth";

export default function CreateBetRequestForm({ method }) {
    const { friendsList } = useLoaderData();
    const [friends, setFriends] = useState(() => []);
    const [selectedMode, setSelectedMode] = useState("coins");

    const handleFriends = (event, newFormats) => {
        setFriends(newFormats);
    };

    const navigation = useNavigation();
    const isSubmitting = navigation.state === "submitting";

    const now = new Date();
    const local = new Date(now.getTime() - now.getTimezoneOffset() * 60000);
    const formattedNow = local.toISOString().slice(0, 16);

    return (
        <>
            <Form method={method} className="p-5 m-5">
                <div className="font-extrabold p-10 m-5">
                    <label className="font-extrabold">Bet Description</label>
                    <Input
                        id="desc"
                        type="text"
                        name="desc"
                        required
                        variant="outline"
                        className="w-96 h-10 p-2 m-4 "
                    ></Input>
                </div>
                <div className="font-extrabold p-10 m-5">
                    <label className="font-extrabold">Bet Type</label>
                    <select
                        name="betType"
                        value={selectedMode}
                        onChange={(e) => setSelectedMode(e.target.value)}
                    >
                        <option value="coin">Coins</option>
                        <option value="input">User Input</option>
                    </select>
                    <label className="font-extrabold">Bet Value</label>
                    <Input
                        id="bet"
                        type={selectedMode == "coin" ? "number" : "text"}
                        name="bet"
                        variant="outline"
                        required
                        className="w-64 h-10 p-2 m-4 "
                    ></Input>
                </div>

                <div className="font-extrabold p-10 m-5 hidden">
                    <Input
                        id="deadline"
                        type="datetime-local"
                        min={formattedNow}
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
                                                <FriendItem
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
    let betModel = {
        ParticipantUsernames: JSON.parse(formData.get("friends")),
        BetDescription: formData.get("desc"),
    };
    if (formData.get("betType") == "coin") {
        betModel = { ...betModel, BetStakeCoins: formData.get("bet") };
    } else if (formData.get("betType") == "input") {
        betModel = { ...betModel, BetStakeUserInput: formData.get("bet") };
    } else {
        console.log("error getting data");
        return null;
    }

    const response = await sendHttpRequest("/Bet/CreateBet", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + getAuthToken(),
        },
        body: JSON.stringify(betModel),
    });

    if (!response.ok) {
        throw new Response(
            JSON.stringify({ message: "Could not create bet" }),
            {
                status: 422,
            }
        );
    }
    return redirect("/bets");
}
