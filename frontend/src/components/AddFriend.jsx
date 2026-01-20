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
        <div className=" items-center p-8">
            <fetcher.Form method="post" className="space-y-3">
                <input type="hidden" name="action" value="add" />

                <label htmlFor="friendUsername" className="block text-sm font-medium text-gray-700">
                    Username
                </label>
                <input
                    id="friendUsername"
                    name="friendUsername"
                    type="text"
                    required
                    className="block w-full rounded-md border border-gray-300 px-3 py-2 shadow-sm placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500"
                    placeholder="username"
                />

                <div className="flex justify-end">
                    <button
                        type="submit"
                        disabled={isSubmitting}
                        className="inline-flex items-center px-4 py-2 bg-indigo-600 hover:bg-indigo-700 text-white text-sm font-medium rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 disabled:opacity-60"
                    >
                        {isSubmitting ? "Sending…" : "Send Request"}
                    </button>
                </div>
            </fetcher.Form>
            {data && (data.error?.title || data.title) && (
                <p className="text-red-600 font-semibold">
                    Error: {data.error?.title || data.title}
                </p>
            )}
        </div>
    );
}
