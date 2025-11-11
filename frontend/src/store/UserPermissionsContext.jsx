import { createContext, useState } from "react";

const UserPermissionsContext = createContext({
        progress: "",
showModel: (data) => {},
hideModel: () => {},

});

export function UserPermissionsContextProvider({ children, data }) {
    const [userPermissionsModel, setUserPermissionsModel] = useState("");
    const [modelData, setModelData] = useState(null);

    function showModel(data = null) {
        console.log("Showing User Permissions Model with data:", data);
        setModelData(data);
        setUserPermissionsModel("show");
    }
    function hideModel() {
        console.log("Hiding User Permissions Model");
        setUserPermissionsModel("");
        setModelData(null);
    }
    const userPermissionsCtx = {
         progress: userPermissionsModel,
        modelData,
        showModel,
        hideModel,
    };
    return (
        <UserPermissionsContext.Provider value={userPermissionsCtx}>
            {children}
        </UserPermissionsContext.Provider>
    );
}
export default UserPermissionsContext;