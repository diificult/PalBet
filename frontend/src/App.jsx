import "./App.css";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import RootLayout from "./pages/Root";
import FriendsPage, { loader as friendsLoader } from "./pages/Friends";
import AuthenticationPage, {
    action as authAction,
} from "./pages/Authentication";
import { tokenLoader } from "./util/auth";

const router = createBrowserRouter([
    {
        path: "/",
        element: <RootLayout />,
        loader: tokenLoader,
        id: "root",
        children: [
            {
                path: "friends",
                element: <FriendsPage />,
                loader: friendsLoader,
            },
            {
                path: "auth",
                element: <AuthenticationPage />,
                action: authAction,
            },
        ],
    },
]);

function App() {
    return <RouterProvider router={router} />;
}

export default App;
