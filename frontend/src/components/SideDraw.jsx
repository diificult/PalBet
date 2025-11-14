import Drawer from "@mui/material/Drawer";
import AppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";
import List from "@mui/material/List";
import Divider from "@mui/material/Divider";
import ListItem from "@mui/material/ListItem";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemIcon from "@mui/material/ListItemIcon";
import ListItemText from "@mui/material/ListItemText";
import HomeIcon from "@mui/icons-material/Home";
import GroupIcon from "@mui/icons-material/Group";
import MoneyIcon from "@mui/icons-material/Money";
import GroupsIcon from "@mui/icons-material/Groups";
import LogoutIcon from "@mui/icons-material/Logout";
import {
    Await,
    Form,
    NavLink,
    useRouteLoaderData,
    redirect,
    useSubmit,
} from "react-router-dom";

import defaultimg from "../assets/default.jpg";
import { sendHttpRequest } from "../hooks/useHttp";
import { getAuthToken } from "../util/auth";
import { Group } from "@mui/icons-material";
import { red } from "@mui/material/colors";
import { Suspense } from "react";
const drawerWidth = 256;

export default function SideDraw() {
    const { token, sideData, username } = useRouteLoaderData("root");
    // const { coins } = useLoaderData();
    console.log(sideData);

    const submit = useSubmit();

    return (
        <aside className="w-64 flex-shrink-0 bg-white border-r border-gray-200 min-h-screen">
            <div className="p-4">
                {token ? (
                    <>
                        <div className="flex items-center gap-3">
                            <img
                                src={defaultimg}
                                className="rounded-md max-w-12"
                            />
                            <div>
                                <div className="font-semibold">{username}</div>
                                <Suspense fallback={<div className="text-sm text-gray-500">...</div>}>
                                    <Await resolve={sideData}>
                                        {(loaded) => (
                                            <Suspense fallback={<div className="text-sm text-gray-500">Loading...</div>}>
                                                <Await resolve={Promise.resolve(loaded?.timeRemaining)}>
                                                    {(timeRemaining) => (
                                                        <div className="text-sm text-gray-500">
                                                            {loaded?.coins ?? 0} coins
                                                            <div>
                                                                {timeRemaining === "00:00:00" ? (
                                                                    <Form method="post" action="/">
                                                                        <button type="submit" className="bg-blue-500 text-white px-2 py-1 rounded-md mt-1">Claim Daily Reward</button>
                                                                    </Form>
                                                                ) : (
                                                                    <div>Daily Reward in: {timeRemaining}</div>
                                                                )}
                                                            </div>
                                                        </div>
                                                    )}
                                                </Await>
                                            </Suspense>
                                        )}
                                    </Await>
                                </Suspense>
                            </div>
                        </div>
                    </>
                ) : (
                    <div className="text-sm text-gray-600">Not signed in</div>
                )}
            </div>

                {token && sideData &&(
                    <>
                    <div className="m-4 border-t border-gray-100" />
                    <Suspense fallback={<div className="text-sm text-gray-500">...</div>}>
                        <Await resolve={sideData}>
                            {(loaded) => (
                                <NavLink to="/notifications" className="flex items-center gap-3 px-3 py-2 rounded-md hover:bg-gray-100">
                                    <p
                                        className={
                                            loaded?.notificationCount > 0
                                                ? "bg-red-500 rounded-full px-2 text-white"
                                                : "bg-gray-500 rounded-full px-2 text-white"
                                        }
                                    >
                                        {loaded?.notificationCount ?? 0}
                                    </p>
                                    <span>Notifications</span>
                                </NavLink>
                            )}
                        </Await>
                    </Suspense>
                    </>
                ) 
                }



                <div className="mt-4 border-t border-gray-100" />

            <nav className="px-2 py-4">
                <NavLink to="/" className="flex items-center gap-3 px-3 py-2 rounded-md hover:bg-gray-100">
                    <HomeIcon fontSize="small" /> <span>Home</span>
                </NavLink>
                <NavLink to="/friends" className="flex items-center gap-3 px-3 py-2 rounded-md hover:bg-gray-100">
                    <GroupIcon fontSize="small" /> <span>Friends</span>
                </NavLink>
                <NavLink to="/bets" className="flex items-center gap-3 px-3 py-2 rounded-md hover:bg-gray-100">
                    <MoneyIcon fontSize="small" /> <span>Bets</span>
                </NavLink>
                
                <NavLink to="/groups" className="flex items-center gap-3 px-3 py-2 rounded-md hover:bg-gray-100">
                    <GroupIcon fontSize="small" /> <span>Groups</span>
                </NavLink>
                                <div className="mt-4 border-t border-gray-100" />
                {!token && (
                    <NavLink to="/auth?mode=login" className="flex items-center gap-3 px-3 py-2 rounded-md hover:bg-gray-100 mt-3">
                        <LogoutIcon fontSize="small" /> <span>Sign In</span>
                    </NavLink>
                )}
                                {!token && (
                    <NavLink to="/auth?mode=register" className="flex items-center gap-3 px-3 py-2 rounded-md hover:bg-gray-100 mt-3">
                        <LogoutIcon fontSize="small" /> <span>Sign Up</span>
                    </NavLink>
                    
                )}
                {token && (
                    <Form method="post" action="/logout" className="mt-3">
                        <button type="submit" className="flex items-center gap-3 px-3 py-2 rounded-md hover:bg-gray-100 w-full text-left">
                            <LogoutIcon fontSize="small" /> <span>Sign Out</span>
                        </button>
                    </Form>
                )}
            </nav>
        </aside>
    );
}

export async function loader() {
    return {
        coins: getCoins(),
        notificationCount: getNotifications(),
        timeRemaining: getDailyRewardsTimeRemaining(),
    };
}

async function getCoins() {
    const response = await sendHttpRequest("/GetCoins", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + getAuthToken(),
        },
    });
    const resData = await response.json();
    return resData;
}

async function getDailyRewardsTimeRemaining() {
    const response = await sendHttpRequest("/Rewards/CheckLast", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + getAuthToken(),
        },
    });
    const resData = await response.text();
    return resData;
}
async function getNotifications() {
    const response = await sendHttpRequest("/Notification/GetCount", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + getAuthToken(),
        },
    });
    const resData = await response.json();
    return resData;
}

export async function action() {
    
 const response = await sendHttpRequest("/Rewards/GetRewards", {
    method: "PUT",
    headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + getAuthToken(),
    },
    });

    if (!response.ok) {
        throw new Response(
            JSON.stringify({ message: "Could not claim daily reward" }),
            {
                status: 422,
            }
        );
    }

    return redirect("/");
}

