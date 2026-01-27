import { useEffect, useState, useCallback } from "react";

const BASEURL = "https://localhost:7130/api";

export async function sendHttpRequest(path, config) {
    const response = await fetch(BASEURL + path, config);
    // const resData = await response.json();

    // if (!response.ok) {
    //     throw new Error(resData.message || "Something went wrong.");
    // }

    return response;
}

export default function useHttp(url, config, initData) {
    const [data, setData] = useState(initData);
    const [error, setError] = useState();
    const [isLoading, setIsLoading] = useState(false);

    const sendRequest = useCallback(
        async function sendRequest(data) {
            setIsLoading(true);
            try {
                const resData = await sendHttpRequest(url, {
                    ...config,
                    body: data,
                });
                setData(resData);
            } catch (error) {
                setError(error.message || "Something went wrong.");
            }
            setIsLoading(false);
        },
        [url, config]
    );

    useEffect(() => {
        if (
            (config && (config.method === "GET" || !config.method)) ||
            !config
        ) {
            sendRequest();
        }
    }, [sendRequest, config]);

    return {
        data,
        isLoading,
        error,
        sendRequest,
    };
}
