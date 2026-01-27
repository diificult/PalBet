import { Await, useRouteLoaderData } from "react-router-dom";
import { sendHttpRequest } from "../hooks/useHttp";
import { Suspense } from "react";
import { getAuthToken } from "../util/auth";

export default function BetDetailPage() {
    const { bet } = useRouteLoaderData("bet-detail");

    return (
        <Suspense fallback={<p className="text-sm text-gray-500">Loading...</p>}>
            <Await resolve={bet}>
                {(loadedBet) => (
                    <div className="max-w-4xl mx-auto">
                        <div className="flex items-center justify-between mb-6">
                            <div>
                                <h1 className="text-2xl font-semibold">Bet details</h1>
                                <p className="text-sm text-gray-500">#{loadedBet.betId} • {loadedBet.groupName ?? 'Personal'}</p>
                            </div>
                            <div className="text-right">
                                <span className="inline-flex items-center px-3 py-1 rounded-full text-sm font-medium bg-gray-100 text-gray-800 border border-gray-200">
                                    {loadedBet.betState}
                                </span>
                            </div>
                        </div>

                        <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
                            <div className="lg:col-span-2 rounded-lg shadow bg-white border border-gray-200 p-6">
                                <h2 className="text-lg font-medium mb-2">{loadedBet.betDescription}</h2>

                                <div className="grid grid-cols-2 gap-4 my-4">
                                    <div>
                                        <h3 className="text-sm text-gray-500">Stake</h3>
                                        <p className="font-medium">{loadedBet.betStake ?? '—'}</p>
                                    </div>
                                    <div>
                                        <h3 className="text-sm text-gray-500">Deadline</h3>
                                        <p className="font-medium">{loadedBet.deadline ?? 'No deadline'}</p>
                                    </div>
                                </div>

                                {/* Choices: show available options for this bet */}
                                <div className="mt-4">
                                    <h3 className="text-sm text-gray-500 mb-2">Choices</h3>
                                    {loadedBet.choices && loadedBet.choices.length > 0 ? (
                                        <div className="flex flex-wrap gap-2">
                                            {loadedBet.choices.map((c, i) => (
                                                <span key={i} className="px-2 py-1 bg-gray-100 text-gray-800 text-sm rounded border border-gray-200">{String(c ?? '').trim() || '—'}</span>
                                            ))}
                                        </div>
                                    ) : (
                                        <p className="text-sm text-gray-500">No choices available</p>
                                    )}
                                </div>

                                <div className="mt-4">
                                    <h3 className="text-sm text-gray-500 mb-2">Participants</h3>
                                    <ul className="space-y-2">
                                        {loadedBet.participantNames && loadedBet.participantNames.length > 0 ? (
                                            loadedBet.participantNames.map((p, idx) => {
                                                const username = p?.item1 ?? 'Unknown';
                                                const choice = p?.item2;
                                                return (
                                                    <li key={idx} className="flex items-center gap-3">
                                                        <div className="w-8 h-8 rounded-full bg-blue-100 text-blue-700 flex items-center justify-center font-medium">{username.charAt(0).toUpperCase()}</div>
                                                        <div className="flex-1">
                                                            <div className="flex items-center justify-between">
                                                                <div className="font-medium">{username}</div>
                                                                <div className="text-sm text-gray-500">
                                                                    {choice ? (
                                                                        <span className="inline-flex items-center px-2 py-0.5 rounded bg-green-50 text-green-700 border border-green-100">{choice}</span>
                                                                    ) : (
                                                                        <span className="italic text-gray-400">(not accepted)</span>
                                                                    )}
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </li>
                                                );
                                            })
                                        ) : (
                                            <li className="text-sm text-gray-500">No participants</li>
                                        )}
                                    </ul>
                                </div>

                                <div className="mt-6">
                                    <h3 className="text-sm text-gray-500 mb-2">Winners</h3>
                                    {loadedBet.userWinner && loadedBet.userWinner.length > 0 ? (
                                        <ul className="list-disc pl-5">
                                            {loadedBet.userWinner.map((w, i) => (
                                                <li key={i}>{w}</li>
                                            ))}
                                        </ul>
                                    ) : (
                                        <p className="text-sm text-gray-500">No winners chosen yet.</p>
                                    )}
                                </div>
                            </div>

                            <div className="space-y-6">
                                <div className="rounded-lg shadow bg-white border border-gray-200 p-4">
                                    <h3 className="text-sm font-medium mb-3">Settings</h3>
                                    <div className="space-y-3 text-sm text-gray-700">
                                        <div className="flex items-center justify-between">
                                            <div className="text-gray-500">Allow multiple winners</div>
                                            <div className="font-medium">{loadedBet.allowMultipleWinners ? 'Yes' : 'No'}</div>
                                        </div>
                                        <div className="flex items-center justify-between">
                                            <div className="text-gray-500">Burn stake on no winner</div>
                                            <div className="font-medium">{loadedBet.burnStakeOnNoWnner ? 'Yes' : 'No'}</div>
                                        </div>
                                        <div className="flex items-center justify-between">
                                            <div className="text-gray-500">Outcome choice</div>
                                            <div className="font-medium">{typeof loadedBet.outcomeChoice === 'number' ? loadedBet.outcomeChoice : '—'}</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                )}
            </Await>
        </Suspense>
    );
}

export async function loader({params, request}) {
    const id = params.betId;

    return {
        bet: loadBet(id),
    }
}

async function loadBet(id) {
        var response = await sendHttpRequest("/Bet/"+id, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + getAuthToken(),
        }
    });
        if (!response.ok) {
        throw new Response(
            JSON.stringify({
                message: "Could not fetch details for selected bet",
            }),
            { status: 500 }
        );
    } else {
        
        const resData = await response.json();
        console.log(resData);
        return resData;
    }
}