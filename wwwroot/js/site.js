// User dropdown toggle
document.addEventListener('click', function (e) {
    const toggle = document.querySelector('.user-menu-toggle');
    const menu = document.getElementById('UserDropdown');
    const target = e.target; // Casteamos el target a Node para poder usar .contains()
    if (toggle && menu && target) {
        if (toggle.contains(target)) {
            menu.classList.toggle('show');
        }
        else if (!menu.contains(target)) {
            menu.classList.remove('show');
        }
    }
});
//# sourceMappingURL=site.js.map