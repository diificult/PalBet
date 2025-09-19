import { useState, createContext } from "react";

const ChooseWinnerContext = createContext({
    progress: "",
    showModel: (data) => {},
    hideModel: () => {},
});

export function ChooseWinnerContextProvider({ children, data }) {
    const [chooseWinnerModel, setChooseWinnerModel] = useState("");
    const [modelData, setModelData] = useState(null);

    function showModel(data = null) {
        setModelData(data);
        setChooseWinnerModel("show");
    }
    function hideModel() {
        setChooseWinnerModel("");
        setModelData(null);
    }
    const chooseWinnerCtx = {
        progress: chooseWinnerModel,
        modelData,
        showModel,
        hideModel,
    };
    return (
        <ChooseWinnerContext.Provider value={chooseWinnerCtx}>
            {children}
        </ChooseWinnerContext.Provider>
    );
}

export default ChooseWinnerContext;
