import { Await, useLoaderData, useRouteLoaderData } from "react-router-dom";
import { sendHttpRequest } from "../hooks/useHttp";
import { Suspense } from "react";
import { getAuthToken } from "../util/auth";

export default function BetDetailPage() {
    const {bet} = useRouteLoaderData("bet-detail");
    return(
        <> 
        <Suspense fallback={<p>Loading....</p>} >
        <Await resolve={bet}>
            {(loadedBet) => (
                <>
                <div><strong>Description:</strong> <p>{loadedBet.betDescription}</p></div>
            
                <div>
                <strong>Stake</strong>
                <p>{loadedBet.betStake}</p>
            </div>

                {
                loadedBet.deadline && 
                    <div><strong>Deadline: </strong> {loadBet.Deadline  }</div>
                }
            

            <div><strong>Participants</strong> 
                            <ul>
                    {loadedBet.participantNames && loadedBet.participantNames.length > 0 ? (
                        loadedBet.participantNames.map((p, idx) => <li key={idx}>{p}</li>)
                    ) : (
                        <li>No participants</li>
                    )}
                </ul>
            </div>
                </>
            )}
            

        </Await>
        </Suspense>
        
            
        </>
    );
}

export async function loader({params, request}) {
    const id = params.betId;

    return {
        bet: loadBet(id),
    }
}

async function loadBet(id) {
        var response = await sendHttpRequest("/Bet/GetBetFromId/"+id, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + getAuthToken(),
        }
    });
        if (!response.ok) {
        throw new Response(
            JSON.stringify({
                message: "Could not fetch details for selected event",
            }),
            { status: 500 }
        );
    } else {
        
        const resData = await response.json();
        console.log(resData);
        return resData;
    }
}