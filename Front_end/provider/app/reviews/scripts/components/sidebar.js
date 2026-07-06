// ============================================
// PROVIDER HUB — Sidebar UI Component
// File: /scripts/components/sidebar.js
// ============================================

import { qs, on, toggleClass, addClass, removeClass } from '../core/helpers.js';

const Sidebar = (() => {

  let sidebar    = null;
  let menuBtn    = null;
  let overlay    = null;
  let isOpen     = false;

  /** Create a backdrop overlay element */
  const _createOverlay = () => {
    const div = document.createElement('div');
    div.id = 'sidebarOverlay';
    div.style.cssText = `
      position: fixed; inset: 0;
      background: rgba(0,0,0,0.25);
      z-index: 39;
      opacity: 0;
      transition: opacity 0.3s ease;
      display: none;
    `;
    document.body.appendChild(div);
    return div;
  };

  const open = () => {
    if (!sidebar) return;
    addClass(sidebar, 'is-open');
    overlay.style.display = 'block';
    requestAnimationFrame(() => { overlay.style.opacity = '1'; });
    menuBtn?.setAttribute('aria-expanded', 'true');
    document.body.style.overflow = 'hidden';
    isOpen = true;
  };

  const close = () => {
    if (!sidebar) return;
    removeClass(sidebar, 'is-open');
    overlay.style.opacity = '0';
    setTimeout(() => { overlay.style.display = 'none'; }, 300);
    menuBtn?.setAttribute('aria-expanded', 'false');
    document.body.style.overflow = '';
    isOpen = false;
  };

  const toggle = () => isOpen ? close() : open();

  /**
   * Highlight the active nav link based on current page
   * @param {string} activePage - e.g. 'reviews'
   */
  const setActivePage = (activePage) => {
    if (!sidebar) return;
    sidebar.querySelectorAll('.sidebar__nav-link').forEach(link => {
      const page = link.dataset.page;
      if (page === activePage) {
        addClass(link, 'sidebar__nav-link--active');
        link.setAttribute('aria-current', 'page');
      } else {
        removeClass(link, 'sidebar__nav-link--active');
        link.removeAttribute('aria-current');
      }
    });
  };

  const init = () => {
    sidebar = qs('#sidebar');
    menuBtn = qs('#menuToggleBtn');
    overlay = _createOverlay();

    // Mobile menu toggle
    on(menuBtn, 'click', toggle);

    // Close on overlay click
    on(overlay, 'click', close);

    // Close on Escape key
    on(document, 'keydown', (e) => {
      if (e.key === 'Escape' && isOpen) close();
    });

    // Close sidebar on desktop resize (no need for overlay)
    on(window, 'resize', () => {
      if (window.innerWidth >= 768 && isOpen) close();
    });

    // Serve Me button
    const serveMeBtn = qs('#serveMeBtn');
    on(serveMeBtn, 'click', () => {
      console.log('[Sidebar] "Serve Me" clicked — route to onboarding or availability page');
      // router.navigate('/serve') — wire up to your router
    });
  };

  return { init, open, close, toggle, setActivePage };
})();

export default Sidebar;
