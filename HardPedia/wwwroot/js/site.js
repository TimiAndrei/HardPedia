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
                    <pre class="custom-short-content">${data.shortContent}</pre>
                </div>
            `;
        })
        .catch(error => console.error('Error fetching subject:', error));
}

document.addEventListener('DOMContentLoaded', (event) => {

    formatInitialShortContent();
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
            const parser = new DOMParser();
            const doc = parser.parseFromString(html, 'text/html');

            doc.querySelectorAll('p').forEach(p => {
                const pre = document.createElement('pre');
                pre.classList.add('custom-short-content');
                let content = p.innerHTML;
                content = content.replace(/(<br\s*\/?>\s*){2}/gi, '<br>');
                pre.innerHTML = content;
                p.replaceWith(pre);
            });

            document.getElementById('categories-container').innerHTML = doc.body.innerHTML; 
        })
        .catch(error => console.error('Error:', error));

    return false;
}





function formatShortContent(shortContent) {
    return shortContent.replace(/\n/g, '<br>');
}

function formatInitialShortContent() {
    const shortContentElements = document.querySelectorAll('.custom-short-content');

    shortContentElements.forEach(element => {
        const shortContent = element.textContent;
        const formattedContent = formatShortContent(shortContent);
        element.innerHTML = formattedContent;
    });
}
