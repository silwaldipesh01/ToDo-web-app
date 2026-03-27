const API = "/api/ToDo";

const form = document.getElementById("task-form");
const tasksDiv = document.getElementById("tasks");
const tpl = document.getElementById("task-row");
const submitBtn = document.getElementById("submitBtn");

let editMode = false;
let currentTitle = "";

// ================= LOAD =================
async function loadTasks() {
    tasksDiv.innerHTML = "⏳ Loading...";

    try {
        const res = await fetch(`${API}/GetTasks`);
        if (res.status === 204) {
            tasksDiv.innerHTML = "No tasks found.";
            return;
        }

        const data = await res.json();
        renderTasks(data);
    } catch (err) {
        tasksDiv.innerHTML = "Error loading tasks";
    }
}

// ================= RENDER =================
function renderTasks(tasks) {
    tasksDiv.innerHTML = "";

    tasks.forEach(t => {
        const node = tpl.content.cloneNode(true);

        const title = node.querySelector(".task-title");
        title.textContent = t.taskTitle;

        if (t.taskIsCompleted) {
            title.style.textDecoration = "line-through";
            title.style.color = "gray";
        }

        const priorityColor =
            t.taskPriority === "High" ? "red" :
                t.taskPriority === "Medium" ? "orange" : "green";

        node.querySelector(".task-meta").innerHTML =
            `Due: ${t.taskDueDate || "N/A"} ${t.dueTime || ""} 
             • <span style="color:${priorityColor}">${t.taskPriority}</span>`;

        node.querySelector(".task-desc").textContent = t.taskDescription;

        node.querySelector(".edit-btn").onclick = () => fillForm(t);
        node.querySelector(".delete-btn").onclick = () => deleteTask(t.taskTitle);

        tasksDiv.appendChild(node);
    });
}

// ================= FORM =================
function fillForm(t) {
    editMode = true;
    currentTitle = t.taskTitle;

    form.title.value = t.taskTitle;
    form.description.value = t.taskDescription;
    form.dueDate.value = t.taskDueDate;
    form.dueTime.value = t.dueTime;
    form.priority.value = t.taskPriority;
    form.isCompleted.checked = t.taskIsCompleted;
}

// ================= SAVE =================
form.addEventListener("submit", async (e) => {
    e.preventDefault();

    const payload = {
        taskTitle: form.title.value,
        taskDescription: form.description.value,
        taskDueDate: form.dueDate.value,
        dueTime: form.dueTime.value,
        taskIsCompleted: form.isCompleted.checked,
        taskPriority: form.priority.value
    };

    submitBtn.disabled = true;
    submitBtn.textContent = "Saving...";

    try {
        if (!editMode) {
            await fetch(`${API}/AddTasks/${payload.taskTitle}`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payload)
            });
        } else {
            await fetch(`${API}/UpdateTasks/${currentTitle}`, {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payload)
            });
        }

        clearForm();
        loadTasks();
    } catch {
        alert("Error saving task");
    }

    submitBtn.disabled = false;
    submitBtn.textContent = "Save Task";
});

// ================= DELETE =================
async function deleteTask(title) {
    if (!confirm("Delete this task?")) return;

    await fetch(`${API}/DeleteByTitle/${title}`, {
        method: "DELETE"
    });

    loadTasks();
}

// ================= CLEAR =================
function clearForm() {
    form.reset();
    editMode = false;
}

// ================= INIT =================
document.getElementById("refresh-btn").onclick = loadTasks;
document.getElementById("clear-btn").onclick = clearForm;

loadTasks();
