(function () {
    var root = document.documentElement;
    var toggle = document.getElementById("theme-toggle");
    if (toggle) {
        toggle.addEventListener("click", function () {
            var next = root.getAttribute("data-theme") === "dark" ? "light" : "dark";
            root.setAttribute("data-theme", next);
            localStorage.setItem("theme", next);
            var shown = document.getElementById("theme-now");
            if (shown) {
                shown.textContent = next;
            }
        });
    }

    var themeNow = document.getElementById("theme-now");
    if (themeNow) {
        themeNow.textContent = localStorage.getItem("theme") || "light (default)";
    }

    sessionStorage.setItem("lastPath", window.location.pathname);
    var last = document.getElementById("last-path");
    if (last) {
        last.textContent = sessionStorage.getItem("lastPath");
    }

})();
