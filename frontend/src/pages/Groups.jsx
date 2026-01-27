import { Suspense } from "react";
import { useLoaderData, NavLink, Await } from "react-router-dom";
import GroupList from "../components/GroupList";
import { sendHttpRequest } from "../hooks/useHttp";
import { getAuthToken } from "../util/auth";

export default function GroupsPage() {

    const { groups } = useLoaderData();

    return (<>
                <div className="flex items-center justify-between mb-6">
                <h1 className="text-2xl font-semibold">Groups</h1>
                <NavLink
                    to="new"
                    className="inline-flex items-center gap-2 bg-green-600 rounded-sm px-4 py-1 text-white shadow-sm hover:bg-green-700 transition"
                >
                    Create new group
                </NavLink>
            </div>


                                <div className="bg-white border border-gray-200 rounded-lg p-4 shadow-sm flex flex-col">
                                    <div className="flex items-center justify-between mb-3">
                                        <p className="text-lg font-medium">Your Groups</p>
                                    </div>
                                    <div className="border-t border-gray-100 pt-3 overflow-auto">
                                        <Suspense fallback={<p className="text-sm text-gray-500">Loading...</p>}>
                                            <Await resolve={groups}>
                                                {(loadedGroups) => (
                                                    <GroupList
                                                        groups={loadedGroups}
                                                    />
                                                )}
                                            </Await>
                                        </Suspense>
                                    </div>
                                </div>
            


    </>);
}

export async function loader() {

    const response = await sendHttpRequest(`/group/me`, {
        method: "GET",
        headers: {
                    "Content-Type": "application/json",
                    Authorization: "Bearer " + getAuthToken(),
                },
    }
    );
    const groups = await response.json();
    console.log("Loaded groups:", groups);
    return {
        groups: groups, 
    }

}