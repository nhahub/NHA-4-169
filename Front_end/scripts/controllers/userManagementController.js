/**
 * BAYTACK ADMIN — User Management Controller
 * Shows all platform users via the real backend (reuses UsersService / the same
 * Controllers/Admin/UsersController.cs the Users page already talks to).
 *
 * NOTE on tabs: "Customers" / "Providers" filter by the `role` query param
 * (Identity role name). If your database doesn't tag users with "Customer"/
 * "Provider" Identity roles (this system distinguishes providers mainly by the
 * presence of a ProviderProfile row, not an Identity role), those two tabs
 * will legitimately come back empty rather than fabricating a distinction
 * the backend doesn't expose yet.
 *
 * NOTE on Reinstate: there's no "reactivate" endpoint on UsersController yet
 * (only deactivate/delete) - the Reinstate button is left in the UI but shows
 * a toast explaining this instead of silently doing nothing.
 */
import AuthService from '../services/authService.js';
import UsersService from '../services/usersService.js';
import Modal from '../components/modal.js';
import { showToast } from '../core/helpers.js';

const UserManagementController = {
  _currentTab:  'all',
  _currentPage: 1,
  _pageSize:    10,
  _searchQuery: '',
  _searchDebounce: null,
  _totalCount:  0,
  _totalPages:  1,
  _rows:        [],

  async init() {
    AuthService.requireAuth();
    this._bindTabs();
    this._bindSearch();
    this._bindPagination();
    await this._loadStats();
    await this._loadPage();
    console.log('[UserManagementController] ready');
  },

  _roleForTab(tab) {
    return tab === 'customers' ? 'Customer' : tab === 'providers' ? 'Provider' : undefined;
  },

  async _loadStats() {
    try {
      const [all, customers, providers] = await Promise.all([
        UsersService.getAll({ page: 1, perPage: 1 }),
        UsersService.getAll({ page: 1, perPage: 1, role: 'Customer' }),
        UsersService.getAll({ page: 1, perPage: 1, role: 'Provider' }),
      ]);
      this._setStat('um-stat-total', all.totalCount);
      this._setStat('um-stat-customers', customers.totalCount);
      this._setStat('um-stat-providers', providers.totalCount);
      // No global "suspended count" endpoint yet - approximate from the full first page below in _loadPage().
    } catch (err) {
      console.warn('[UserManagementController] failed to load stats', err);
    }
  },

  _setStat(id, value) {
    const el = document.getElementById(id);
    if (el) el.textContent = value.toLocaleString('en-US');
  },

  _bindTabs() {
    document.querySelectorAll('.um-tab').forEach(tab => {
      tab.addEventListener('click', () => {
        document.querySelectorAll('.um-tab').forEach(t => {
          t.classList.remove('um-tab--active');
          t.setAttribute('aria-selected', 'false');
        });
        tab.classList.add('um-tab--active');
        tab.setAttribute('aria-selected', 'true');
        this._currentTab = tab.dataset.tab;
        this._currentPage = 1;
        this._loadPage();
      });
    });
  },

  _bindSearch() {
    const input = document.getElementById('um-search');
    if (!input) return;
    input.addEventListener('input', () => {
      clearTimeout(this._searchDebounce);
      this._searchDebounce = setTimeout(() => {
        this._searchQuery = input.value.trim();
        this._currentPage = 1;
        this._loadPage();
      }, 300);
    });
  },

  _bindPagination() {
    const controls = document.getElementById('um-pagination-controls');
    if (!controls) return;
    controls.addEventListener('click', (e) => {
      const btn = e.target.closest('.pagination-btn');
      if (!btn || btn.disabled) return;
      const page = parseInt(btn.dataset.page, 10);
      if (!isNaN(page)) {
        this._currentPage = page;
        this._loadPage();
      }
    });
  },

  async _loadPage() {
    const tbody = document.getElementById('um-tbody');
    try {
      const result = await UsersService.getAll({
        search: this._searchQuery,
        role: this._roleForTab(this._currentTab),
        page: this._currentPage,
        perPage: this._pageSize,
      });
      this._rows = result.items;
      this._totalCount = result.totalCount;
      this._totalPages = Math.max(1, result.totalPages);
      this._renderTable();

      if (this._currentTab === 'all' && this._currentPage === 1) {
        const suspended = result.items.filter(u => u.status === 'Suspended').length;
        this._setStat('um-stat-suspended', suspended);
      }
    } catch (err) {
      console.warn('[UserManagementController] failed to load users', err);
      showToast('Could not load users', 'error');
      if (tbody) tbody.innerHTML = `<tr><td colspan="6" style="text-align:center;padding:2rem;color:var(--color-on-surface-variant)">Could not load users.</td></tr>`;
    }
    this._renderPaginationInfo();
    this._renderPaginationControls();
  },

  _renderTable() {
    const tbody = document.getElementById('um-tbody');
    if (!tbody) return;

    if (!this._rows.length) {
      tbody.innerHTML = `<tr><td colspan="6" style="text-align:center;padding:2rem;color:var(--color-on-surface-variant)">No users found.</td></tr>`;
      return;
    }

    const STATUS_BADGE = {
      Active:      { cls: 'status-badge--active',    label: 'Active'      },
      Suspended:   { cls: 'status-badge--suspended', label: 'Suspended'   },
      Deactivated: { cls: 'status-badge--suspended', label: 'Deactivated' },
    };

    tbody.innerHTML = this._rows.map(u => {
      const isProvider = (u.roles || []).some(r => r.toLowerCase() === 'provider');
      const typeLabel   = isProvider ? 'Provider' : 'Customer';
      const typeCls     = isProvider ? 'um-badge--provider' : 'um-badge--customer';
      const icon        = isProvider ? 'engineering' : 'person';
      const iconStyle    = isProvider
        ? 'background:rgba(0,105,71,0.10);color:var(--color-secondary)'
        : 'background:rgba(0,80,212,0.10);color:var(--color-primary)';
      const st = STATUS_BADGE[u.status] ?? { cls: 'status-badge--active', label: u.status };
      const joined = u.createdAt ? new Date(u.createdAt).toLocaleDateString('en-US', { day:'2-digit', month:'short', year:'numeric' }) : '—';

      return `
        <tr data-id="${u.id}">
          <td>
            <div class="provider-cell">
              <div class="provider-cell__avatar" style="${iconStyle}">
                <span class="material-symbols-outlined" style="font-variation-settings:'FILL' 1">${icon}</span>
              </div>
              <div>
                <p class="provider-cell__name">${u.fullName}</p>
                <p class="provider-cell__meta">ID: #${u.id.slice(0, 8)}</p>
              </div>
            </div>
          </td>
          <td><span class="um-badge ${typeCls}">${typeLabel}</span></td>
          <td><span class="provider-cell__meta">${u.email || '—'}</span></td>
          <td><span class="provider-cell__meta">${joined}</span></td>
          <td><span class="status-badge ${st.cls}">${st.label}</span></td>
          <td style="text-align:right">
            <div class="action-group">
              <button class="btn btn--icon" title="View Profile" data-action="view" data-id="${u.id}"><span class="material-symbols-outlined">visibility</span></button>
              ${u.status === 'Suspended' || u.status === 'Deactivated'
                ? `<button class="btn btn--icon" title="Reinstate" style="color:var(--color-secondary)" data-action="reinstate" data-id="${u.id}"><span class="material-symbols-outlined">check_circle</span></button>`
                : `<button class="btn btn--icon btn--icon-danger" title="Suspend" data-action="suspend" data-id="${u.id}"><span class="material-symbols-outlined">block</span></button>`}
            </div>
          </td>
        </tr>
      `;
    }).join('');

    this._bindRowActions();
  },

  _bindRowActions() {
    document.querySelectorAll('#um-tbody [data-action]').forEach(btn => {
      btn.addEventListener('click', () => {
        const { action, id } = btn.dataset;
        const user = this._rows.find(u => u.id === id);
        if (!user) return;

        if (action === 'view') {
          showToast(`${user.fullName} \u2014 ${user.email || 'no email'} \u2014 ${user.status}`, 'info');
        }

        if (action === 'suspend') {
          Modal.open({
            title: 'Suspend User',
            content: `<p>Suspend <strong>${user.fullName}</strong>? They won't be able to sign in until reinstated.</p>`,
            confirmLabel: 'Suspend',
            onConfirm: async () => {
              try {
                await UsersService.deactivate(id);
                showToast(`${user.fullName} has been suspended.`, 'success');
                await this._loadPage();
              } catch (err) {
                console.warn('[UserManagementController] suspend failed', err);
                showToast('Could not suspend this user', 'error');
              }
            },
          });
        }

        if (action === 'reinstate') {
          showToast('Reinstating a suspended user isn\u2019t supported by the backend yet.', 'info');
        }
      });
    });
  },

  _renderPaginationInfo() {
    const infoEl = document.getElementById('um-pagination-info');
    if (!infoEl) return;
    if (this._totalCount === 0) { infoEl.textContent = 'No users'; return; }
    const start = (this._currentPage - 1) * this._pageSize + 1;
    const end = Math.min(this._currentPage * this._pageSize, this._totalCount);
    infoEl.textContent = `Showing ${start}\u2013${end} of ${this._totalCount} users`;
  },

  _renderPaginationControls() {
    const controls = document.getElementById('um-pagination-controls');
    if (!controls) return;
    controls.innerHTML = '';

    const mkBtn = (label, page, isActive = false, isDisabled = false, icon = null) => {
      const btn = document.createElement('button');
      btn.className = 'pagination-btn' + (isActive ? ' pagination-btn--active' : '');
      btn.disabled  = isDisabled;
      if (page !== null) btn.dataset.page = page;
      btn.innerHTML = icon ? `<span class="material-symbols-outlined">${icon}</span>` : label;
      return btn;
    };

    controls.appendChild(mkBtn('', this._currentPage - 1, false, this._currentPage === 1, 'chevron_left'));
    this._getPageNumbers(this._currentPage, this._totalPages).forEach(p => {
      if (p === '...') {
        const el = document.createElement('span');
        el.className = 'pagination-ellipsis'; el.textContent = '\u2026';
        controls.appendChild(el);
      } else {
        controls.appendChild(mkBtn(p, p, p === this._currentPage));
      }
    });
    controls.appendChild(mkBtn('', this._currentPage + 1, false, this._currentPage === this._totalPages, 'chevron_right'));
  },

  _getPageNumbers(current, total) {
    if (total <= 7) return Array.from({ length: total }, (_, i) => i + 1);
    if (current <= 4) return [1, 2, 3, 4, 5, '...', total];
    if (current >= total - 3) return [1, '...', total - 4, total - 3, total - 2, total - 1, total];
    return [1, '...', current - 1, current, current + 1, '...', total];
  },
};

export default UserManagementController;
