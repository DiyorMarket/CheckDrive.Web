document.addEventListener("DOMContentLoaded", () => {
    const currentUrl = window.location.pathname;
    const menuItems = document.querySelectorAll('.e-menu-item');

    menuItems.forEach(item => {
        const anchor = item.querySelector('a');
        if (item.textContent === 'Dashboard' && location.pathname === '/Dashboard') {
            item.classList.add('active-menu-item');
        }
        else if (anchor && anchor.getAttribute('href') === currentUrl) {
            item.classList.add('active-menu-item'); 
            if (item.id === "handoversItem") {
                updateTarixUrl("/mechanichandovers/historyindexforpersonalpage");
            }
        }

        item.addEventListener('click', () => {
            menuItems.forEach(i => i.classList.remove('active-menu-item'));
            item.classList.add('active-menu-item');
        });
    });

    function updateTarixUrl(newUrl) {
        var tarixMenuItem = document.getElementById("historyItem");
        var historyLink = tarixMenuItem.querySelector("a.e-menu-url");
        if (historyLink) {
            historyLink.href = newUrl;
        }
    }
});
    
function toggleDropdown() {
    const dropdown = document.getElementById('dropdown');
    if (dropdown.style.display === 'none' || dropdown.style.display === '') {
        dropdown.style.display = 'block';
    } else {
        dropdown.style.display = 'none';
    }
}
    