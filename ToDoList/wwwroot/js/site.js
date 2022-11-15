function InitDeleteListener() {
    const deleteBtn = document.querySelectorAll(".delete-btn");
    deleteBtn.forEach(btn => {
        btn.addEventListener("click", (e) => {
            let taskId = e.target.dataset.taskId;
            DeleteTask(taskId);
        })
    })
}

function InitUpdateListener(){
    const updateBtn = document.querySelectorAll(".update-btn");
    updateBtn.forEach(btn => {
        btn.addEventListener("click", (e) => {
            let taskId = e.target.dataset.taskId;
            let taskName = e.target.parentNode.dataset.value;

            console.log(taskName);
            UpdateTask(taskId, taskName);
        })
    })
}

async function DeleteTask(taskId) {
    let Id = parseInt(taskId);
    let response = await fetch('Home/DeleteTask', {
        method: "POST",
        headers : {'Content-Type' : 'application/json'},
        body: JSON.stringify({Id : Id})
    });
    if (!response.ok) {
        const message = `Il y'a eu une erreur : ${response.status}`;
        throw new Error(message);
    } else {
        location.reload();
    }
}

async function UpdateTask(taskId, taskName) {
    let Id = parseInt(taskId);
    let todo = taskName;
    let response = await fetch('Home/PopulateForm', {
        method: "GET",
        headers : {'Content-Type' : 'application/json'},
        data: JSON.stringify({Id : Id})
    });
    if (!response.ok) {
        const message = `Il y'a eu une erreur : ${response.status}`;
        throw new Error(message);
    } else {
        document.getElementById("Todo_Task").value = todo; 
        document.getElementById("Todo_Id").value = Id;
        document.getElementById("form-button").value = "Update Todo";
        document.getElementById("form-action").setAttribute("action", "Home/Update");
    }
}

document.addEventListener("DOMContentLoaded", () => {
    InitDeleteListener();
    InitUpdateListener();
});