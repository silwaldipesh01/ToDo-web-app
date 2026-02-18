const API_BASE = "https://localhost:7167/api/ToDo"; // change to http if not using HTTPS

const form = document.getElementById("task-form");
const refreshBtn = document.getElementById("refresh-btn");
const clearBtn = document.getElementById("clear-btn");
const tasksDiv = document.getElementById("tasks");
const tpl = document.getElementById("task-row");

function toPayload() {
    return {
        taskTitle: form.title.value.trim(),
        taskDescription: form.description.value.trim(),
        taskDueDate: form.dueDate.value || null,
        dueTime: form.dueTime.value || "",
        taskIsCompleted: form.isCompleted.checked,
        taskPriority: form.priority.value || "Normal",
    };
}

async function fetchJson(url, options = {}) {
    const res = await fetch(url, {
        headers: { "Content-Type": "application/json" },
        ...options,
    });
    if (!res.ok && res.status !== 204) {
        const msg = await res.text();
        throw new Error(msg || `HTTP ${res.status}`);
    }
    if (res.status === 204) return null;
    return res.json();
}

async function loadTasks() {
    tasksDiv.textContent = "Loading...";
    try {
        const data = await fetchJson(`${API_BASE}/GetTasks`);
        renderTasks(data || []);
    } catch (err) {
        tasksDiv.textContent = `Error: ${err.message}`;
    }
}

function renderTasks(tasks) {
    tasksDiv.innerHTML = "";
    if (!tasks.length) {
        tasksDiv.textContent = "No tasks yet.";
        return;
    }
    tasks.forEach((t) => {
        const node = tpl.content.firstElementChild.cloneNode(true);
        node.querySelector(".task-title").textContent = t.taskTitle;
        node.querySelector(".task-desc").textContent = t.taskDescription || "";
        node.querySelector(".task-meta").textContent =
            `Due: ${t.taskDueDate || "n/a"} ${t.dueTime || ""} • ` +
            `Priority: ${t.taskPriority || "Normal"} • ` +
            `Completed: ${t.taskIsCompleted ? "Yes" : "No"}`;
        node.querySelector(".edit-btn").onclick = () => fillForm(t);
        node.querySelector(".delete-btn").onclick = () => deleteTask(t.taskTitle);
        tasksDiv.appendChild(node);
    });
}

function fillForm(t) {
    form.title.value = t.taskTitle || "";
    form.description.value = t.taskDescription || "";
    form.dueDate.value = t.taskDueDate || "";
    form.dueTime.value = (t.dueTime || "").substring(0, 8);
    form.priority.value = t.taskPriority || "Normal";
    form.isCompleted.checked = !!t.taskIsCompleted;
    form.title.focus();
}

async function deleteTask(title) {
    if (!confirm(`Delete task "${title}"?`)) return;
    try {
        await fetchJson(`${API_BASE}/DeleteByTitle/${encodeURIComponent(title)}`, { method: "DELETE" });
        await loadTasks();
    } catch (err) {
        alert(err.message);
    }
}

form.addEventListener("submit", async (e) => {
    e.preventDefault();
    const payload = toPayload();
    if (!payload.taskTitle) {
        alert("Title is required");
        return;
    }
    if (!payload.dueTime) {
        alert("Due Time is required (HH:mm or HH:mm:ss)");
        return;
    }

    try {
        // Check existence by title
        const existing = await fetch(`${API_BASE}/GetTaskByTitle/${encodeURIComponent(payload.taskTitle)}`);
        const exists = existing.ok;

        if (!exists) {
            // POST create
            await fetchJson(`${API_BASE}/AddTasks/${encodeURIComponent(payload.taskTitle)}`, {
                method: "POST",
                body: JSON.stringify(payload),
            });
        } else {
            // PUT update
            await fetchJson(`${API_BASE}/UpdateTasks/${encodeURIComponent(payload.taskTitle)}`, {
                method: "PUT",
                body: JSON.stringify(payload),
            });
        }

        clearForm();
        await loadTasks();
    } catch (err) {
        alert(err.message);
    }
});

function clearForm() {
    form.reset();
}

clearBtn.onclick = clearForm;
refreshBtn.onclick = loadTasks;

loadTasks();