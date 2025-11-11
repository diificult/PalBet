import { Suspense } from "react";
import UsersList from "../components/UsersList";
import { sendHttpRequest } from "../hooks/useHttp";
import { getAuthToken } from "../util/auth";
import { Await, Form, Link, redirect, useLoaderData, useNavigation, useSubmit } from "react-router-dom";
import BetList from "../components/BetList";
import UserPermissions from "../components/UserPermissions";

export default function GroupDetailsPage() {
    const { groupDetails } = useLoaderData();


        const submit = useSubmit();


            function handleAction(actionType, method) {
        submit(
            {  action: actionType },
            {
                method: method,
            }
        );
    }
       const navigation = useNavigation();
    const isSubmitting = navigation.state === "submitting";

    return (
        <>
        <UserPermissions />
        <Suspense fallback={<p>Loading....</p>} >
        <Await resolve={groupDetails}>
            {(loadedGroup) => (
                <>
                <h1 className="text-2xl font-bold mb-4">{loadedGroup.name}</h1>
                <div className="bg-white border border-gray-200 rounded-lg shadow-sm p-4">
                            <h2 className="text-lg font-medium mb-3">Group Members</h2>
                            <div className="border-t border-gray-100 pt-3">
                    <UsersList Users={loadedGroup.users} mode="groupMember" isAdmin={loadedGroup.isRequesterAdmin} /> 
                            </div>
                </div>
                <div className="mt-6 bg-white border border-gray-200 rounded-lg shadow-sm p-4">
                    <h2 className="text-lg font-medium mb-3">Group Bets</h2>
                    <Link to="bet/new" className="mb-4 px-3 py-1 bg-green-500 text-white rounded hover:bg-green-600">Create Bet</Link>
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
                            <Form method="PUT" className="mt-2 flex items-center gap-4">
                                <p className="font-medium">Default Starting Balance:</p>
                                <input name="defaultCointBalance" type="number"
                                    defaultValue={loadedGroup.defaultCoinAmount}
                                    readOnly={loadedGroup.isRequesterAdmin ? false : true}
                                    />
                                    {loadedGroup.isRequesterAdmin && (<button type="submit"  onClick={() => handleAction("SaveGroupSettings", "PUT")} className="px-3 py-1 bg-blue-500 text-white rounded hover:bg-blue-600">Save</button>)}
                                
                            </Form>
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

export async function action({ params, request }) {
    const groupId = params.groupId;
    if (request.action === "removeUserFromGroup") {
        return RemoveUserFromGroup({groupId, username: request.friendUsername});
    }
    if (request.action === "SaveGroupSettings") {  
        return EditGroupSettings({groupId, request});
    }

    
   

}

async function EditGroupSettings({groupId, request}) {

 const formData = await request.formData();
    const updatedDefaultCoinBalance = formData.get("defaultCointBalance");
    const response = await sendHttpRequest(`/group/EditGroup`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + getAuthToken(),
        },
        body: JSON.stringify({
            groupId: groupId,
            defaultCoinBalance  : updatedDefaultCoinBalance,
        }),
    });
    if (!response.ok) {
        throw new Response(
            JSON.stringify({ message: "Could not update group" }),
            {
                status: 422,
            }
        );
    }


    return (redirect(`/groups/${groupId}`));
}

async function RemoveUserFromGroup({groupId, username}) {
    const response = await sendHttpRequestt("/group/RemoveUser", {
        method: "DELETE",
        headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + getAuthToken(),
        },
        body: JSON.stringify({
            groupId: groupId,
            username: username,
        }),
    }); 
    if (!response.ok) {
        throw new Response(
            JSON.stringify({ message: "Could not remove user from group" }),
            {
                status: 422,
            }
        );
            }
    return redirect(`/groups/${groupId}`);
 }
    