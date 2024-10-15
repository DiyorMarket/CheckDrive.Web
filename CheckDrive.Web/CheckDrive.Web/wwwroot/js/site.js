document.addEventListener("DOMContentLoaded", function () {
    const currentUrl = window.location.pathname;
    const menuItems = document.querySelectorAll('.e-menu-item'); 

    menuItems.forEach(item => {
        const anchor = item.querySelector('a');
        if (anchor && anchor.getAttribute('href') === currentUrl) {
            item.classList.add('e-selected');
        }
    });
});

function toggleDropdown() {
    const dropdown = document.getElementById('dropdown');
    if (dropdown.style.display === 'none' || dropdown.style.display === '') {
        dropdown.style.display = 'block';
    } else {
        dropdown.style.display = 'none';
    }
}
