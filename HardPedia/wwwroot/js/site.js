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
                        <a href="/Subject/ListSubject?id=${data.id}" class="subject-link h4">${data.title}</a>
                    </div>
                    <p>${data.shortContent}</p>
                </div>
            `;
        })
        .catch(error => console.error('Error fetching subject:', error));
}

document.addEventListener('DOMContentLoaded', (event) => {
    const searchInput = document.getElementById('search-query');
    const searchUrl = searchInput.getAttribute('data-search-url');

    searchInput.addEventListener('input', function (event) {
        performSearch(event, searchUrl);
    });
});

function performSearch(event, searchUrl) {
    event.preventDefault();
    const query = document.getElementById('search-query').value;

    fetch(`${searchUrl}?query=${encodeURIComponent(query)}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    })
        .then(response => response.text())
        .then(html => {
            document.getElementById('categories-container').innerHTML = html;
        })
        .catch(error => console.error('Error:', error));

    return false;
}
