import { useEffect, useState } from "react";
import TaskList from "./TaskList";
import TaskForm from "./TaskForm";
import { PostNewTask, GetTasks, DeleteTask, PatchTaskState, PatchTaskEdit } from "./ManageTask";

const BACKEND_URL = 'http://localhost:5225';

function App() {
  const [tasks, setTasks] = useState([]);
  
  useEffect(() => {
      async function LoadTasks() {
      const res = await GetTasks(BACKEND_URL);
      setTasks(res);
    } LoadTasks();
    }, []);

  async function onSubmit(task) {
    const res = await PostNewTask(BACKEND_URL, task);
    if (res) setTasks(prev => [...prev, res]);
  }

  async function onDelete(id) {
    await DeleteTask(BACKEND_URL, id);
    setTasks(prev => prev.filter(task => task.id !== id));
    console.log(`deleted task: ${id}`);
  }

  async function onChange(id, changed) {
    await PatchTaskState(BACKEND_URL, id, changed);
    console.log(`changed ${id} state to ${changed}`);
    setTasks(prev => prev.map(task => task.id === id ? {...task, completed: changed} : task));
    console.log(`task state changed:${changed}`);
  }

  async function onEdit(id, editedTask) {
    const res = await PatchTaskEdit(BACKEND_URL, id, editedTask);
    setTasks(prev => prev.map(task => task.id === id ? res : task));
  }
    
  return (
    <div>
      <h1>Tasks</h1>
      <TaskList list={tasks} onDelete={onDelete} onChange={onChange} onEdit={onEdit}/>
      <TaskForm onSubmit={onSubmit}/>
    </div>
  );
} export default App;