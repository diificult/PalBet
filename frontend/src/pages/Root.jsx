import { Outlet, useLoaderData, useSubmit } from "react-router-dom";
import SideDraw from "../components/SideDraw";
import { useEffect } from "react";
import { getTokenDuration } from "../util/auth";

function RootLayout() {
    const token = useLoaderData();
    const submit = useSubmit();
    useEffect(() => {
        if (!token) {
            return;
        }
        if (token === "EXPIRED") {
            submit(null, { action: "/logout", method: "post" });
        }

        const tokenDuration = getTokenDuration();
        console.log(tokenDuration);

        setTimeout(() => {
            submit(null, { action: "/logout", method: "post" });
        }, tokenDuration);
    }, [token, submit]);

    return (
        <>
            <SideDraw />
            <main>
                <Outlet />
            </main>
        </>
    );
}

export default RootLayout;
