import Drawer from "@mui/material/Drawer";
import AppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";
import List from "@mui/material/List";
import Typography from "@mui/material/Typography";
import Divider from "@mui/material/Divider";
import ListItem from "@mui/material/ListItem";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemIcon from "@mui/material/ListItemIcon";
import ListItemText from "@mui/material/ListItemText";
import GroupIcon from "@mui/icons-material/Group";
import MoneyIcon from "@mui/icons-material/Money";
import GroupsIcon from "@mui/icons-material/Groups";
import LogoutIcon from "@mui/icons-material/Logout";
import { NavLink, useRouteLoaderData } from "react-router-dom";

const drawerWidth = 240;

export default function SideDraw() {
    const token = useRouteLoaderData("root");
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
                <Toolbar />
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
                            <ListItemButton>
                                <ListItemIcon>
                                    <LogoutIcon></LogoutIcon>
                                </ListItemIcon>
                                <ListItemText primary="Sign Out" />
                            </ListItemButton>
                        </ListItem>
                    )}
                </List>
            </Drawer>
            <Toolbar />
        </>
    );
}
