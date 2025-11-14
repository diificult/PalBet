import "./App.css";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import RootLayout from "./pages/Root";
import FriendsPage, {
    loader as friendsLoader,
    action as friendAction,
} from "./pages/Friends";
import AuthenticationPage, {
    action as authAction,
} from "./pages/Authentication";
import { tokenLoader, usernameLoader, checkAuth } from "./util/auth";

import { action as logoutAction } from "./pages/Logout.jsx";
import BetsPage, {
    loader as betsLoader,
    action as betsAction,
} from "./pages/Bets";
import NewBetPage from "./pages/NewBet";
import {
    loader as newBetLoader,
    action as newBetAction,
} from "./components/CreateBetRequestForm";
import { loader as SideDrawLoader } from "./components/SideDraw";
import ErrorPage from "./pages/Error.jsx";
import NotificationPage, {
    loader as notificationsLoader,
} from "./pages/Notifications.jsx";
import { action as notificationFriendAction } from "./components/NotificationList.jsx";
import IndexPage from "./pages/Index.jsx";
import BetDetailPage, {loader as betDetailLoader} from "./pages/BetDetails.jsx"
import GroupsPage, {loader as groupLoader} from "./pages/Groups.jsx";
import NewGroupPage from "./pages/NewGroup.jsx";
import {loader as newGroupLoader, action as newGroupAction} from "./components/CreateGroupForm.jsx";
import GroupDetailsPage, {loader as groupDetailLoader, action as groupUpdateAction} from "./pages/GroupDetails.jsx";
import {action as claimRewardsAction} from "./components/SideDraw.jsx";

async function rootLoader() {
    const token = await tokenLoader();

    const promises = [];

    let username = null;
    console.log(token);
    if (token && token !== "EXPIRED") {
        username = await usernameLoader();
        promises.push(SideDrawLoader());
    }

    const [sideData] = await Promise.all(promises);

    return { token, sideData: sideData ?? null, username };
}

const router = createBrowserRouter([
    {
        path: "/",
        element: <RootLayout />,
        loader: rootLoader,
        action: claimRewardsAction,
        errorElement: <ErrorPage />,
        id: "root",
        children: [
            {
                index: true,
                element: <IndexPage />,
            },
            {
                path: "friends",
                id: "friends",
                element: <FriendsPage />,
                loader: friendsLoader,
                action: friendAction,
            },
            {
                path: "bets",
                id: "bets",
                loader: checkAuth,
                children: [
                    {
                        index: true,
                        element: <BetsPage />,
                        loader: betsLoader,
                        action: betsAction,
                    },
                    {
                        path: ":betId",
                        id: "bet-detail",
                        loader: betDetailLoader,
                        children: [
                            {
                                index: true,
                                element: <BetDetailPage />
                            }
                        ]
                    },
                    {
                        path: "new",
                        element: <NewBetPage />,
                        loader: newBetLoader,
                        action: newBetAction,
                    },
                ],
            },
            {
                path: "groups",
                id: "groups",
                loader: checkAuth,
                children: [
                    {
                        index: true,
                        element: <GroupsPage />,
                        loader: groupLoader,
                    },
                    {
                        path:":groupId",
                        element: <GroupDetailsPage />,
                        loader: groupDetailLoader,
                        action: groupUpdateAction,
                    },
                    {
                        path: ":groupId/bet/new",
                        element: <NewBetPage />,
                        loader: newBetLoader,
                        action: newBetAction
                    },
                    {
                        path: "new",
                        element: <NewGroupPage />,
                        loader: newGroupLoader,
                        action: newGroupAction,
                    }
                ]
            },
            {
                path: "notifications",
                id: "notifications",
                element: <NotificationPage />,
                loader: notificationsLoader,
                action: notificationFriendAction,
            },
            {
                path: "auth",
                element: <AuthenticationPage />,
                action: authAction,
            },
            { path: "logout", action: logoutAction },
        ],
    },
]);

function App() {
    return <RouterProvider router={router} />;
}

export default App;
