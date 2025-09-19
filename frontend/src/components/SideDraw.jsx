import Drawer from "@mui/material/Drawer";
import AppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";
import List from "@mui/material/List";
import Divider from "@mui/material/Divider";
import ListItem from "@mui/material/ListItem";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemIcon from "@mui/material/ListItemIcon";
import ListItemText from "@mui/material/ListItemText";
import GroupIcon from "@mui/icons-material/Group";
import MoneyIcon from "@mui/icons-material/Money";
import GroupsIcon from "@mui/icons-material/Groups";
import LogoutIcon from "@mui/icons-material/Logout";
import {
    Await,
    Form,
    NavLink,
    useLoaderData,
    useRouteLoaderData,
} from "react-router-dom";

import defaultimg from "../assets/default.jpg";
import { sendHttpRequest } from "../hooks/useHttp";
import { Suspense } from "react";
import { getAuthToken } from "../util/auth";
const drawerWidth = 340;

export default function SideDraw() {
    const { token, sideData, username } = useRouteLoaderData("root");
    // const { coins } = useLoaderData();

    return (
        <>
            <Drawer
                sx={{
                    width: drawerWidth,
                    flexShrink: 0,
                    "& .MuiDrawer-paper": {
                        width: drawerWidth,
                        boxSizing: "border-box",
                    },
                }}
                variant="permanent"
                anchor="left"
            >
                {token && (
                    <div className=" p-3 flex">
                        <div className="p-2">
                            <img
                                src={defaultimg}
                                className="rounded-md max-w-16"
                            />
                        </div>
                        <div>
                            <p className="font-bold text-3xl">{username}</p>
                            <Suspense fallback={<p>Loading...</p>}>
                                <Await resolve={sideData}>
                                    {(loadedCoins) => (
                                        <p>{loadedCoins} coins</p>
                                    )}
                                </Await>
                            </Suspense>
                        </div>
                    </div>
                )}
                <Divider />
                <List>
                    <ListItem disablePadding>
                        <ListItemButton>
                            <ListItemIcon>
                                <p className="bg-gray-500 rounded-full px-5 text-white">
                                    0
                                </p>
                            </ListItemIcon>
                            <ListItemText primary="Notification" />
                        </ListItemButton>
                    </ListItem>
                </List>
                <Divider />
                <List>
                    <ListItem disablePadding>
                        <ListItemButton component={NavLink} to="/friends">
                            <ListItemIcon>
                                <GroupIcon></GroupIcon>
                            </ListItemIcon>
                            <ListItemText primary="Friends" />
                        </ListItemButton>
                    </ListItem>
                    <ListItem key="Bets" disablePadding>
                        <ListItemButton component={NavLink} to="/bets">
                            <ListItemIcon>
                                <MoneyIcon></MoneyIcon>
                            </ListItemIcon>
                            <ListItemText primary="Bets" />
                        </ListItemButton>
                    </ListItem>
                    <ListItem key="Groups" disablePadding>
                        <ListItemButton>
                            <ListItemIcon>
                                <GroupsIcon></GroupsIcon>
                            </ListItemIcon>
                            <ListItemText primary="Groups" />
                        </ListItemButton>
                    </ListItem>
                </List>
                <Divider />
                <List>
                    {!token && (
                        <ListItem disablePadding>
                            <ListItemButton
                                component={NavLink}
                                to="/auth?mode=login"
                            >
                                <ListItemIcon>
                                    <LogoutIcon></LogoutIcon>
                                </ListItemIcon>
                                <ListItemText primary="Sign In" />
                            </ListItemButton>
                        </ListItem>
                    )}
                    {!token && (
                        <ListItem disablePadding>
                            <ListItemButton
                                component={NavLink}
                                to="/auth?mode=register"
                            >
                                <ListItemIcon>
                                    <LogoutIcon></LogoutIcon>
                                </ListItemIcon>
                                <ListItemText primary="Sign Up" />
                            </ListItemButton>
                        </ListItem>
                    )}
                    {token && (
                        <ListItem key="Signout" disablePadding>
                            <Form method="post" action="/logout">
                                <ListItemButton type="submit">
                                    <ListItemIcon>
                                        <LogoutIcon></LogoutIcon>
                                    </ListItemIcon>
                                    <ListItemText primary="Sign Out" />
                                </ListItemButton>
                            </Form>
                        </ListItem>
                    )}
                </List>
            </Drawer>
            <Toolbar />
        </>
    );
}

export async function loader() {
    return getCoins();
}

async function getCoins() {
    const response = await sendHttpRequest("/GetCoins", {
        method: "Get",
        headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + getAuthToken(),
        },
    });
    const resData = await response.json();
    return resData;
}
