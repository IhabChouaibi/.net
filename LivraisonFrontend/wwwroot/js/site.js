// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

document.addEventListener("DOMContentLoaded", function () {
    const sidebar = document.getElementById("appSidebar");
    const sidebarToggle = document.querySelector("[data-sidebar-toggle]");

    if (sidebarToggle && sidebar) {
        sidebarToggle.addEventListener("click", function () {
            sidebar.classList.toggle("sidebar-open");
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
            paging: true,
            pageLength: 10,
            lengthChange: false,
            info: false,
            searching: true,
            ordering: true,
            responsive: true,
            language: {
                search: "",
                searchPlaceholder: "Rechercher...",
                paginate: {
                    previous: "Préc.",
                    next: "Suiv."
                },
                zeroRecords: "Aucune donnée trouvée"
            }
        });
    });

    document.querySelectorAll("[data-bs-toggle='tooltip']").forEach(function (element) {
        new bootstrap.Tooltip(element);
    });

    if (window.jQuery && $.fn.select2) {
        $(".select2-premium").each(function () {
            const placeholder = this.dataset.placeholder || "Sélectionner";
            $(this).select2({
                theme: "bootstrap-5",
                width: "100%",
                placeholder: placeholder
            });
        });
    }
});
