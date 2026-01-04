import { useState } from "react";

function TaskItem({task, onDelete, onChange, onEdit}) {
    const [isEditing, setIsEditing] = useState(false);
    const [editTitle, setEditTitle] = useState(task.title); 
    const [editDescription, setEditDescription] = useState(task.description);

    function Save() {
        const edited = {}
        if (editTitle !== task.title) edited.title = editTitle;
        if (editDescription !== task.description) edited.description = editDescription;
        if (Object.keys(edited).length === 0) {
            setIsEditing(false);
            return;
        }
        onEdit(task.id, edited);
        setIsEditing(false);
    }

    return isEditing ?
    (
        <li>
            <input value={editTitle} onChange={e => setEditTitle(e.target.value)} />
            <textarea value={editDescription} onChange={e => setEditDescription(e.target.value)} />
            <button onClick={Save}>Done</button>
            <button onClick={() => {
                setIsEditing(false);
                setEditTitle(task.title);
                setEditDescription(task.description);
                }}>Cancel</button>
        </li>
    ) : (
        <li>
            <span>{task.title}</span>
            <span>{task.description}</span>
            Completed:
            <input
                type="checkbox"
                checked={task.completed ?? false}
                onChange={e => onChange(task.id, e.target.checked)}
            />
            <button onClick={() => setIsEditing(true)}>Edit</button>
            <button onClick={() => onDelete(task.id)}>Delete</button>
        </li>
    );
} export default TaskItem;