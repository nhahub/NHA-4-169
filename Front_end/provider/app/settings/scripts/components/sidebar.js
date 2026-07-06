/**
 * components/sidebar.js
 * ─────────────────────────────────────────────────────────
 * Manages the sidebar open/close on mobile via a hamburger toggle.
 * Adds/removes CSS classes; all styles live in _sidebar.css.
 */

export function initSidebar() {
  const sidebar  = document.getElementById('sidebar');
  const overlay  = document.getElementById('sidebar-overlay');
  const toggleBtn = document.getElementById('sidebar-toggle');

  if (!sidebar) return;

  /** Open the sidebar (mobile) */
  function open() {
    sidebar.classList.add('sidebar--open');
    if (overlay) overlay.classList.add('sidebar-overlay--visible');
    document.body.style.overflow = 'hidden';
    if (toggleBtn) toggleBtn.setAttribute('aria-expanded', 'true');
  }

  /** Close the sidebar (mobile) */
  function close() {
    sidebar.classList.remove('sidebar--open');
    if (overlay) overlay.classList.remove('sidebar-overlay--visible');
    document.body.style.overflow = '';
    if (toggleBtn) toggleBtn.setAttribute('aria-expanded', 'false');
  }

  /** Toggle */
  function toggle() {
    sidebar.classList.contains('sidebar--open') ? close() : open();
  }

  // Wire events
  if (toggleBtn) toggleBtn.addEventListener('click', toggle);
  if (overlay)   overlay.addEventListener('click', close);

  // Close on Escape key
  document.addEventListener('keydown', (e) => {
    if (e.key === 'Escape') close();
  });

  // Close when a nav link is tapped on mobile
  sidebar.querySelectorAll('.sidebar__link').forEach((link) => {
    link.addEventListener('click', () => {
      if (window.innerWidth < 768) close();
    });
  });
}
