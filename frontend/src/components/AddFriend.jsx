import Button from "@mui/material/Button";
import TextField from "@mui/material/TextField";

export default function AddFriend() {
    return (
        <div>
            <h1>Add Friend</h1>
            <TextField
                id="outlined-basic"
                label="Add Friend"
                variant="outlined"
            />
            <Button variant="contained">Contained</Button>
        </div>
    );
}
