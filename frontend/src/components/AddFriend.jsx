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
                <input type="hidden" name="action" value="add" />
                <TextField
                    id="friendUsername"
                    name="friendUsername"
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
            {data && (data.error?.title || data.title) && (
                <p className="text-red-600 font-semibold">
                    Error: {data.error?.title || data.title}
                </p>
            )}
        </div>
    );
}
