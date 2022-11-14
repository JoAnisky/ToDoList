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
            UpdateTask(taskId);
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
        console.log(response);
        location.reload();
    }
}

async function DeleteTask(taskId) {
    let Id = parseInt(taskId);
    let response = await fetch('Home/PopulateForm', {
        method: "GET",
        headers : {'Content-Type' : 'application/json'},
        body: JSON.stringify({Id : Id})
    });
    if (!response.ok) {
        const message = `Il y'a eu une erreur : ${response.status}`;
        throw new Error(message);
    } else {
        response => {

        }
    }
}

document.addEventListener("DOMContentLoaded", () => {
    InitDeleteListener();
    InitUpdateListener();
});