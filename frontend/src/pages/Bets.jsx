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

export default function BetsPage() {
    const { betRequests, betRequested, openBets, completedBets } =
        useLoaderData();
    return (
        <>
            <ChooseWinner />
            <NavLink
                to="new"
                className="bg-green-600 rounded-sm p-4 m-4 drop-shadow-2xl text-white "
            >
                Create new bet
            </NavLink>
            <div className="flex justify-center">
                <div className="grid grid-cols-4 gap-6 w-full min-h-screen px-8">
                    <div>
                        <p className="text-3xl">Bet Requests</p>
                        <Suspense fallback={<p>Loading...</p>}>
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
                    <div>
                        <p className="text-3xl">Requested Bets</p>
                        <Suspense fallback={<p>Loading...</p>}>
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
                    <div>
                        <p className="text-3xl">Open Bets</p>
                        <Suspense fallback={<p>Loading...</p>}>
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
                    <div>
                        <p className="text-3xl">Completed Bets</p>
                        <Suspense fallback={<p>Loading...</p>}>
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
    if (actionType === "accept") endpoint = "/Bet/AcceptBet";
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
