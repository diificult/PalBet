import { Outlet, useLoaderData, useSubmit } from "react-router-dom";
import SideDraw from "../components/SideDraw";
import { useEffect } from "react";
import { getTokenDuration } from "../util/auth";
import { ChooseWinnerContextProvider } from "../store/ChooseWinnerContext";
import { OutcomeChoiceContextProvider } from "../store/OutcomeChoiceContext";
import { UserPermissionsContextProvider } from "../store/UserPermissionsContext";

function RootLayout() {
    const { token } = useLoaderData();
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
            <ChooseWinnerContextProvider>
                <OutcomeChoiceContextProvider>
                    <UserPermissionsContextProvider>
                        <div className="min-h-screen flex w-full items-start">
                            <SideDraw />
                            <main className="flex-1 min-h-screen p-6 max-w-full">
                                <Outlet />
                            </main>
                        </div>
                    </UserPermissionsContextProvider>
                </OutcomeChoiceContextProvider>
            </ChooseWinnerContextProvider>
        </>
    );
}

export default RootLayout;
