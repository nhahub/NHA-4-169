/**
 * EKHDEMNI — Provider App Shared Navigation
 * ============================================================
 * Inject this script in every provider app page.
 * It builds the sidebar + mobile bottom nav dynamically,
 * highlights the active page, and wires up the session.
 *
 * Usage: <script src="../_shared/ekhdemni-provider-nav.js"></script>
 * Set   data-page="bookings|earnings|portfolio|reviews|settings|bid"
 * on    <body>.
 * ============================================================
 */
(function () {
  'use strict';

  /* ── Routes (relative to /provider/app/<page>/) ── */
  const ROOT = '../..';   // → /provider/
  const APP  = '..';      // → /provider/app/

  const NAV_ITEMS = [
    { page: 'bookings',  label: 'Bookings',  icon: 'event_available', href: `${APP}/bookings/index.html`  },
    { page: 'earnings',  label: 'Earnings',  icon: 'payments',        href: `${APP}/earnings/index.html`  },
    { page: 'portfolio', label: 'Portfolio', icon: 'photo_library',   href: `${APP}/portfolio/index.html` },
    { page: 'reviews',   label: 'Reviews',   icon: 'star_rate',       href: `${APP}/reviews/index.html`   },
    { page: 'settings',  label: 'Settings',  icon: 'tune',            href: `${APP}/settings/index.html`  },
  ];

  /* ── Session guard ── */
  function getSession() {
    try { return JSON.parse(sessionStorage.getItem('ek_provider_signup') || localStorage.getItem('ek_user_session') || 'null'); }
    catch { return null; }
  }

  /* ── Current page ── */
  const currentPage = document.body.dataset.page || '';

  /* ── Inject CSS variables for active state ── */
  const styleEl = document.createElement('style');
  styleEl.textContent = `
    :root {
      --ek-primary: #0050d4;
      --ek-surface: #f5f7f9;
      --ek-on-surface: #1a1c20;
      --ek-outline-var: #c4c6d0;
    }
    .ek-nav-link--active { color: var(--ek-primary) !important; font-weight: 800 !important; }
    .ek-nav-link--active .ek-nav-icon { font-variation-settings: 'FILL' 1, 'wght' 700, 'GRAD' 0, 'opsz' 24 !important; color: var(--ek-primary) !important; }
    .ek-mobile-nav__item--active { color: var(--ek-primary) !important; }
    .ek-mobile-nav__item--active span { font-variation-settings: 'FILL' 1, 'wght' 700, 'GRAD' 0, 'opsz' 24 !important; }
  `;
  document.head.appendChild(styleEl);

  /* ── Apply active state on all nav-link elements ── */
  function applyActiveStates() {
    // Sidebar links
    document.querySelectorAll('[data-page]').forEach(el => {
      const pg = el.dataset.page;
      if (!pg) return;
      if (pg === currentPage) {
        el.classList.add('is-active', 'nav-link--active', 'ek-nav-link--active', 'sidebar__nav-link--active');
        el.setAttribute('aria-current', 'page');
      } else {
        el.classList.remove('is-active', 'nav-link--active', 'ek-nav-link--active', 'sidebar__nav-link--active');
        el.removeAttribute('aria-current');
      }
    });

    // Fix all href links that use old absolute paths
    document.querySelectorAll('a[href]').forEach(a => {
      const href = a.getAttribute('href');
      if (!href || href.startsWith('http') || href.startsWith('#')) return;

      const mapping = {
        '/bookings':  `${APP}/bookings/index.html`,
        '/earnings':  `${APP}/earnings/index.html`,
        '/portfolio': `${APP}/portfolio/index.html`,
        '/reviews':   `${APP}/reviews/index.html`,
        '/settings':  `${APP}/settings/index.html`,
        '/bids':      `${APP}/bid/index.html`,
        '/explore':   `${ROOT}/dashboard/index.html`,
        '/account':   `${APP}/settings/index.html`,
        '/':          `${ROOT}/dashboard/index.html`,
        'earnings.html':  `${APP}/earnings/index.html`,
        'reviews.html':   `${APP}/reviews/index.html`,
        'settings.html':  `${APP}/settings/index.html`,
        'bookings.html':  `${APP}/bookings/index.html`,
        'index.html':     `${APP}/bookings/index.html`,
      };

      if (mapping[href]) {
        a.setAttribute('href', mapping[href]);
        // Set data-page for active detection
        const pg = Object.keys({ bookings:1, earnings:1, portfolio:1, reviews:1, settings:1 }).find(k => mapping[href].includes(`/${k}/`));
        if (pg) a.dataset.page = pg;
      }
    });

    // Fix button-based mobile nav items (onclick nav)
    document.querySelectorAll('.mobile-nav__item, .mobile-nav__link, .bottom-nav__item, .bottom-nav__link').forEach(btn => {
      const pg = btn.dataset.page || btn.dataset.bottomNav;
      if (!pg || !NAV_ITEMS.find(n => n.page === pg)) return;
      // Convert button → anchor if needed for correct navigation
      btn.style.cursor = 'pointer';
      btn.addEventListener('click', () => {
        const item = NAV_ITEMS.find(n => n.page === pg);
        if (item) window.location.href = item.href;
      });
      if (pg === currentPage) {
        btn.classList.add('is-active', 'mobile-nav__item--active', 'bottom-nav__item--active', 'ek-mobile-nav__item--active');
      }
    });
  }

  /* ── Verified account lookup ── */
  function getAccount() {
    try {
      const signup = JSON.parse(sessionStorage.getItem('ek_provider_signup') || '{}');
      const sess   = JSON.parse(localStorage.getItem('ek_user_session') || '{}');
      const accts  = JSON.parse(localStorage.getItem('ek_accounts') || '[]');
      const phone  = signup.phone || sess.phone || '';
      return accts.find(a => a.phone === phone) || signup || sess || {};
    } catch (e) { return {}; }
  }

  /* ── Sync user info in header ── */
  function syncUser() {
    const session = getSession();
    if (!session) return;
    const name = session.name || '';
    document.querySelectorAll('.provider-name, .user-name, [data-provider-name]').forEach(el => {
      el.textContent = name;
    });
    // Avatar initials
    document.querySelectorAll('.avatar-initials, [data-avatar-initials]').forEach(el => {
      el.textContent = name.charAt(0).toUpperCase();
    });

    // Persistent "Verified" badge next to the provider's name
    const account = getAccount();
    if (account.status === 'verified') {
      document.querySelectorAll('.provider-name, .user-name, [data-provider-name]').forEach(el => {
        if (el.querySelector('.ek-verified-badge')) return; // avoid duplicates
        const badge = document.createElement('span');
        badge.className = 'ek-verified-badge';
        badge.title = 'Verified provider';
        badge.setAttribute('aria-label', 'Verified provider');
        badge.style.cssText = 'display:inline-flex;align-items:center;gap:2px;margin-left:6px;color:#059669;font-size:0.85em;font-weight:700;vertical-align:middle;';
        badge.innerHTML = '<span class="material-symbols-outlined" style="font-size:1.05em;font-variation-settings:\'FILL\' 1">verified</span>';
        el.appendChild(badge);
      });
    }
  }

  /* ── Run ── */
  document.addEventListener('DOMContentLoaded', () => {
    applyActiveStates();
    syncUser();
  });

  // Also run immediately in case DOM already loaded
  if (document.readyState !== 'loading') {
    applyActiveStates();
    syncUser();
  }
})();
