/**
 * BAYTACK ADMIN — Providers Controller
 * Talks to the real BayTack.API admin endpoints via ProvidersService:
 *   GET    /api/admin/providers            (search, categoryId, status, page, limit)
 *   GET    /api/admin/providers/stats
 *   GET    /api/admin/providers/:id
 *   PUT    /api/admin/providers/:id
 *   PATCH  /api/admin/providers/:id/approve
 *   PATCH  /api/admin/providers/:id/suspend
 *   PATCH  /api/admin/providers/:id/reinstate
 */

import AuthService       from '../services/authService.js';
import ProvidersService  from '../services/providersService.js';
import CategoriesService from '../services/categoriesService.js';
import Modal              from '../components/modal.js';
import { showToast, debounce } from '../core/helpers.js';

/* The backend's VerificationStatus enum <-> the labels this page shows. */
const STATUS_TO_API = {
  verified:  'Approved',
  pending:   'Pending',
  review:    'UnderReview',
  suspended: 'Suspended',
};
const STATUS_FROM_API = {
  Approved:    'verified',
  Pending:     'pending',
  UnderReview: 'review',
  Suspended:   'suspended',
  Rejected:    'rejected',
};

const STATUS_CONFIG = {
  verified:  { label: 'Verified',      icon: 'verified',      cls: 'verification-status--verified'  },
  pending:   { label: 'Pending',       icon: 'schedule',      cls: 'verification-status--pending'   },
  review:    { label: 'Under Review',  icon: 'manage_search', cls: 'verification-status--review'    },
  suspended: { label: 'Suspended',     icon: 'block',         cls: 'verification-status--suspended' },
  rejected:  { label: 'Rejected',      icon: 'cancel',        cls: 'verification-status--suspended' },
};

