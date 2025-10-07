import typography from "@tailwindcss/typography";

/** @type {import('tailwindcss').Config} */
export default {
    content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx}"],
    theme: {
        extend: {
            typography: (theme) => ({
                DEFAULT: {
                    css: {
                        code: {
                            color: theme("colors.pink.600"),
                            backgroundColor: theme("colors.gray.100"),
                            padding: "0.2em 0.4em",
                            borderRadius: "0.25rem",
                            fontWeight: "400",
                            fontFamily:
                                "ui-monospace, SFMono-Regular, Menlo, Monaco, Consolas, Liberation Mono, Courier New, monospace",
                        },
                        pre: {
                            backgroundColor: theme("colors.gray.900"),
                            color: theme("colors.gray.100"),
                            borderRadius: "0.5rem",
                            padding: "1rem",
                            overflowX: "auto",
                            fontSize: "0.9rem",
                            lineHeight: "1.5",
                        },
                        "pre code": {
                            backgroundColor: "transparent", // remove inline background
                            padding: "0",
                            color: "inherit",
                            fontFamily: "inherit",
                        },
                    },
                },
                invert: {
                    css: {
                        code: {
                            color: theme("colors.pink.400"),
                            backgroundColor: theme("colors.gray.800"),
                        },
                        pre: {
                            backgroundColor: theme("colors.gray.800"),
                            color: theme("colors.gray.100"),
                        },
                    },
                },
            }),
        },
    },
    plugins: [typography],
};
