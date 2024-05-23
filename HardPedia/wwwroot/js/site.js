// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function confirmDelete(categoryName, deleteUrl) {
    if (confirm("Are you sure you want to delete the category '" + categoryName + "'?")) {
        window.location.href = deleteUrl;
    }
}
function fetchSubject(categoryId, url, direction) {
    var currentSubjectId = document.querySelector(`#subject-container-${categoryId} .subject-content`).getAttribute('data-current-subject-id');
    fetch(`${url}?categoryId=${categoryId}&currentSubjectId=${currentSubjectId}&direction=${direction}`)
        .then(response => response.json())
        .then(data => {
            document.querySelector(`#subject-container-${categoryId}`).innerHTML = `
                <div class="subject-content" data-current-subject-id="${data.id}">
                    <div class="subject-title">
                        <a href="/Subject/ListSubject?id=${data.id}" class="subject-link">${data.title}</a>
                    </div>
                    <p>${data.shortContent}</p>
                </div>
            `;
        })
        .catch(error => console.error('Error fetching subject:', error));
}
