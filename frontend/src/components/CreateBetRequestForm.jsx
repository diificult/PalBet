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
import UserItem from "./UserItem";
import { Suspense, useState } from "react";
import { sendHttpRequest } from "../hooks/useHttp";
import { getAuthToken } from "../util/auth";

export default function CreateBetRequestForm({ method, mode = 'friends', groupId = null }) {
    const { participants } = useLoaderData();
    const [selectedParticipants, setSelectedParticipants] = useState(() => []);
    const [selectedMode, setSelectedMode] = useState("coins");
    const [selectedOutcomeType, setSelectedOutcomeType] = useState("ParticipantAssigned");

    const handleParticipants = (event, newFormats) => {
        setSelectedParticipants(newFormats);
    };

    const navigation = useNavigation();
    const isSubmitting = navigation.state === "submitting";

    const now = new Date();
    const local = new Date(now.getTime() - now.getTimezoneOffset() * 60000);
    const formattedNow = local.toISOString().slice(0, 16);
    /*
        
    public string BetDescription { get; set; }
    public int? BetStakeCoins { get; set; }
    
    public string? BetStakeUserInput { get; set; }
    
    public DateTime? Deadline { get; set; }
    
    public List<string> ParticipantUsernames { get; set; }


        public bool AllowMultipleWinners { get; set; } = false;
        public bool AllowUserSubmittedChoices { get; set; } = false;
        public bool BurnStakeOnNoWnner { get; set; } = true;

    */


    const [numChoices, setNumChoices] = useState(2);

    return (
        <>
            <Form method={method} className="flex flex-col items-start m-10">
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

                <div className="font-extrabold p-10 m-5">
                    <label className="font-extrabold">Deadline? (optional) </label>
                    <Input
                        id="deadline"
                        name="deadline"
                        type="datetime-local"
                        min={formattedNow}
                    ></Input>
                </div>
                <input
                    type="hidden"
                    name="friends"
                    value={JSON.stringify(selectedParticipants)}
                />
                <div className="font-extrabold p-10 m-5 flex">
                    <div className="font-extrabold p-10 m-5">
                        <label className="font-extrabold">Settings:</label>    
                        <select name="BetMode" className="m-4" onChange={(e) => setSelectedOutcomeType(e.target.value)} >
                            <option value="ParticipantAssigned">Default Options ("x" player wins)</option>
                            <option value="HostDefined">Host Defined Choices</option>
                            <option value="UserSubmitted">User Submitted Choices</option>
                        </select>
                        <input type="checkbox" name="AllowMultipleWinners" /> Allow Multiple Winners
                        <input type="checkbox" name="BurnStakeOnNoWnner" defaultChecked={true} /> Burn Stake On No Winner
                    </div>
                </div>

                {selectedOutcomeType === "HostDefined" && (
                    <div className="font-extrabold p-10 m-5">
                        <label className="font-extrabold">Number of Choices (1-32):</label>
                        <Input
                            type="number"
                            min="1"
                            max="32"
                            value={numChoices}
                            onChange={(e) => setNumChoices(Math.min(32, Math.max(1, parseInt(e.target.value) || 1)))}
                            className="w-20 h-10 p-2 m-4"
                        />
                        <div className="font-extrabold p-10 m-5">
                            <label className="font-extrabold">Input Choices:</label>
                            {Array.from({ length: numChoices }, (_, i) => (
                                <Input
                                    key={`choice_${i}`}
                                    id={`choice_${i}`}
                                    type="text"
                                    name={`choice_${i}`}
                                    placeholder={`Choice ${i + 1}`}
                                    variant="outline"
                                    required
                                    className="w-96 h-10 p-2 m-4"
                                />
                            ))}
                        </div>
                    </div>
                )}
                    
                <div className="font-extrabold p-10 m-5">
                    <label className="font-extrabold">Select Participants(s)</label>
                    <Suspense
                        fallback={
                            <p style={{ textAlign: "center" }}>Loading...</p>
                        }
                    >
                        <Await resolve={participants}>
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
                                        value={selectedParticipants}
                                        onChange={handleParticipants}
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

export async function loader({ params }) {
    const groupId = params.groupId;
    
    if (groupId) {
        return {
            participants: loadGroupMembers(groupId),
        };
    }
    
    return {
        participants: loadFriends(),
    };
}

async function loadGroupMembers(groupId) {
    const response = await sendHttpRequest(`/group/${groupId}/GetMembers`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + getAuthToken(),
        },
    });
    
    if (!response.ok) {
        throw new Response(
            JSON.stringify({ message: "Could not get group members list" }),
            {
                status: 422,
            }
        );
    }
    
    return response.json();
}

export async function action({ request, params }) {
    const groupId = params.groupId;
    const formData = await request.formData();
    let betModel = {
        ParticipantUsernames: JSON.parse(formData.get("friends")),
        BetDescription: formData.get("desc"),
    };
    if (formData.get("deadline")) {
        betModel = {...betModel, Deadline: formData.get("deadline")}
    }
    if (formData.get("betType") == "coin") {
        betModel = { ...betModel, BetStakeCoins: formData.get("bet") };
    } else if (formData.get("betType") == "input") {
        betModel = { ...betModel, BetStakeUserInput: formData.get("bet") };
    } else {
        console.log("error getting data");
        return null;
    }
    if (groupId) {
        betModel = { ...betModel, GroupId: groupId };
    }
    const outcomeType = formData.get("BetMode");
    if (outcomeType === "HostDefined") {
        const choices = [];
        for (const [key, value] of formData.entries()) {
        if (key.startsWith('choice_')) {
            choices.push(value);
        }
    }       
        betModel = { ...betModel, ChoicesText: choices };
    }
    betModel = {
        ...betModel,
        OutcomeChoice: outcomeType,}

    if (formData.get("AllowMultipleWinners")) {
        betModel = { ...betModel, AllowMultipleWinners: true };
    } else {
        betModel = { ...betModel, AllowMultipleWinners: false };
    }
    if (formData.get("BurnStakeOnNoWnner")) {
        betModel = { ...betModel, BurnStakeOnNoWinner: true };
    } else {
        betModel = { ...betModel, BurnStakeOnNoWinner: false };
    }

    console.log(betModel);
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
