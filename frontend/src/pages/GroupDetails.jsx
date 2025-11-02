import { Suspense } from "react";
import UsersList from "../components/UsersList";
import { sendHttpRequest } from "../hooks/useHttp";
import { getAuthToken } from "../util/auth";
import { Await, useLoaderData } from "react-router-dom";
import BetList from "../components/BetList";

export default function GroupDetailsPage() {
    const { groupDetails } = useLoaderData();
    return (
        <>
        <Suspense fallback={<p>Loading....</p>} >
        <Await resolve={groupDetails}>
            {(loadedGroup) => (
                <>
                <h1 className="text-2xl font-bold mb-4">{loadedGroup.name}</h1>
                <div className="bg-white border border-gray-200 rounded-lg shadow-sm p-4">
                            <h2 className="text-lg font-medium mb-3">Group Members</h2>
                            <div className="border-t border-gray-100 pt-3">
                    <UsersList Users={loadedGroup.users} mode="groupMember" /> 
                            </div>
                </div>
                <div className="mt-6 bg-white border border-gray-200 rounded-lg shadow-sm p-4">
                    <h2 className="text-lg font-medium mb-3">Group Bets</h2>
                    <div className="border-t border-gray-100 pt-3">
                        <BetList bets={loadedGroup.bets} mode="groupBet" />
                    </div>
                </div>

                <div className="mt-6 bg-white border border-gray-200 rounded-lg shadow-sm p-4">
                    <h2 className="text-lg font-medium mb-3">Group Settings</h2>
                    <div className="border-t border-gray-100 pt-3">
                        <div className="text-sm text-gray-500">
                            {/* Group ID temporary just incase i need it for debugging */}
                            <p>Group ID: {loadedGroup.id}</p>
                            <div className="mt-2 flex items-center gap-4">
                                <p className="font-medium">Default Starting Balance:</p>
                                <input type="number"
                                    value={loadedGroup.defaultCoinAmount}
                                    readOnly />
                                 </div>
                       
                    </div>
                </div>
                </div>
                </>
            )}
        </Await>
        </Suspense>
           

        </>
    );
}

export async function loader({ params }) {
    const groupId = params.groupId;
    const response = await sendHttpRequest(`/group/${groupId}/GetDetails`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + getAuthToken(),
        },
    });

    if (!response.ok) {
          throw new Response(
            JSON.stringify({ message: "Could not create group" }),
            {
                status: 422,
            }
        );
    }

    const groupDetails = await response.json();

    console.log("Loaded group details:", groupDetails);
    return {
        groupDetails: groupDetails,
    };
}