import { redirect } from "react-router-dom";
import AuthForm from "../components/AuthForm";
import useHttp, { sendHttpRequest } from "../hooks/useHttp";

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
    console.log(authData);
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
    console.log(response);
    if (!response.ok) {
        throw new Response(JSON.stringify({ message: "Could not auth" }), {
            status: 422,
        });
    }
    const resData = await response.json();
    const token = resData.token;

    localStorage.setItem("token", token);
    const expiration = new Date();
    expiration.setHours(expiration.getHours() + 24);
    localStorage.setItem("expiration", expiration.toISOString());

    console.log("token is " + token);
    return redirect("/");
}
