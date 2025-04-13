document.addEventListener("DOMContentLoaded", function() {
    const sidebar = document.getElementById("sidebar");
    const toggleButton = document.getElementById("toggleSidebar");
    const content = document.getElementById("content");

    toggleButton.addEventListener("click", function() {
        sidebar.classList.toggle("sidebar-hidden");
        content.classList.toggle("content-expanded");
    });
});