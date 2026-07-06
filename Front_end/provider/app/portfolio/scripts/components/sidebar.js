/**
 * sidebar.js — Sidebar UI interactions
 * Radiant Pro / BayTack
 *
 * Handles:
 *  - Mobile open/close toggle
 *  - Active link highlighting
 *  - Backdrop click to close
 */

import { $, $$ } from "../core/helpers.js";

/** @type {HTMLElement|null} */
let sidebar = null;
/** @type {HTMLElement|null} */
let backdrop = null;

/**
 * Initialise the sidebar.
 * Must be called after the sidebar HTML is injected into the DOM.
 */
export function initSidebar() {
  sidebar = $("#sidebar");
  backdrop = $("#sidebar-backdrop");

  if (!sidebar) return;

  // Close when backdrop is clicked
  backdrop?.addEventListener("click", closeSidebar);

  // Highlight the active link based on data-nav attribute
  _highlightActiveLink();
}

/** Open the sidebar drawer (mobile). */
export function openSidebar() {
  sidebar?.classList.add("is-open");
  backdrop?.classList.add("is-visible");
  document.body.style.overflow = "hidden";
}

/** Close the sidebar drawer (mobile). */
export function closeSidebar() {
  sidebar?.classList.remove("is-open");
  backdrop?.classList.remove("is-visible");
  document.body.style.overflow = "";
}

/** Toggle the sidebar drawer. */
export function toggleSidebar() {
  const isOpen = sidebar?.classList.contains("is-open");
  isOpen ? closeSidebar() : openSidebar();
}

/* ── Private ── */

function _highlightActiveLink() {
  const currentPage = _getCurrentPage();
  const links = $$(".sidebar__nav-link", sidebar);

  links.forEach((link) => {
    const nav = link.dataset.nav;
    link.classList.toggle("sidebar__nav-link--active", nav === currentPage);
    if (nav === currentPage) {
      link.setAttribute("aria-current", "page");
    } else {
      link.removeAttribute("aria-current");
    }
  });
}

/**
 * Derive the current page slug from the URL hash or pathname.
 * @returns {string}
 */
function _getCurrentPage() {
  const hash = location.hash.replace("#", "");
  if (hash) return hash;
  const segments = location.pathname.split("/").filter(Boolean);
  return segments[segments.length - 1] || "dashboard";
}
