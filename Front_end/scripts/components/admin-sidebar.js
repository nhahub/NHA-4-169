/**
 * BAYTACK ADMIN — Admin Sidebar Component
 * Components are inlined directly in HTML - no fetch needed.
 */

import Config  from '../core/config.js';
import Storage from '../core/storage.js';

const AdminSidebarComponent = {

  async init() {
    // Components already inlined in HTML - just wire up behaviour
    this._highlightActive();
    this._bindMobileToggle();
    this._bindBottomNav();
    this._bindFabBtn();
    this._bindLogout();
    this._syncUser();
    this._bindNotifications();
  },

  _highlightActive() {
    const pageMap = { 'verification-review': 'verification' };
    const rawPage = document.body.dataset.page;
    const page    = pageMap[rawPage] || rawPage;
    if (!page) return;

    document.querySelectorAll('.admin-sidebar__nav-link[data-page]').forEach(link => {
      link.classList.toggle('is-active', link.dataset.page === page);
    });
    document.querySelectorAll('.admin-bottom-nav__item[data-page]').forEach(btn => {
      btn.classList.toggle('is-active', btn.dataset.page === page);
    });
  },

  _bindMobileToggle() {
    const sidebar = document.getElementById('admin-sidebar');
    const overlay = document.getElementById('admin-sidebar-overlay');
    const toggle  = document.getElementById('admin-sidebar-toggle');
    if (!sidebar) return;

    // Lock body scroll while the sidebar is open on mobile, so a touch that
    // starts outside the sidebar can't drag the page behind it — the
    // sidebar itself scrolls internally (see admin-sidebar.css overflow-y).
    const open  = () => {
      sidebar.classList.add('is-open');
      overlay?.classList.add('is-visible');
      document.body.classList.add('admin-body--sidebar-open');
    };
    const close = () => {
      sidebar.classList.remove('is-open');
      overlay?.classList.remove('is-visible');
      document.body.classList.remove('admin-body--sidebar-open');
    };

    toggle?.addEventListener('click', open);
    overlay?.addEventListener('click', close);
    document.addEventListener('keydown', e => { if (e.key === 'Escape') close(); });

    // Collapse back to desktop layout without leaving the body locked.
    window.addEventListener('resize', () => {
      if (window.innerWidth >= 768) close();
    });
  },

  /* ── Notifications dropdown ── */
  _bindNotifications() {
    const wrap  = document.getElementById('admin-notif');
    const btn   = document.getElementById('admin-notif-btn');
    const panel = document.getElementById('admin-notif-panel');
    const list  = document.getElementById('admin-notif-list');
    const badge = document.getElementById('admin-notif-badge');
    const markAll = document.getElementById('admin-notif-mark-all');
    if (!wrap || !btn || !panel || !list) return;

    const NOTIFICATIONS = [
      { id: 'n1', icon: 'verified',      title: 'New provider awaiting verification', desc: 'Cairo Electric Pros submitted documents for review.', time: '5m ago' },
      { id: 'n2', icon: 'receipt_long',  title: 'Order marked completed',             desc: '#EK-9482 — Full Villa AC Revamp closed successfully.',  time: '1h ago' },
      { id: 'n3', icon: 'report',        title: 'New complaint filed',                desc: 'A customer reported an issue with Nile Cleaning Co.',   time: '3h ago' },
      { id: 'n4', icon: 'person_add',    title: 'New provider signup',                desc: 'Master Plumbers Giza completed registration.',          time: 'Yesterday' },
    ];
    const READ_KEY = 'ek_admin_notifications_read';

    const getRead = () => {
      const val = Storage.get(READ_KEY, []);
      return Array.isArray(val) ? val : [];
    };
    const setRead = (ids) => Storage.set(READ_KEY, ids);

    const render = () => {
      const read = getRead();

      list.innerHTML = NOTIFICATIONS.map(n => `
        <button class="admin-notif-item${read.includes(n.id) ? '' : ' is-unread'}" data-id="${n.id}" type="button" role="menuitem">
          <span class="material-symbols-outlined admin-notif-item__icon">${n.icon}</span>
          <span class="admin-notif-item__body">
            <span class="admin-notif-item__title">${n.title}</span>
            <span class="admin-notif-item__desc">${n.desc}</span>
            <span class="admin-notif-item__time">${n.time}</span>
          </span>
        </button>
      `).join('') || `<p class="admin-notif-panel__empty">You're all caught up.</p>`;

      const unread = NOTIFICATIONS.filter(n => !read.includes(n.id)).length;
      badge?.classList.toggle('is-visible', unread > 0);

      list.querySelectorAll('[data-id]').forEach(item => {
        item.addEventListener('click', () => {
          const ids = getRead();
          if (!ids.includes(item.dataset.id)) setRead([...ids, item.dataset.id]);
          render();
        });
      });
    };

    const openPanel = () => {
      panel.classList.add('is-open');
      panel.setAttribute('aria-hidden', 'false');
      btn.setAttribute('aria-expanded', 'true');
    };
    const closePanel = () => {
      panel.classList.remove('is-open');
      panel.setAttribute('aria-hidden', 'true');
      btn.setAttribute('aria-expanded', 'false');
    };

    btn.addEventListener('click', (e) => {
      e.stopPropagation();
      panel.classList.contains('is-open') ? closePanel() : openPanel();
    });
    markAll?.addEventListener('click', () => {
      setRead(NOTIFICATIONS.map(n => n.id));
      render();
    });
    document.addEventListener('click', (e) => {
      if (!wrap.contains(e.target)) closePanel();
    });
    document.addEventListener('keydown', (e) => { if (e.key === 'Escape') closePanel(); });

    render();
  },

  _bindBottomNav() {
    document.querySelectorAll('.admin-bottom-nav__item[data-page]').forEach(btn => {
      btn.addEventListener('click', () => {
        const route = Config.ROUTES[btn.dataset.page];
        if (route) window.location.href = route;
      });
    });
  },

  _bindFabBtn() {
    document.addEventListener('click', e => {
      const fab = e.target.closest('.admin-bottom-nav__fab-btn');
      if (!fab) return;

      let sheet = document.getElementById('fab-quick-sheet');
      if (sheet) { sheet.remove(); return; }

      sheet = document.createElement('div');
      sheet.id = 'fab-quick-sheet';
      sheet.style.cssText = `
        position:fixed;bottom:6rem;left:50%;transform:translateX(-50%);
        background:var(--color-surface-container-lowest);
        border-radius:1rem;box-shadow:0 8px 32px rgba(0,0,0,0.18);
        padding:1rem;display:flex;flex-direction:column;gap:0.5rem;
        z-index:500;min-width:200px;
      `;

      const actions = [
        { label:'Analytics',    page:'analytics',       icon:'bar_chart' },
        { label:'Users',        page:'user-management', icon:'manage_accounts' },
        { label:'Orders',       page:'orders',          icon:'receipt_long' },
        { label:'Verification', page:'verification',    icon:'verified' },
        { label:'Providers',    page:'providers',       icon:'engineering' },
      ];

      actions.forEach(a => {
        const btn = document.createElement('button');
        btn.style.cssText = `
          display:flex;align-items:center;gap:0.75rem;
          padding:0.75rem 1rem;border-radius:0.5rem;border:none;
          background:none;cursor:pointer;font-size:0.95rem;font-weight:600;
          color:var(--color-on-surface);font-family:inherit;width:100%;text-align:left;
        `;
        btn.innerHTML = `<span class="material-symbols-outlined" style="font-size:1.2rem">${a.icon}</span>${a.label}`;
        btn.onmouseover = () => btn.style.background = 'var(--color-surface-container)';
        btn.onmouseout  = () => btn.style.background = 'none';
        btn.addEventListener('click', () => {
          const route = Config.ROUTES?.[a.page];
          if (route) window.location.href = route;
          sheet.remove();
        });
        sheet.appendChild(btn);
      });

      document.body.appendChild(sheet);
      setTimeout(() => {
        document.addEventListener('click', function closeSheet(ev) {
          if (!sheet.contains(ev.target) && !ev.target.closest('.admin-bottom-nav__fab-btn')) {
            sheet.remove();
            document.removeEventListener('click', closeSheet);
          }
        });
      }, 50);
    });
  },

  _bindLogout() {
    document.addEventListener('click', e => {
      if (!e.target.closest('#admin-logout-btn')) return;
      Storage.remove(Config.STORAGE_KEYS.ACCESS_TOKEN);
      Storage.remove(Config.STORAGE_KEYS.USER);
      window.location.href = '../index.html';
    });
  },

  _syncUser() {
    try {
      const user = JSON.parse(Storage.get(Config.STORAGE_KEYS.USER) || '{}');
      const nameEl   = document.getElementById('admin-header-username');
      const avatarEl = document.getElementById('admin-header-avatar');
      if (nameEl && user.name) nameEl.textContent = user.name;
      if (avatarEl && user.name) {
        avatarEl.src = `https://ui-avatars.com/api/?name=${encodeURIComponent(user.name)}&background=0050d4&color=fff&size=80`;
      }
    } catch {}
  },
};

export default AdminSidebarComponent;
