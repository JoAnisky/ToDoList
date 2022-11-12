// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function InitDeleteListener() {
    const deleteBtn = document.querySelectorAll(".delete-btn");
    deleteBtn.forEach(btn => {
        btn.addEventListener("click", (e) => {
            let taskNumber = e.target.dataset.taskId;
            DeleteTodo(taskNumber)
        })
    })

}

function DeleteTodo(id) {
    let taskNumber = id;
}

document.addEventListener("DOMContentLoaded", () => {
    InitDeleteListener();
});