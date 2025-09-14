import BetListItem from "./BetListItem";

export default function BetList({ bets, mode }) {
    console.log(bets);
    return (
        <div>
            <ul>
                {bets.map((bet) => (
                    <BetListItem
                        title={bet.betDescription}
                        value={bet.betStake}
                        participants={bet.participants}
                        mode={mode}
                    />
                ))}
            </ul>
        </div>
    );
}
