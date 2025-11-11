import { useParams } from "react-router-dom";

export default function NewGroupBet() {
    
    const params  = useParams();
    
    return (
        <>
                <CreateBetRequestForm method="post" mode="groupBet" groupId={params.groupId} />
        </>
    );
}