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
import { tokenLoader } from "./util/auth";

import { action as logoutAction } from "./pages/Logout";
import BetsPage, { loader as betsLoader } from "./pages/Bets";
import NewBetPage from "./pages/NewBet";
import {
    loader as newBetLoader,
    action as newBetAction,
} from "./components/CreateBetRequestForm";

const router = createBrowserRouter([
    {
        path: "/",
        element: <RootLayout />,
        loader: tokenLoader,
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
                children: [
                    { index: true, element: <BetsPage />, loader: betsLoader },
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
