/**
 * sidebar.js — Sidebar open/close, active state, and tablet collapse
 * The Radiant Marketplace — Provider Hub
 */

const SidebarComponent = (() => {
  // ── Elements ──────────────────────────────────────────────
  let sidebar, overlay, menuBtn;

  // ── State ─────────────────────────────────────────────────
  let isOpen = false;

  // ── Private helpers ───────────────────────────────────────

  function open() {
    isOpen = true;
    sidebar.classList.add('is-open');
    overlay.classList.add('is-visible');
    document.body.style.overflow = 'hidden';
    menuBtn?.classList.add('is-open');
  }

  function close() {
    isOpen = false;
    sidebar.classList.remove('is-open');
    overlay.classList.remove('is-visible');
    document.body.style.overflow = '';
    menuBtn?.classList.remove('is-open');
  }

  function toggle() {
    isOpen ? close() : open();
  }

  /**
   * Mark the nav link whose href matches the current page as active.
   */
  function setActiveLink() {
    const currentPath = window.location.pathname.split('/').pop() || 'index.html';
    sidebar.querySelectorAll('.nav-link').forEach(link => {
      const href = link.getAttribute('href') || '';
      link.classList.toggle('is-active', href === currentPath || href === '#');
    });
  }

  // ── Public API ────────────────────────────────────────────
  return {
    init() {
      sidebar   = document.querySelector('.site-sidebar');
      overlay   = document.querySelector('.sidebar-overlay');
      menuBtn   = document.querySelector('.mobile-menu-btn');

      if (!sidebar) return;

      // Mobile hamburger
      menuBtn?.addEventListener('click', toggle);

      // Overlay click to close
      overlay?.addEventListener('click', close);

      // Close on Escape
      document.addEventListener('keydown', e => {
        if (e.key === 'Escape' && isOpen) close();
      });

      // Set active nav link
      setActiveLink();
    },

    open,
    close,
    toggle,
  };
})();

export default SidebarComponent;
