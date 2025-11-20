import { useContext, useState } from "react";
import Modal from "./UI/Model";
import OutcomeChoiceContext from "../store/OutcomeChoiceContext";
import { useSubmit } from "react-router-dom";

export default function UserDefinedChoices() {
    const submit = useSubmit();
    const outcomeChoiceCtx = useContext(OutcomeChoiceContext);
    const [userChoice, setUserChoice] = useState("");
    console.log("UserDefinedChoices render:", { progress: outcomeChoiceCtx.progress, modalType: outcomeChoiceCtx.modalType });

    function handleCloseModel() {
        outcomeChoiceCtx.hideModal();
    }

    function handleSubmitChoice() {
        if (userChoice.trim()) {
            submit(
                {
                    betId: outcomeChoiceCtx.modelData.id,
                    outcomeChoice: userChoice,
                    action: "accept",
                },
                {
                    method: "PUT",
                }
            );
            handleCloseModel();
            setUserChoice("");
        }
    }

    function handleKeyDown(e) {
        if (e.key === "Enter") {
            handleSubmitChoice();
        }
    }

    return (
        <Modal
            open={outcomeChoiceCtx.progress === "show" && outcomeChoiceCtx.modalType === "userDefinedChoices"}
            onClose={outcomeChoiceCtx.progress === "close" ? handleCloseModel : null}
        >
            <div className="fixed inset-0 bg-gray-900/50 z-10"></div>
            <div className="fixed inset-0 flex items-center justify-center z-20">
                <div className="relative transform overflow-hidden rounded-lg bg-gray-800 text-left shadow-xl outline -outline-offset-1 outline-white/10 transition-all data-closed:translate-y-4 data-closed:opacity-0 data-enter:duration-300 data-enter:ease-out data-leave:duration-200 data-leave:ease-in sm:my-8 sm:w-full sm:max-w-lg data-closed:sm:translate-y-0 data-closed:sm:scale-95">
                    <div className="bg-gray-800 px-4 pt-5 pb-4 sm:p-6 sm:pb-4">
                        <div className="sm:flex sm:items-start">
                            <div className="mt-3 text-center sm:mt-0 sm:ml-4 sm:text-left w-full">
                                <h3
                                    id="dialog-title"
                                    className="text-base font-semibold text-white"
                                >
                                    Enter Your Choice
                                </h3>
                                <div className="mt-4">
                                    <input
                                        type="text"
                                        value={userChoice}
                                        onChange={(e) => setUserChoice(e.target.value)}
                                        onKeyDown={handleKeyDown}
                                        placeholder="Type your outcome choice"
                                        className="w-full px-4 py-2 bg-gray-700 text-white rounded border border-gray-600 focus:outline-none focus:ring-2 focus:ring-blue-500"
                                        autoFocus
                                    />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="bg-gray-700/25 px-4 py-3 sm:flex sm:flex-row-reverse sm:px-6 gap-2">
                        <button
                            className="mt-6 rounded bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 transition disabled:opacity-50 disabled:cursor-not-allowed"
                            onClick={handleSubmitChoice}
                            disabled={!userChoice.trim()}
                        >
                            Submit
                        </button>
                        <button
                            className="mt-6 rounded bg-gray-600 hover:bg-gray-700 text-white px-4 py-2 transition"
                            onClick={handleCloseModel}
                        >
                            Cancel
                        </button>
                    </div>
                </div>
            </div>
        </Modal>
    );
}
