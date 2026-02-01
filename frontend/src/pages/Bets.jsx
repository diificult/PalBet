import {
    Await,
    Link,
    NavLink,
    redirect,
    useLoaderData,
} from "react-router-dom";
import { sendHttpRequest } from "../hooks/useHttp";
import { getAuthToken } from "../util/auth";
import { Suspense, useState } from "react";
import BetList from "../components/BetList";
import ChooseWinner from "../components/ChooseWinner";
import HostDefinedChoices from "../components/HostDefinedChoices";
import UserDefinedChoices from "../components/UserDefinedChoices";


export default function BetsPage() {
    const { betRequests, betRequested, openBets, completedBets } = useLoaderData();
    const [activeTab, setActiveTab] = useState(0);
    const tabList = [
        {
            label: 'Bet Requests',
            data: betRequests,
            mode: 'betRequest',
        },
        {
            label: 'Requested Bets',
            data: betRequested,
            mode: 'betRequested',
        },
        {
            label: 'Open Bets',
            data: openBets,
            mode: 'openBet',
        },
        {
            label: 'Completed Bets',
            data: completedBets,
            mode: 'completedBet',
        },
    ];
    return (
        <>
            <ChooseWinner />
            <HostDefinedChoices />
            <UserDefinedChoices />

            <div className="flex items-center justify-between mb-6">
                <h1 className="text-2xl font-semibold">Bets</h1>
                <NavLink
                    to="new"
                    className="inline-flex items-center gap-2 bg-green-600 rounded px-4 py-2 text-white shadow hover:bg-green-700 transition"
                >
                    Create new bet
                </NavLink>
            </div>

            <div className="w-full max-w-3xl mx-auto">
                <div className="flex border-b border-gray-200 mb-4">
                    {tabList.map((tab, idx) => (
                        <button
                            key={tab.label}
                            className={`px-4 py-2 font-medium focus:outline-none transition border-b-2 -mb-px ${activeTab === idx ? 'border-blue-600 text-blue-600 bg-gray-50' : 'border-transparent text-gray-500 hover:text-blue-600 hover:bg-gray-50'}`}
                            onClick={() => setActiveTab(idx)}
                        >
                            <span>{tab.label}</span>
                            <Suspense fallback={<span className="ml-2 text-xs bg-gray-100 rounded px-2">…</span>}>
                                <Await resolve={tab.data}>
                                    {(loaded) => (
                                        <span className="ml-2 text-xs bg-gray-100 rounded px-2">
                                            {loaded?.length ?? 0}
                                        </span>
                                    )}
                                </Await>
                            </Suspense>
                        </button>
                    ))}
                </div>
                <div className="rounded-lg shadow bg-white border border-gray-200 p-6 min-h-[300px]">
                    <Suspense fallback={<p className="text-sm text-gray-500">Loading...</p>}>
                        <Await resolve={tabList[activeTab].data}>
                            {(loadedBets) => (
                                <BetList
                                    bets={loadedBets}
                                    mode={tabList[activeTab].mode}
                                />
                            )}
                        </Await>
                    </Suspense>
                </div>
            </div>
        </>
    );
}

async function loadBets(domain) {
    const response = await sendHttpRequest(`/Bet${domain}`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + getAuthToken(),
        },
    });
    if (!response.ok) {
        throw new Response(
            JSON.stringify({ message: "Could not get bet requests" }),
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
        betRequests: loadBets("/requests"),
        betRequested: loadBets("/state?state=1"),
        openBets: loadBets("/state?state=3"),
        completedBets: loadBets("/state?state=4"),
    };
}

export async function action({ request }) {
    console.log(request);
    const formData = await request.formData();
    const actionType = formData.get("action");
    let data = formData.get("betId");
    
    let endpoint = "";
    if (actionType === "accept") {
        endpoint = `/Bet/${data}/AcceptBet`;
        const outcomeChoice = formData.get("outcomeChoice");
        data = outcomeChoice;
    }
    if (actionType === "cancel") endpoint = "/Bet/CancelBet";
    if (actionType === "winner") {
        endpoint = `/Bet/${data}/winner`;
        console.log(request.Username);
        data = { betId: data, winnerUsername: formData.get("winnerUsername") };
    }

    const response = await sendHttpRequest(endpoint, {
        method: request.method,
        headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + getAuthToken(),
        },
        body: JSON.stringify(data),
    });
    if (!response.ok) {
        throw new Response(
            JSON.stringify({ message: "Could not modify bet status" }),
            {
                status: 422,
            }
        );
    }
    return redirect("/bets");
}