/* ── Controller ── */
const ProvidersController = {
  _items:       [],
  _categories:  [],
  _currentPage: 1,
  _perPage:     10,
  _totalCount:  0,
  _totalPages:  1,
  _search:      '',
  _categoryId:  '',
  _status:      '',

  async init() {
    AuthService.requireAuth();
    await this._loadCategories();
    this._bindSearch();
    this._bindFilters();
    this._bindAddProvider();
    await this._load();
    console.log('[ProvidersController] ready');
  },

  /* ── Categories (for the filter dropdown) ── */
  async _loadCategories() {
    try {
      const res = await CategoriesService.getAll();
      this._categories = Array.isArray(res) ? res : (res?.data ?? []);
    } catch (err) {
      console.error('[ProvidersController] loadCategories', err);
      this._categories = [];
    }

    const select = document.getElementById('admin-filter-category');
    if (!select) return;
    select.innerHTML = '<option value="">All Categories</option>' +
      this._categories.map(c => `<option value="${c.id}">${c.name}</option>`).join('');
  },

  /* ── Load stats + paginated providers from the API ── */
  async _load() {
    const tbody = document.getElementById('admin-providers-tbody');
    if (tbody) {
      tbody.innerHTML = `<tr><td colspan="6" style="text-align:center;padding:3rem;color:var(--color-on-surface-variant)">Loading…</td></tr>`;
    }

    try {
      const [stats, list] = await Promise.all([
        ProvidersService.getStats(),
        ProvidersService.getAll({
          page:       this._currentPage,
          perPage:    this._perPage,
          search:     this._search || undefined,
          categoryId: this._categoryId || undefined,
          status:     this._status ? STATUS_TO_API[this._status] : undefined,
        }),
      ]);

      this._applyStats(stats);

      this._items      = Array.isArray(list?.items) ? list.items : (Array.isArray(list) ? list : []);
      this._totalCount = list?.totalCount ?? this._items.length;
      this._totalPages = list?.totalPages ?? Math.max(1, Math.ceil(this._totalCount / this._perPage));

      this._renderTable();
      this._renderPagination();
    } catch (err) {
      console.error('[ProvidersController] load', err);
      showToast(err.message || 'Could not load providers', 'error');
      this._items = [];
      this._totalCount = 0;
      this._totalPages = 1;
      this._renderTable();
      this._renderPagination();
    }
  },

  _applyStats(stats) {
    const s = stats ?? {};
    this._setText('admin-stat-total',     (s.total     ?? 0).toLocaleString());
    this._setText('admin-stat-verified',  (s.verified  ?? 0).toLocaleString());
    this._setText('admin-stat-pending',   (s.pending   ?? 0).toLocaleString());
    this._setText('admin-stat-suspended', (s.suspended ?? 0).toLocaleString());
  },

  _setText(id, val) {
    const el = document.getElementById(id);
    if (el) el.textContent = val;
  },

  _statusKey(apiStatus) { return STATUS_FROM_API[apiStatus] ?? 'pending'; },

  /* ── Render Table ── */
  _renderTable() {
    const tbody = document.getElementById('admin-providers-tbody');
    if (!tbody) return;

    if (!this._items.length) {
      tbody.innerHTML = `<tr><td colspan="6" style="text-align:center;padding:3rem;color:var(--color-on-surface-variant)">No providers found.</td></tr>`;
      return;
    }

    tbody.innerHTML = this._items.map(p => {
      const statusKey   = this._statusKey(p.status);
      const st          = STATUS_CONFIG[statusKey] ?? STATUS_CONFIG.pending;
      const label       = p.categoryName ?? '—';
      const isSuspended = statusKey === 'suspended';
      const typeLabel   = p.providerType === 'Company' ? 'Company' : 'Individual';
      const typeIcon    = p.providerType === 'Company' ? 'business' : 'person';
      const typeBg      = p.providerType === 'Company'
        ? 'background:rgba(111,89,0,0.1);color:#6f5900'
        : 'background:rgba(0,80,212,0.1);color:var(--color-primary)';
      const avatar      = `https://ui-avatars.com/api/?name=${encodeURIComponent(p.name)}&background=0050d4&color=fff`;
      const experience  = p.yearsOfExperience != null ? `${p.yearsOfExperience} yrs` : '—';

      return `
        <tr>
          <td>
            <div class="provider-identity">
              <img class="provider-identity__avatar ${isSuspended ? 'provider-identity__avatar--suspended' : ''}"
                   src="${avatar}" alt="${p.name}" width="48" height="48"/>
              <div>
                <p class="provider-identity__name ${isSuspended ? 'provider-identity__name--suspended' : ''}">${p.name}</p>
                <p class="provider-identity__id">${p.id}${p.email ? ' · ' + p.email : ''}</p>
              </div>
            </div>
          </td>
          <td>
            <span class="category-badge">${label}</span>
          </td>
          <td style="text-align:center">
            <span style="display:inline-flex;align-items:center;gap:0.3rem;padding:0.3rem 0.7rem;border-radius:9999px;font-size:0.75rem;font-weight:700;${typeBg}">
              <span class="material-symbols-outlined" style="font-size:0.9rem">${typeIcon}</span>
              ${typeLabel}
            </span>
          </td>
          <td style="text-align:center;font-weight:600">${experience}</td>
          <td>
            <span class="verification-status ${st.cls}">
              <span class="material-symbols-outlined" style="font-size:1rem;font-variation-settings:'FILL' 1">${st.icon}</span>
              ${st.label}
            </span>
          </td>
          <td>
            <div class="row-actions">
              <button class="row-action-btn" data-action="view" data-id="${p.id}" aria-label="View ${p.name}">
                <span class="material-symbols-outlined" style="font-size:1.1rem">visibility</span>
              </button>
              ${statusKey === 'pending' || statusKey === 'review' ? `
                <button class="row-action-btn row-action-btn--success" data-action="approve" data-id="${p.id}" aria-label="Approve ${p.name}">
                  <span class="material-symbols-outlined" style="font-size:1.1rem">check_circle</span>
                </button>
              ` : ''}
              ${statusKey !== 'suspended' ? `
                <button class="row-action-btn row-action-btn--danger" data-action="suspend" data-id="${p.id}" aria-label="Suspend ${p.name}">
                  <span class="material-symbols-outlined" style="font-size:1.1rem">block</span>
                </button>
              ` : `
                <button class="row-action-btn row-action-btn--success" data-action="reinstate" data-id="${p.id}" aria-label="Reinstate ${p.name}">
                  <span class="material-symbols-outlined" style="font-size:1.1rem">restore</span>
                </button>
              `}
            </div>
          </td>
        </tr>
      `;
    }).join('');

    this._bindRowActions();
  },

  /* ── Pagination (server-driven) ── */
  _renderPagination() {
    const info     = document.getElementById('admin-pagination-info');
    const controls = document.getElementById('admin-pagination-controls');
    if (!info || !controls) return;

    const total = this._totalCount;
    const pages = Math.max(1, this._totalPages);
    const start = total === 0 ? 0 : Math.min((this._currentPage - 1) * this._perPage + 1, total);
    const end   = Math.min(this._currentPage * this._perPage, total);

    info.innerHTML = `Showing <strong>${start}–${end}</strong> of <strong>${total}</strong> providers`;

    const mkBtn = (label, pg, active = false, disabled = false, icon = null) => {
      const b = document.createElement('button');
      b.className = 'pagination-btn' + (active ? ' is-active' : '');
      b.disabled  = disabled;
      b.dataset.pg = pg;
      b.innerHTML = icon ? `<span class="material-symbols-outlined" style="font-size:1rem">${icon}</span>` : label;
      return b;
    };

    controls.innerHTML = '';
    controls.appendChild(mkBtn('', this._currentPage - 1, false, this._currentPage === 1, 'chevron_left'));
    this._getPageNums(this._currentPage, pages).forEach(n => {
      if (n === '…') { const s = document.createElement('span'); s.className='pagination-ellipsis'; s.textContent='…'; controls.appendChild(s); }
      else controls.appendChild(mkBtn(n, n, n === this._currentPage));
    });
    controls.appendChild(mkBtn('', this._currentPage + 1, false, this._currentPage === pages, 'chevron_right'));

    controls.querySelectorAll('[data-pg]').forEach(btn => {
      btn.addEventListener('click', () => {
        const pg = parseInt(btn.dataset.pg, 10);
        if (!Number.isNaN(pg) && pg >= 1 && pg <= pages && pg !== this._currentPage) {
          this._currentPage = pg;
          this._load();
        }
      });
    });
  },

  _getPageNums(cur, total) {
    if (total <= 7) return Array.from({ length: total }, (_, i) => i + 1);
    if (cur <= 4)            return [1, 2, 3, 4, 5, '…', total];
    if (cur >= total - 3)   return [1, '…', total-4, total-3, total-2, total-1, total];
    return [1, '…', cur-1, cur, cur+1, '…', total];
  },

  /* ── Row Actions ── */
  _bindRowActions() {
    document.querySelectorAll('#admin-providers-tbody [data-action]').forEach(btn => {
      btn.addEventListener('click', () => {
        const { action, id } = btn.dataset;
        const provider = this._items.find(p => String(p.id) === String(id));
        if (!provider) return;

        if (action === 'view')      this._openViewModal(provider);
        if (action === 'approve')   this._confirmApprove(provider);
        if (action === 'suspend')   this._confirmSuspend(provider);
        if (action === 'reinstate') this._confirmReinstate(provider);
      });
    });
  },

  _openViewModal(provider) {
    const statusKey = this._statusKey(provider.status);
    const st        = STATUS_CONFIG[statusKey] ?? STATUS_CONFIG.pending;
    const typeLabel = provider.providerType === 'Company' ? 'Company' : 'Individual';
    const typeIcon  = provider.providerType === 'Company' ? 'business' : 'person';
    const joined    = provider.createdAt
      ? new Date(provider.createdAt).toLocaleDateString('en-EG', { day: 'numeric', month: 'long', year: 'numeric' })
      : '—';

    const reasonSection = provider.suspendReason ? `
      <div style="margin-top:var(--sp-4);padding:var(--sp-4);background:rgba(186,26,26,0.06);border-radius:var(--radius-md);border:1px solid rgba(186,26,26,0.15)">
        <p style="font-size:0.75rem;font-weight:700;text-transform:uppercase;letter-spacing:0.05em;color:#ba1a1a;margin-bottom:var(--sp-2)">Suspend Reason</p>
        <p style="font-size:0.85rem;line-height:1.6;color:var(--color-on-surface-variant)">${provider.suspendReason}</p>
      </div>` : '';

    const cardHtml = `
      <div style="font-family:var(--font-body)">
        <div style="display:flex;align-items:center;gap:var(--sp-4);padding-bottom:var(--sp-5);border-bottom:1px solid var(--color-surface-container)">
          <img src="https://ui-avatars.com/api/?name=${encodeURIComponent(provider.name)}&background=0050d4&color=fff&size=80"
               style="width:4rem;height:4rem;border-radius:50%;object-fit:cover;border:2px solid var(--color-outline-variant)"
               alt="${provider.name}" />
          <div style="flex:1;min-width:0">
            <h4 style="font-size:1.05rem;font-weight:800;color:var(--color-on-surface);margin:0 0 0.25rem">${provider.name}</h4>
            <div style="display:flex;align-items:center;flex-wrap:wrap;gap:0.4rem">
              <span style="font-size:0.78rem;color:var(--color-on-surface-variant)">${provider.id}</span>
              <span style="display:inline-flex;align-items:center;gap:0.25rem;padding:0.2rem 0.6rem;border-radius:9999px;font-size:0.72rem;font-weight:700;background:rgba(0,0,0,0.04);color:var(--color-on-surface)">
                <span class="material-symbols-outlined" style="font-size:0.85rem;font-variation-settings:'FILL' 1">${st.icon}</span>${st.label}
              </span>
              <span style="display:inline-flex;align-items:center;gap:0.2rem;padding:0.2rem 0.6rem;border-radius:9999px;font-size:0.72rem;font-weight:700;background:rgba(0,80,212,0.08);color:var(--color-primary)">
                <span class="material-symbols-outlined" style="font-size:0.85rem">${typeIcon}</span>${typeLabel}
              </span>
            </div>
          </div>
        </div>

        <div style="display:grid;grid-template-columns:1fr 1fr;gap:var(--sp-3);margin-top:var(--sp-5)">
          <div style="padding:var(--sp-3);background:var(--color-surface-container-low);border-radius:var(--radius-md)">
            <p style="font-size:0.72rem;font-weight:700;text-transform:uppercase;letter-spacing:0.05em;color:var(--color-on-surface-variant);margin-bottom:0.25rem">Category</p>
            <p style="font-size:0.9rem;font-weight:700;color:var(--color-on-surface)">${provider.categoryName ?? '—'}</p>
          </div>
          <div style="padding:var(--sp-3);background:var(--color-surface-container-low);border-radius:var(--radius-md)">
            <p style="font-size:0.72rem;font-weight:700;text-transform:uppercase;letter-spacing:0.05em;color:var(--color-on-surface-variant);margin-bottom:0.25rem">Experience</p>
            <p style="font-size:0.9rem;font-weight:700;color:var(--color-on-surface)">${provider.yearsOfExperience != null ? provider.yearsOfExperience + ' yrs' : '—'}</p>
          </div>
          <div style="padding:var(--sp-3);background:var(--color-surface-container-low);border-radius:var(--radius-md);grid-column:1/-1">
            <p style="font-size:0.72rem;font-weight:700;text-transform:uppercase;letter-spacing:0.05em;color:var(--color-on-surface-variant);margin-bottom:0.25rem">Email</p>
            <p style="font-size:0.9rem;font-weight:700;color:var(--color-on-surface)">${provider.email ?? '—'}</p>
          </div>
          <div style="padding:var(--sp-3);background:var(--color-surface-container-low);border-radius:var(--radius-md);grid-column:1/-1">
            <p style="font-size:0.72rem;font-weight:700;text-transform:uppercase;letter-spacing:0.05em;color:var(--color-on-surface-variant);margin-bottom:0.25rem">Joined</p>
            <p style="font-size:0.85rem;color:var(--color-on-surface-variant)">${joined}</p>
          </div>
        </div>
        ${reasonSection}
      </div>`;

    Modal.open({
      title: 'Provider Profile',
      content: cardHtml,
      confirmLabel: 'Close',
      onConfirm: () => {},
    });
  },

  _confirmApprove(provider) {
    Modal.open({
      title: 'Approve Provider',
      content: `<p>Approve <strong>${provider.name}</strong> as a verified provider?</p>`,
      confirmLabel: 'Approve',
      onConfirm: async () => {
        try {
          await ProvidersService.approve(provider.id);
          showToast(`${provider.name} approved successfully.`, 'success');
          await this._load();
        } catch (err) {
          console.error('[ProvidersController] approve', err);
          showToast(err.message || 'Could not approve provider', 'error');
        }
      },
    });
  },

  _confirmSuspend(provider) {
    Modal.open({
      title: 'Suspend Provider',
      content: `
        <p>Suspend <strong>${provider.name}</strong>? They won't appear in search results.</p>
        <textarea class="input-field" id="modal-suspend-reason" rows="3" placeholder="Reason (optional)" style="width:100%;margin-top:var(--sp-3);resize:vertical"></textarea>
      `,
      confirmLabel: 'Suspend',
      onConfirm: async () => {
        const reason = document.getElementById('modal-suspend-reason')?.value.trim() || '';
        try {
          await ProvidersService.suspend(provider.id, reason);
          showToast(`${provider.name} suspended.`, 'error');
          await this._load();
        } catch (err) {
          console.error('[ProvidersController] suspend', err);
          showToast(err.message || 'Could not suspend provider', 'error');
        }
      },
    });
  },

  _confirmReinstate(provider) {
    Modal.open({
      title: 'Reinstate Provider',
      content: `<p>Reinstate <strong>${provider.name}</strong>? They'll become visible in search results again.</p>`,
      confirmLabel: 'Reinstate',
      onConfirm: async () => {
        try {
          await ProvidersService.reinstate(provider.id);
          showToast(`${provider.name} reinstated.`, 'success');
          await this._load();
        } catch (err) {
          console.error('[ProvidersController] reinstate', err);
          showToast(err.message || 'Could not reinstate provider', 'error');
        }
      },
    });
  },

  /* ── Search ── */
  _bindSearch() {
    const input = document.getElementById('admin-provider-search');
    if (!input) return;
    input.addEventListener('input', debounce(() => {
      this._search = input.value.trim();
      this._currentPage = 1;
      this._load();
    }, 300));
  },

  /* ── Filters ──
   * Category + Status are real API query params. Experience has no backend
   * filter (the API doesn't expose a years-of-experience range param), so
   * that select stays visual-only until the endpoint supports it. */
  _bindFilters() {
    document.getElementById('admin-filter-category')?.addEventListener('change', (e) => {
      this._categoryId = e.target.value;
      this._currentPage = 1;
      this._load();
    });
    document.getElementById('admin-filter-status')?.addEventListener('change', (e) => {
      this._status = e.target.value;
      this._currentPage = 1;
      this._load();
    });
    document.getElementById('admin-reset-filters-btn')?.addEventListener('click', () => {
      ['admin-filter-category', 'admin-filter-status', 'admin-filter-experience'].forEach(id => {
        const el = document.getElementById(id);
        if (el) el.value = '';
      });
      const s = document.getElementById('admin-provider-search');
      if (s) s.value = '';
      this._search = '';
      this._categoryId = '';
      this._status = '';
      this._currentPage = 1;
      this._load();
    });
  },

  /* ── Add Provider ──
   * There is no admin-facing "create provider" endpoint — providers are
   * created through their own onboarding flow (CreateProviderProfile),
   * which needs a provider user account. Rather than fabricate a fake
   * record, tell the admin how new providers actually show up here. */
  _bindAddProvider() {
    document.getElementById('admin-add-provider-btn')?.addEventListener('click', () => {
      Modal.open({
        title: 'Add New Provider',
        content: `<p>Providers can't be created directly from the admin panel. They register themselves through the provider onboarding flow, and will appear here — as <strong>Pending</strong> — once they submit their profile.</p>`,
        confirmLabel: 'Got it',
        onConfirm: () => {},
      });
    });
  },
};

export default ProvidersController;
