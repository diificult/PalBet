// MUI removed except icons. Use native elements and Tailwind for layout.
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


export default function CreateBetRequestForm({ method }) {
    const { participants } = useLoaderData();
    const [selectedParticipants, setSelectedParticipants] = useState([]);
    const [selectedMode, setSelectedMode] = useState("coins");
    const [selectedOutcomeType, setSelectedOutcomeType] = useState("ParticipantAssigned");
    const [numChoices, setNumChoices] = useState(2);
    const [hostChoices, setHostChoices] = useState(["", ""]);

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


    return (
        <Form method={method} className="space-y-8 p-6 w-full max-w-xl bg-white rounded shadow mx-auto">
            <div>
                <label htmlFor="desc" className="block font-semibold mb-2">Bet Description</label>
                <input
                    id="desc"
                    type="text"
                    name="desc"
                    required
                    className="w-full border border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                />
            </div>
            <div>
                <label className="block font-semibold mb-2">Bet Type</label>
                <select
                    name="betType"
                    value={selectedMode}
                    onChange={e => setSelectedMode(e.target.value)}
                    className="w-full border border-gray-300 rounded px-3 py-2 mb-2"
                >
                    <option value="coin">Coins</option>
                    <option value="input">User Input</option>
                </select>
                <label htmlFor="bet" className="block font-semibold mb-2">Bet Value</label>
                <input
                    id="bet"
                    type={selectedMode === "coin" ? "number" : "text"}
                    name="bet"
                    required
                    className="w-full border border-gray-300 rounded px-3 py-2"
                />
            </div>
            <div>
                <label htmlFor="deadline" className="block font-semibold mb-2">Deadline? (optional)</label>
                <input
                    id="deadline"
                    name="deadline"
                    type="datetime-local"
                    min={formattedNow}
                    className="w-full border border-gray-300 rounded px-3 py-2"
                />
            </div>
            <input
                type="hidden"
                name="friends"
                value={JSON.stringify(selectedParticipants)}
            />
            <div>
                <label className="block font-semibold mb-2">Settings</label>
                <select
                    name="BetMode"
                    className="w-full border border-gray-300 rounded px-3 py-2 mb-2"
                    value={selectedOutcomeType}
                    onChange={e => {
                        setSelectedOutcomeType(e.target.value);
                        if (e.target.value === "HostDefined") {
                            setNumChoices(2);
                            setHostChoices(["", ""]);
                        }
                    }}
                >
                    <option value="ParticipantAssigned">Default Options ("x" player wins)</option>
                    <option value="HostDefined">Host Defined Choices</option>
                    <option value="UserSubmitted">User Submitted Choices</option>
                </select>
                <div className="flex items-center gap-4 mt-2">
                    <label className="inline-flex items-center">
                        <input type="checkbox" name="AllowMultipleWinners" className="accent-blue-600" />
                        <span className="ml-2">Allow Multiple Winners</span>
                    </label>
                    <label className="inline-flex items-center">
                        <input type="checkbox" name="BurnStakeOnNoWnner" defaultChecked={true} className="accent-blue-600" />
                        <span className="ml-2">Burn Stake On No Winner</span>
                    </label>
                </div>
            </div>
            {selectedOutcomeType === "HostDefined" && (
                <div className="border rounded p-4 bg-gray-50">
                    <label className="block font-semibold mb-2">Number of Choices (2-32):</label>
                    <input
                        type="number"
                        min="2"
                        max="32"
                        value={numChoices}
                        onChange={e => {
                            const val = Math.max(2, Math.min(32, parseInt(e.target.value) || 2));
                            setNumChoices(val);
                            setHostChoices(arr => {
                                const next = arr.slice(0, val);
                                while (next.length < val) next.push("");
                                return next;
                            });
                        }}
                        className="w-20 border border-gray-300 rounded px-2 py-1 mb-2"
                    />
                    <div className="space-y-2 mt-2">
                        <label className="block font-semibold mb-1">Input Choices:</label>
                        {Array.from({ length: numChoices }, (_, i) => (
                            <input
                                key={`choice_${i}`}
                                id={`choice_${i}`}
                                type="text"
                                name={`choice_${i}`}
                                placeholder={`Choice ${i + 1}`}
                                required
                                value={hostChoices[i] || ""}
                                onChange={e => {
                                    setHostChoices(arr => {
                                        const next = arr.slice();
                                        next[i] = e.target.value;
                                        return next;
                                    });
                                }}
                                className="w-full border border-gray-300 rounded px-3 py-2"
                            />
                        ))}
                    </div>
                </div>
            )}
            <div>
                <label className="block font-semibold mb-2">Select Participants(s)</label>
                <Suspense fallback={<p className="text-center">Loading...</p>}>
                    <Await resolve={participants}>
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
                                        <label key={friend.username} className={`flex items-center border rounded px-3 py-2 cursor-pointer transition ${selectedParticipants.includes(friend.username) ? 'bg-blue-100 border-blue-400' : 'bg-white border-gray-300'}`}>
                                            <input
                                                type="checkbox"
                                                className="mr-2 accent-blue-600"
                                                checked={selectedParticipants.includes(friend.username)}
                                                onChange={e => {
                                                    if (e.target.checked) {
                                                        setSelectedParticipants([...selectedParticipants, friend.username]);
                                                    } else {
                                                        setSelectedParticipants(selectedParticipants.filter(f => f !== friend.username));
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
                {isSubmitting ? 'Submitting...' : 'Create Bet'}
            </button>
        </Form>
    );
}


export async function loader({ params }) {
    const groupId = params.groupId;
    let participants;
    if (groupId) {
        participants = await loadGroupMembers(groupId);
    } else {
        participants = await loadFriends();
    }
    return { participants };
}
async function loadFriends() {
    const response = await sendHttpRequest("/friend/me", {
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
    return response.json();
}

async function loadGroupMembers(groupId) {
    const response = await sendHttpRequest(`/group/${groupId}/members`, {
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
    console.log("creating bets");
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
    const response = await sendHttpRequest("/Bet/create", {
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
