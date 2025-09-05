import {
    Form,
    useNavigate,
    useNavigation,
    useSearchParams,
} from "react-router-dom";

export default function AuthForm() {
    const navigation = useNavigation();
    const isSubmitting = navigation.state === "submitting";

    const [searchParams, setSearchParams] = useSearchParams();

    const isLogin = searchParams.get("mode") === "login";

    return (
        <>
            <Form method="post">
                <h1>{isLogin ? "Login" : "Create new user"}</h1>
                {!isLogin && (
                    <p>
                        <label htmlFor="email">Email</label>
                        <input id="email" type="email" name="email" required />
                    </p>
                )}
                <p>
                    <label htmlFor="username">Username</label>
                    <input id="username" type="text" name="username" required />
                </p>

                <p>
                    <label htmlFor="image"> Password</label>
                    <input
                        id="password"
                        type="password"
                        name="password"
                        required
                    />
                    <button disabled={isSubmitting}>
                        {isSubmitting ? "Saving..." : "Save"}{" "}
                    </button>
                </p>
            </Form>
        </>
    );
}
