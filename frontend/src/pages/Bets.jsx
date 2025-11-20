import {
    Await,
    Link,
    NavLink,
    redirect,
    useLoaderData,
} from "react-router-dom";
import { sendHttpRequest } from "../hooks/useHttp";
import { getAuthToken } from "../util/auth";
import { Suspense } from "react";
import BetList from "../components/BetList";
import ChooseWinner from "../components/ChooseWinner";
import HostDefinedChoices from "../components/HostDefinedChoices";
import UserDefinedChoices from "../components/UserDefinedChoices";

export default function BetsPage() {
    const { betRequests, betRequested, openBets, completedBets } =
        useLoaderData();
    return (
        <>
            <ChooseWinner />
            <HostDefinedChoices />
            <UserDefinedChoices />

            <div className="flex items-center justify-between mb-6">
                <h1 className="text-2xl font-semibold">Bets</h1>
                <NavLink
                    to="new"
                    className="inline-flex items-center gap-2 bg-green-600 rounded-sm px-4 py-1 text-white shadow-sm hover:bg-green-700 transition"
                >
                    Create new bet
                </NavLink>
            </div>

            <div className="w-full">
                <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6 w-full h-full items-start">
                    <div className="bg-white border border-gray-200 rounded-lg p-4 shadow-sm flex flex-col">
                        <div className="flex items-center justify-between mb-3">
                            <p className="text-lg font-medium">Bet Requests</p>
                            <Suspense fallback={<span className="inline-block px-2 py-0.5 text-sm bg-gray-100 rounded">…</span>}>
                                <Await resolve={betRequests}>
                                    {(loaded) => (
                                        <span className="inline-block px-2 py-0.5 text-sm bg-gray-100 rounded">
                                            {loaded?.length ?? 0}
                                        </span>
                                    )}
                                </Await>
                            </Suspense>
                        </div>
                        <div className="border-t border-gray-100 pt-3 overflow-auto">
                            <Suspense fallback={<p className="text-sm text-gray-500">Loading...</p>}>
                                <Await resolve={betRequests}>
                                    {(loadedBetRequests) => (
                                        <BetList
                                            bets={loadedBetRequests}
                                            mode="betRequest"
                                        />
                                    )}
                                </Await>
                            </Suspense>
                        </div>
                    </div>

                    <div className="bg-white border border-gray-200 rounded-lg p-4 shadow-sm flex flex-col">
                        <div className="flex items-center justify-between mb-3">
                            <p className="text-lg font-medium">Requested Bets</p>
                            <Suspense fallback={<span className="inline-block px-2 py-0.5 text-sm bg-gray-100 rounded">…</span>}>
                                <Await resolve={betRequested}>
                                    {(loaded) => (
                                        <span className="inline-block px-2 py-0.5 text-sm bg-gray-100 rounded">
                                            {loaded?.length ?? 0}
                                        </span>
                                    )}
                                </Await>
                            </Suspense>
                        </div>
                        <div className="border-t border-gray-100 pt-3 overflow-auto">
                            <Suspense fallback={<p className="text-sm text-gray-500">Loading...</p>}>
                                <Await resolve={betRequested}>
                                    {(loadedBetRequested) => (
                                        <BetList
                                            bets={loadedBetRequested}
                                            mode="betRequested"
                                        />
                                    )}
                                </Await>
                            </Suspense>
                        </div>
                    </div>

                    <div className="bg-white border border-gray-200 rounded-lg p-4 shadow-sm flex flex-col">
                        <div className="flex items-center justify-between mb-3">
                            <p className="text-lg font-medium">Open Bets</p>
                            <Suspense fallback={<span className="inline-block px-2 py-0.5 text-sm bg-gray-100 rounded">…</span>}>
                                <Await resolve={openBets}>
                                    {(loaded) => (
                                        <span className="inline-block px-2 py-0.5 text-sm bg-gray-100 rounded">
                                            {loaded?.length ?? 0}
                                        </span>
                                    )}
                                </Await>
                            </Suspense>
                        </div>
                        <div className="border-t border-gray-100 pt-3 overflow-auto">
                            <Suspense fallback={<p className="text-sm text-gray-500">Loading...</p>}>
                                <Await resolve={openBets}>
                                    {(loadedOpenBets) => (
                                        <BetList
                                            bets={loadedOpenBets}
                                            mode="openBet"
                                        />
                                    )}
                                </Await>
                            </Suspense>
                        </div>
                    </div>

                    <div className="bg-white border border-gray-200 rounded-lg p-4 shadow-sm flex flex-col">
                        <div className="flex items-center justify-between mb-3">
                            <p className="text-lg font-medium">Completed Bets</p>
                            <Suspense fallback={<span className="inline-block px-2 py-0.5 text-sm bg-gray-100 rounded">…</span>}>
                                <Await resolve={completedBets}>
                                    {(loaded) => (
                                        <span className="inline-block px-2 py-0.5 text-sm bg-gray-100 rounded">
                                            {loaded?.length ?? 0}
                                        </span>
                                    )}
                                </Await>
                            </Suspense>
                        </div>
                        <div className="border-t border-gray-100 pt-3 overflow-auto">
                            <Suspense fallback={<p className="text-sm text-gray-500">Loading...</p>}>
                                <Await resolve={completedBets}>
                                    {(loadedCompletedBets) => (
                                        <BetList
                                            bets={loadedCompletedBets}
                                            mode="completedBet"
                                        />
                                    )}
                                </Await>
                            </Suspense>
                        </div>
                    </div>
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
        betRequests: loadBets("/GetBetRequests"),
        betRequested: loadBets("/GetBetFromState?state=1"),
        openBets: loadBets("/GetBetFromState?state=3"),
        completedBets: loadBets("/GetBetFromState?state=4"),
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
        endpoint = "/Bet/ChooseWinner";
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
