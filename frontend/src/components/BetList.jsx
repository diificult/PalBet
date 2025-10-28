import BetListItem from "./BetListItem";

export default function BetList({ bets, mode }) {
    return (
        <div>
            <ul className="">
                {bets.map((bet) => (
                    <li className="py-2">
                        <BetListItem
                            id={bet.betId}
                            title={bet.betDescription}
                            value={bet.betStake}
                            participants={bet.participantNames}
                            deadline={bet.deadline}
                            mode={mode}
                            isHost={bet.isHost}
                        />
                    </li>
                ))}
            </ul>
        </div>
    );
}
