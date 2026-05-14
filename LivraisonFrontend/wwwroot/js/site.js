// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

document.addEventListener("DOMContentLoaded", function () {
    const sidebar = document.getElementById("appSidebar");
    const sidebarToggle = document.querySelector("[data-sidebar-toggle]");

    if (sidebarToggle && sidebar) {
        sidebarToggle.addEventListener("click", function () {
            sidebar.classList.toggle("show");
        });
    }

    document.querySelectorAll("form[data-confirm-delete]").forEach(function (form) {
        form.addEventListener("submit", function (event) {
            event.preventDefault();

            Swal.fire({
                title: "Confirmer la suppression",
                text: "Cette action est définitive.",
                icon: "warning",
                showCancelButton: true,
                confirmButtonText: "Oui, supprimer",
                cancelButtonText: "Annuler",
                confirmButtonColor: "#dc2626"
            }).then(function (result) {
                if (result.isConfirmed) {
                    form.submit();
                }
            });
        });
    });

    document.querySelectorAll("table[data-enhance='true']").forEach(function (table) {
        $(table).DataTable({
            paging: false,
            info: false,
            searching: false,
            ordering: true,
            responsive: true
        });
    });
});
