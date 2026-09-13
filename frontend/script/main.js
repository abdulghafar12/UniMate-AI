// =====================================================================
// UniMate AI — main.js
// Step 1 only handles the mobile hamburger menu. Later steps will add
// auth.js, dashboard.js, gpa.js, ai.js, roadmap.js etc.
// =====================================================================

document.addEventListener("DOMContentLoaded", function () {
  const navToggle = document.getElementById("navToggle");
  const mainNav = document.getElementById("mainNav");

  if (!navToggle || !mainNav) return;

  navToggle.addEventListener("click", function () {
    const isOpen = mainNav.classList.toggle("is-open");
    navToggle.setAttribute("aria-expanded", isOpen ? "true" : "false");
  });

  // Close the mobile menu when a link is tapped, so the user
  // actually sees the section they jumped to.
  mainNav.querySelectorAll("a").forEach(function (link) {
    link.addEventListener("click", function () {
      mainNav.classList.remove("is-open");
      navToggle.setAttribute("aria-expanded", "false");
    });
  });
});
