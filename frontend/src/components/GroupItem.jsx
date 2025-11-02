import { People } from "@mui/icons-material";
import { Link } from "react-router-dom";
import { grey } from '@mui/material/colors';
export default function GroupItem({name, participants, id}) {
    return (
                <div className=" p-12px shadow-sm border-[2px] border-gray-300 bg-gray-50 rounded-md">
            <div className="px-4" ><Link to={`${id}`}><h2 className="text-base/7 font-semibold text-gray-900">{name}</h2></Link></div>
            <div className="px-4 py-4 flex justify-between border-t border-gray-300">
                <div className="flex px-6">
                    <People sx={{color: grey[500]}} />
                    <p className="text-sm/6 font-medium text-gray-900 px-2">Participants:</p>
                    </div>

                <ul className="text-sm/6 text-gray-700 ">
                    {participants && participants.length > 0 ? (
                        participants.map((p) => (
                            <li key={p.userId} className="flex items-right gap-4">
                                <p>{p.username}</p>
                                <p className="text-gray-500">{p.balance} coins</p>
                            </li>
                        ))
                    ) : (
                        <li>No participants</li>
                    )}
                </ul>
                
            </div><div className="px-4 py-4 flex justify-between border-t border-gray-300">
                    <Link to={`/groups/${id}/bet/new`}><button className="bg-green-500 rounded-md px-4 py-1 text-white">Create new Bet</button> </Link>
                </div>
            </div>
    )
}