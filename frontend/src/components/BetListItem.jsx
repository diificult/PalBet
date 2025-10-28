import { useContext } from "react";
import { Link, useSubmit } from "react-router-dom";
import ChooseWinnerContext from "../store/ChooseWinnerContext";

export default function BetListItem({
    id,
    title,
    value,
    participants,
    mode,
    isHost,
}) {
    const submit = useSubmit();
    const modelCtx = useContext(ChooseWinnerContext);

    function handleAction(actionType, method) {
        console.log(id);
        submit(
            { betId: id, action: actionType },
            {
                method: method,
            }
        );
    }

    function handleShowModal() {
        console.log("Should be showing");
        modelCtx.showModel({ participants, id });
    }

    function startDeleteHandler(betId) {
        const proceed = window.confirm("Are you sure?");

        if (proceed) {
            handleAction("cancel", "PUT");
        }
    }

    return (
        <div className=" p-12px shadow-2xl border-[1px] border-r-2 bg-gray-50 rounded-md">
            <Link to={`${id}`}><h2 className="p-3 text-xl">{title}</h2></Link>
            <div className="font-semibold text-md">Bet Value: {value}</div>
            <div>
                <strong>Participants:</strong>
                <ul>
                    {participants && participants.length > 0 ? (
                        participants.map((p, idx) => <li key={idx}>{p}</li>)
                    ) : (
                        <li>No participants</li>
                    )}
                </ul>
            </div>
            {mode === "betRequest" && (
                <button
                    className="p-2 m-3 bg-green-600  rounded-md text-gray-50"
                    onClick={() => {
                        handleAction("accept", "PUT");
                    }}
                >
                    Accept
                </button>
            )}
            {mode === "betRequested" && (
                <button
                    className="p-2 m-3 bg-gray-600  rounded-md text-white"
                    onClick={() => {
                        startDeleteHandler();
                    }}
                >
                    Cancel
                </button>
            )}
            {mode === "openBet" && isHost && (
                <>
                    <button
                        className="p-2 m-3 bg-amber-600  rounded-md text-gray-50"
                        onClick={handleShowModal}
                    >
                        Choose Winner
                    </button>
                    <button
                        className="p-2 m-3 border-gray-600 border-2  rounded-md text-black"
                        onClick={() => {
                            startDeleteHandler();
                        }}
                    >
                        Cancel Bet
                    </button>
                </>
            )}
            {mode === "openBet" && !isHost && (
                <p className="font-semibold">Only host can make changes</p>
            )}
        </div>
    );
}
