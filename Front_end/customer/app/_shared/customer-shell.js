/**
 * BAYTACK — Customer App Shared Shell
 * ============================================================
 * Injects the header, footer and mobile bottom-nav that are
 * visually identical to the public Home Page, into every
 * Customer portal page. Also resolves relative paths so the
 * same script works from both /customer/dashboard/ and
 * /customer/app/<page>/.
 *
 * Usage on every customer page:
 *   <body data-page="dashboard">
 *     <div id="bt-header-mount"></div>
 *     ... page content ...
 *     <div id="bt-footer-mount"></div>
 *     <script src="../app/_shared/customer-data.js"></script>   (path varies by depth)
 *     <script src="../app/_shared/customer-shell.js"></script>
 *   </body>
 * ============================================================
 */
(function () {
  'use strict';

  /* ── Resolve path depth relative to /customer/ ── */
  const path = window.location.pathname;
  const marker = '/customer/';
  const mi = path.indexOf(marker);
  const after = mi !== -1 ? path.slice(mi + marker.length) : '';
  const depth = after.split('/').filter(Boolean).length - 1; // folders below /customer/
  const R = depth > 0 ? '../'.repeat(depth) : './';   // -> points to /customer/
  const SITE = R + '../';                              // -> points to project root

  const PAGE = document.body.dataset.page || '';

  const NAV = [
    { key: 'dashboard', label: 'Dashboard', icon: 'space_dashboard', href: `${R}dashboard/index.html` },
    { key: 'browse', label: 'Browse Services', icon: 'travel_explore', href: `${R}app/browse/index.html` },
    { key: 'requests', label: 'My Requests', icon: 'edit_note', href: `${R}app/requests/index.html` },
    { key: 'orders', label: 'Orders', icon: 'assignment', href: `${R}app/orders/index.html` },
  ];

  const MOBILE_NAV = [
    { key: 'dashboard', label: 'Home', icon: 'space_dashboard', href: `${R}dashboard/index.html` },
    { key: 'browse', label: 'Browse', icon: 'travel_explore', href: `${R}app/browse/index.html` },
    { key: 'orders', label: 'Orders', icon: 'assignment', href: `${R}app/orders/index.html` },
    { key: 'messages', label: 'Chat', icon: 'chat', href: `${R}app/messages/index.html` },
    { key: 'profile', label: 'Profile', icon: 'person', href: `${R}app/profile/index.html` },
  ];

  function icon(name, extra) {
    return `<span class="material-symbols-outlined ${extra || ''}">${name}</span>`;
  }

  function buildHeader() {
    const user = (window.BTData && BTData.currentUser()) || { name: 'Guest Customer' };
    const initials = (user.name || 'C').trim().charAt(0).toUpperCase();
    const unreadNotif = window.BTData ? BTData.notifications.unreadCount() : 0;
    const unreadMsg = window.BTData ? BTData.messages.unreadCount() : 0;

    const navLinks = NAV.map(n => `
      <a href="${n.href}" class="font-medium transition-colors ${n.key === PAGE ? 'text-primary font-bold' : 'text-on-surface-variant hover:text-primary'}">
        ${n.label}
      </a>`).join('');

    return `
    <header class="fixed top-0 left-0 right-0 z-50 bg-white/80 backdrop-blur-md border-b border-gray-100">
      <div class="max-w-7xl mx-auto px-6 h-20 flex items-center justify-between gap-4">
        <a href="${R}dashboard/index.html" class="flex items-center gap-3 shrink-0">
          <img src="${SITE}assets/images/baytack-logo.png" alt="BayTack" style="height:3.5rem;width:auto;object-fit:contain" />
        </a>

        <nav class="hidden lg:flex items-center gap-8 text-sm">${navLinks}</nav>

        <div class="hidden xl:flex items-center bg-surface-container rounded-full px-4 py-2 w-64">
          ${icon('search', 'text-on-surface-variant mr-2 text-xl')}
          <input class="bg-transparent outline-none w-full text-sm" placeholder="Search services..." id="bt-header-search" />
        </div>

        <div class="flex items-center gap-2">
          <a href="${R}app/notifications/index.html" class="relative w-10 h-10 rounded-full hover:bg-surface-container flex items-center justify-center transition" aria-label="Notifications">
            ${icon('notifications')}
            ${unreadNotif > 0 ? `<span class="absolute top-1.5 right-1.5 w-2.5 h-2.5 bg-error rounded-full border-2 border-white"></span>` : ''}
          </a>
          <a href="${R}app/messages/index.html" class="relative w-10 h-10 rounded-full hover:bg-surface-container hidden sm:flex items-center justify-center transition" aria-label="Messages">
            ${icon('mail')}
            ${unreadMsg > 0 ? `<span class="absolute top-1.5 right-1.5 w-2.5 h-2.5 bg-error rounded-full border-2 border-white"></span>` : ''}
          </a>

          <div class="relative" id="bt-avatar-menu-wrap">
            <button id="bt-avatar-btn" class="w-10 h-10 rounded-full bg-primary text-on-primary flex items-center justify-center font-bold border-2 border-primary-container cursor-pointer">
              ${initials}
            </button>
            <div id="bt-avatar-menu" class="hidden absolute right-0 mt-2 w-56 bg-white rounded-2xl shadow-xl border border-gray-100 py-2 z-50">
              <div class="px-4 py-2 border-b border-gray-100">
                <p class="font-bold text-sm text-on-surface truncate">${user.name}</p>
                <p class="text-xs text-on-surface-variant">Customer</p>
              </div>
              <a href="${R}app/profile/index.html" class="flex items-center gap-3 px-4 py-2.5 text-sm hover:bg-surface-container-low transition">${icon('person', 'text-lg')} My Profile</a>
              <a href="${R}app/saved/index.html" class="flex items-center gap-3 px-4 py-2.5 text-sm hover:bg-surface-container-low transition">${icon('favorite', 'text-lg')} Saved Services</a>
              <a href="${R}app/settings/index.html" class="flex items-center gap-3 px-4 py-2.5 text-sm hover:bg-surface-container-low transition">${icon('settings', 'text-lg')} Settings</a>
              <div class="border-t border-gray-100 mt-1 pt-1">
                <button id="bt-logout-btn" class="w-full flex items-center gap-3 px-4 py-2.5 text-sm text-error hover:bg-red-50 transition text-left">${icon('logout', 'text-lg')} Logout</button>
              </div>
            </div>
          </div>

          <button class="lg:hidden w-10 h-10 rounded-full hover:bg-surface-container flex items-center justify-center" id="bt-mobile-nav-toggle">
            ${icon('menu')}
          </button>
        </div>
      </div>

      <!-- Mobile dropdown nav -->
      <div id="bt-mobile-nav-panel" class="hidden lg:hidden border-t border-gray-100 bg-white px-6 py-3 flex flex-col gap-1">
        ${NAV.map(n => `<a href="${n.href}" class="py-2 text-sm font-medium ${n.key === PAGE ? 'text-primary font-bold' : 'text-on-surface-variant'}">${n.label}</a>`).join('')}
      </div>
    </header>
    <div style="height:5rem"></div>`;
  }

  function buildFooter() {
    return `
    <footer class="bg-surface-container-lowest border-t border-outline-variant/10 mt-20 pb-28 md:pb-10">
      <div class="max-w-7xl mx-auto px-6 py-10 flex flex-col md:flex-row items-center justify-between gap-4 text-sm text-on-surface-variant">
        <p>&copy; 2026 BayTack Services. All rights reserved.</p>
        <div class="flex items-center gap-6">
          <a href="${SITE}index.html#contact" class="hover:text-primary transition-colors">Help Center</a>
          <a href="${SITE}index.html#about" class="hover:text-primary transition-colors">About</a>
          <a href="${R}app/settings/index.html" class="hover:text-primary transition-colors">Privacy</a>
        </div>
      </div>
    </footer>

    <!-- Mobile bottom nav -->
    <nav class="fixed bottom-0 left-0 w-full z-50 flex justify-around items-center px-2 pb-6 pt-3 md:hidden bg-white/90 backdrop-blur-2xl shadow-[0px_-8px_24px_rgba(44,47,49,0.08)] rounded-t-3xl">
      ${MOBILE_NAV.map(n => `
        <a href="${n.href}" class="flex flex-col items-center justify-center rounded-full px-3 py-1.5 transition-all duration-150 ${n.key === PAGE ? 'bg-blue-100 text-primary' : 'text-slate-500'}">
          ${icon(n.icon)}
          <span class="text-[10px] font-semibold mt-0.5">${n.label}</span>
        </a>`).join('')}
    </nav>`;
  }

  function mount() {
    const hMount = document.getElementById('bt-header-mount');
    const fMount = document.getElementById('bt-footer-mount');
    if (hMount) hMount.outerHTML = buildHeader();
    if (fMount) fMount.outerHTML = buildFooter();

    const avatarBtn = document.getElementById('bt-avatar-btn');
    const avatarMenu = document.getElementById('bt-avatar-menu');
    if (avatarBtn && avatarMenu) {
      avatarBtn.addEventListener('click', (e) => {
        e.stopPropagation();
        avatarMenu.classList.toggle('hidden');
      });
      document.addEventListener('click', () => avatarMenu.classList.add('hidden'));
    }

    const mobileToggle = document.getElementById('bt-mobile-nav-toggle');
    const mobilePanel = document.getElementById('bt-mobile-nav-panel');
    if (mobileToggle && mobilePanel) {
      mobileToggle.addEventListener('click', () => mobilePanel.classList.toggle('hidden'));
    }

    const logoutBtn = document.getElementById('bt-logout-btn');
    if (logoutBtn) {
      logoutBtn.addEventListener('click', async () => {
        try { if (window.CustomerApi) await CustomerApi.auth.logout(); }
        catch (err) { /* still clear local session below even if the network call fails */ }
        localStorage.removeItem('ek_user_session');
        window.location.href = `${SITE}index.html`;
      });
    }

    const headerSearch = document.getElementById('bt-header-search');
    if (headerSearch) {
      headerSearch.addEventListener('keydown', (e) => {
        if (e.key === 'Enter' && headerSearch.value.trim()) {
          window.location.href = `${R}app/browse/index.html?q=${encodeURIComponent(headerSearch.value.trim())}`;
        }
      });
    }
  }

  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', mount);
  } else {
    mount();
  }

  window.BT_ROOT = R;
  window.BT_SITE_ROOT = SITE;
})();
