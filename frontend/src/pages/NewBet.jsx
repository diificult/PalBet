import CreateBetRequestForm from "../components/CreateBetRequestForm";
import { useParams } from "react-router-dom";

export default function NewBetPage() {
    const { groupId } = useParams();
    const mode = groupId ? 'group' : 'friends';
    
    return (
        <>
            <CreateBetRequestForm method="post" mode={mode} />
        </>
    );
}
