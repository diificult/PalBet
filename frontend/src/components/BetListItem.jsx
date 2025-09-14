export default function BetListItem({ title, value, participants, mode }) {
    return (
        <div className=" p-12px shadow-2xl border-l-2 border-r-2 bg-gray-50 rounded-md">
            <h2 className="p-3 text-xl">{title}</h2>
            <div className="font-semibold text-md">Bet Value: {value}</div>
            <div>
                <strong>Participants:</strong>
                <ul style={{ margin: "8px 0 0 16px" }}>
                    {participants && participants.length > 0 ? (
                        participants.map((p, idx) => <li key={idx}>{p}</li>)
                    ) : (
                        <li>No participants</li>
                    )}
                </ul>
            </div>
            {mode === "betRequest" && (
                <button className="p-2 m-3 bg-green-600  rounded-md text-gray-50">
                    Accept
                </button>
            )}
            {mode === "betRequested" && (
                <button className="p-2 m-3 bg-gray-600  rounded-md text-white">
                    Cancel
                </button>
            )}
            {mode === "openBet" && (
                <>
                    <button className="p-2 m-3 bg-amber-600  rounded-md text-gray-50">
                        Edit
                    </button>
                    <button className="p-2 m-3 bg-gray-600  rounded-md text-white">
                        Cancel Bet
                    </button>
                </>
            )}
        </div>
    );
}
