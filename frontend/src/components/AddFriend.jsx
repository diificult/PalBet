import Button from "@mui/material/Button";
import TextField from "@mui/material/TextField";
import {
    Form,
    redirect,
    useFetcher,
    useFormAction,
    useNavigation,
} from "react-router-dom";
import { sendHttpRequest } from "../hooks/useHttp";
import { getAuthToken } from "../util/auth";
import { useEffect } from "react";

export default function AddFriend() {
    const fetcher = useFetcher();
    const { data, state } = fetcher;

    useEffect(() => {
        if (state === "idle" && data && data.message) {
            window.alert(data.message);
        }
    }, [data, state]);

    const navigation = useNavigation();
    const isSubmitting = navigation.state === "submitting";
    return (
        <div className=" items-center p-24">
            <h1>Add Friend</h1>
            <fetcher.Form method="post" className="flex justify-center p-6">
                <TextField
                    id="addFriend"
                    name="addFriend"
                    label="Add Friend"
                    variant="outlined"
                    type="text"
                    required
                />
                <Button
                    variant="contained"
                    type="submit"
                    disabled={isSubmitting}
                >
                    {isSubmitting ? "Submitting..." : "Send"}
                </Button>
            </fetcher.Form>
        </div>
    );
}

export async function action({ request, params }) {
    const data = await request.formData();
    const response = await sendHttpRequest("/SendRequest", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + getAuthToken(),
        },
        body: JSON.stringify(data.get("addFriend")),
    });
    if (!response.ok) {
        return { message: "Could not add friend." };
    }
    return { message: "Request sent!" };
}
