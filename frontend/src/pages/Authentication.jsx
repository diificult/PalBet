import { redirect } from "react-router-dom";
import AuthForm from "../components/AuthForm";
import useHttp, { sendHttpRequest } from "../hooks/useHttp";
import { getAuthToken } from "../util/auth";

export default function AuthenticationPage() {
    return <AuthForm />;
}

export async function action({ request }) {
    const searchParams = new URL(request.url).searchParams;
    const mode = searchParams.get("mode") || "login";

    if (mode !== "login" && mode !== "register") {
        throw new Response(JSON.stringify({ message: "Mode Unsupported." }), {
            status: 422,
        });
    }
    const data = await request.formData();
    const authData = {
        Username: data.get("username"),
        Password: data.get("password"),
    };
    if (mode === "register") {
        authData.email = data.get("email");
    }

    const response = await sendHttpRequest(`/${mode}`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(authData),
    });
    if (!response.ok) {
        return { error: response };
    }
    const resData = await response.json();
    const token = resData.token;

    localStorage.setItem("token", token);
    const expiration = new Date();
    expiration.setHours(expiration.getHours() + 24);
    localStorage.setItem("expiration", expiration.toISOString());

    const usernameResponse = await sendHttpRequest("/GetUsername", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + getAuthToken(),
        },
    });

    const username = await usernameResponse.text();
    console.log(username);

    localStorage.setItem("username", username);
    console.log("token is " + token + " username is " + username);
    return redirect("/");
}
