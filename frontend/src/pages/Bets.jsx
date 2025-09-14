import { Await, Link, NavLink, useLoaderData } from "react-router-dom";
import { sendHttpRequest } from "../hooks/useHttp";
import { getAuthToken } from "../util/auth";
import { Suspense } from "react";
import BetList from "../components/BetList";

export default function BetsPage() {
    const { betRequests, betRequested, openBets, completedBets } =
        useLoaderData();
    return (
        <div className="justify-center gap-16">
            <NavLink
                to="new"
                className="bg-green-600 rounded-sm p-4 drop-shadow-2xl text-white "
            >
                Create new bet
            </NavLink>

            <div className="grid grid-cols-3 grid-rows-2 justify-between gap-8 pl-64">
                <div className="col-start-1 col-end-1 row-auto">
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
                <div className="col-start-1 col-end-1 row-start-2 row-end-2">
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
                <div className="col-start-2 col-end-2 row-start-1 row-end-2">
                    <p className="text-3xl">Open Bets</p>
                    <Suspense fallback={<p>Loading...</p>}>
                        <Await resolve={openBets}>
                            {(loadedOpenBets) => (
                                <BetList bets={loadedOpenBets} mode="openBet" />
                            )}
                        </Await>
                    </Suspense>
                </div>
                <div className="col-start-3 col-end-3 row-start-1 row-end-2">
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
