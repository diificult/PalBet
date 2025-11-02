import GroupItem from "./GroupItem";

export default function GroupList({ groups }) {
    return <>
            <div>
                <ul className="">
                    {groups.map((group) => (
                        <li className="py-2">
                            <GroupItem
                                id={group.id}
                                name={group.name} 
                                participants = {group.users}
                            />
                            
                        </li>
                    ))}
                </ul>
            </div>
    </>;
}