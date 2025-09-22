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

async function rootLoader() {
    const token = await tokenLoader();

    const promises = [];

    let username = null;
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
        errorElement: <ErrorPage />,
        id: "root",
        children: [
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
                        path: "new",
                        element: <NewBetPage />,
                        loader: newBetLoader,
                        action: newBetAction,
                    },
                ],
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
