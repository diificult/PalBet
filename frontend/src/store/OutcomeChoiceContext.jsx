import { useState, createContext } from "react";

const OutcomeChoiceContext = createContext({
    progress: "",
    modalType: "",
    modelData: null,
    showModal: (data, type) => {},
    hideModal: () => {},
});

export function OutcomeChoiceContextProvider({ children }) {
    const [outcomeChoiceModal, setOutcomeChoiceModal] = useState("");
    const [modalType, setModalType] = useState("");
    const [modelData, setModelData] = useState(null);

    function showModal(data = null, type = "hostDefinedChoices") {
        setModelData(data);
        setModalType(type);
        setOutcomeChoiceModal("show");
        console.log("OutcomeChoiceContext.showModal:", { data, type });
    }

    function hideModal() {
        setOutcomeChoiceModal("");
        setModalType("");
        setModelData(null);
        console.log("OutcomeChoiceContext.hideModal");
    }

    const outcomeChoiceCtx = {
        progress: outcomeChoiceModal,
        modalType,
        modelData,
        showModal,
        hideModal,
    };

    return (
        <OutcomeChoiceContext.Provider value={outcomeChoiceCtx}>
            {children}
        </OutcomeChoiceContext.Provider>
    );
}

export default OutcomeChoiceContext;
