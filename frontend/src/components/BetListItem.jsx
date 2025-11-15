import { useContext } from "react";
import { Link, useSubmit } from "react-router-dom";
import ChooseWinnerContext from "../store/ChooseWinnerContext";
import TollIcon from '@mui/icons-material/Toll';
import { Bolt, CalendarToday, Group, People, Toll } from "@mui/icons-material";

import { grey } from '@mui/material/colors';

// Helper function to check if deadline has passed
function isDeadlinePassed(deadlineString) {
    // Parse "dd/MM/yy hh:mm" format
    const [datePart, timePart] = deadlineString.split(" ");
    const [day, month, year] = datePart.split("/");
    const [hours, minutes] = timePart.split(":");
    
    const deadline = new Date(
        parseInt("20" + year), 
        parseInt(month) - 1, 
        parseInt(day), 
        parseInt(hours), 
        parseInt(minutes)
    );
    
    return new Date() > deadline;
}
export default function BetListItem({
    id,
    title,
    value,
    participants,
    deadline,
    mode,
    isHost,
    state,
    groupName
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
        <div className=" p-12px shadow-sm border-[2px] border-gray-300 bg-gray-50 rounded-md">
            <div className="px-4" ><Link to={`${id}`}><h2 className="text-base/7 font-semibold text-gray-900">{title}</h2></Link></div>
            <div className="px-4 py-4 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-0 border-t border-gray-300">
                <div className="flex px-6">
                    <Toll sx={{color: grey[500]}} /> 
                    <p className="text-sm/6 font-medium text-gray-900 px-2">Bet:</p>
                </div> 
                
                <p className="mt-1 text-sm/6 text-gray-700 sm:col-span-2 sm:mt-0">{value}</p> 
            </div>
            <div className="px-4 pt-0 pb-4 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-0 ">
                <div className="flex px-6">
                    <Bolt sx={{color: grey[500]}} />
                     <p className="text-sm/6 font-medium text-gray-900 px-2">State:</p>
                    </div>
                <p className="mt-1 text-sm/6 text-gray-700 sm:col-span-2 sm:mt-0">{state}</p>
            </div>
            {groupName && (<div className="px-4 pt-0 pb-4 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-0 ">
                <div className="flex px-6">
                    <Group sx={{color: grey[500]}} />
                        <p className="text-sm/6 font-medium text-gray-900 px-2">Group:</p>
                    </div>
                <p className="mt-1 text-sm/6 text-gray-700 sm:col-span-2 sm:mt-0">{groupName}</p>
            </div>)}

                
                {deadline &&( <div className="px-4 pt-0 pb-4 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-0 ">
                <div className="flex px-6">
                    <CalendarToday sx={{color: grey[500]}} /> 
                    <p className="text-sm/6 font-medium text-gray-900 px-2">Deadline:</p>
                </div> 
                <p className={`mt-1 text-sm/6 sm:col-span-2 sm:mt-0 ${isDeadlinePassed(deadline) ? "text-red-600 font-semibold" : "text-gray-700"}`}>
                    {deadline}
                </p> 
            </div>)}
            <div className="px-4 py-4 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-0 border-t border-gray-300">
                <div className="flex px-6">
                    <People sx={{color: grey[500]}} />
                    <p className="text-sm/6 font-medium text-gray-900 px-2">Participants:</p>
                    </div>

                <ul className="mt-1 text-sm/6 text-gray-700 sm:col-span-2 sm:mt-0">
                    {participants && participants.length > 0 ? (
                        participants.map((p, idx) => <li key={idx}>{p}</li>)
                    ) : (
                        <li>No participants</li>
                    )}
                </ul>
            </div>
            {mode === "betRequest" && (
            <div className="px-4 py-3 flex justify-end border-t border-gray-200">
                   <button
                        className="inline-flex items-center px-3 py-1.5 bg-green-600 hover:bg-green-700 text-white text-sm font-medium rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-green-500"
                        onClick={() => handleAction("accept", "PUT")}
                    >
                        Accept
                    </button>
                </div>
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
