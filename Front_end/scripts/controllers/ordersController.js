/**
 * BAYTACK ADMIN — Orders Controller
 * Handles the Orders & Payments page, backed by the real backend
 * (Controllers/Admin/OrdersController.cs -> GetAllOrdersAdminQuery).
 *
 * NOTE: the "Payment Method" filter select has no backing field in the
 * Order/Payment domain model yet (Payment.MethodId isn't surfaced by this
 * endpoint) - it's left in the UI but currently has no effect. Flagging
 * here rather than silently ignoring it.
 */
import AuthService  from '../services/authService.js';
import OrdersService from '../services/ordersService.js';
import { formatCurrency, showToast } from '../core/helpers.js';

const STATUS_BADGE = {
  Completed:  { cls: 'orders-status-badge--paid',     label: 'Completed'  },
  Pending:    { cls: 'orders-status-badge--pending',  label: 'Pending'    },
  Confirmed:  { cls: 'orders-status-badge--pending',  label: 'Confirmed'  },
  InProgress: { cls: 'orders-status-badge--pending',  label: 'In Progress'},
  Cancelled:  { cls: 'orders-status-badge--refunded', label: 'Cancelled'  },
  Disputed:   { cls: 'orders-status-badge--refunded', label: 'Disputed'   },
};

const OrdersController = {
  _currentPage: 1,
  _pageSize: 10,
  _totalCount: 0,
  _totalPages: 1,
  _currentStatus: '',
  _searchQuery: '',
  _searchDebounce: null,

  async init() {
    AuthService.requireAuth();
    this._bindSearch();
    this._bindTimeTabs();
    this._bindFilter();
    this._bindPagination();
    await this._loadPage();
    console.log('[OrdersController] ready');
  },

  _bindSearch() {
    const input = document.getElementById('orders-search');
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

  _bindTimeTabs() {
    // Purely visual (no backing date-range filter on the backend yet).
    document.querySelectorAll('.orders-time-tab').forEach(tab => {
      tab.addEventListener('click', () => {
        document.querySelectorAll('.orders-time-tab').forEach(t => t.classList.remove('orders-time-tab--active'));
        tab.classList.add('orders-time-tab--active');
      });
    });
  },

  _bindFilter() {
    const btn = document.getElementById('orders-filter-btn');
    if (!btn) return;
    btn.addEventListener('click', () => {
      this._currentStatus = document.getElementById('orders-filter-status')?.value || '';
      this._currentPage = 1;
      this._loadPage();
    });
  },

  _bindPagination() {
    const controls = document.getElementById('orders-pagination-controls');
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
    const tbody = document.getElementById('orders-tbody');
    try {
      const result = await OrdersService.getAll({
        search: this._searchQuery,
        status: this._currentStatus,
        page: this._currentPage,
        limit: this._pageSize,
      });
      this._totalCount = result.totalCount;
      this._totalPages = Math.max(1, result.totalPages);
      this._renderRows(result.items);
    } catch (err) {
      console.warn('[OrdersController] failed to load orders', err);
      showToast('Could not load orders', 'error');
      if (tbody) tbody.innerHTML = `<tr><td colspan="8" style="text-align:center;padding:2rem;color:var(--color-on-surface-variant)">Could not load orders.</td></tr>`;
    }
    this._renderPaginationInfo();
    this._renderPaginationControls();
  },

  _renderRows(rows) {
    const tbody = document.getElementById('orders-tbody');
    if (!tbody) return;

    if (!rows || !rows.length) {
      tbody.innerHTML = `<tr><td colspan="8" style="text-align:center;padding:2rem;color:var(--color-on-surface-variant)">No orders found.</td></tr>`;
      return;
    }

    tbody.innerHTML = rows.map(o => {
      const badge = STATUS_BADGE[o.status] ?? { cls: 'orders-status-badge--pending', label: o.status };
      return `
        <tr>
          <td><span class="orders-order-id">#${o.id.slice(0, 8)}</span></td>
          <td>
            <div class="orders-job-cell">
              <div class="orders-job-icon" style="background:rgba(0,80,212,0.09);color:var(--color-primary)">
                <span class="material-symbols-outlined">handyman</span>
              </div>
              ${o.jobTitle}
            </div>
          </td>
          <td class="orders-person">${o.providerName}</td>
          <td class="orders-person orders-person--dim">${o.customerName}</td>
          <td class="orders-amount">${formatCurrency(o.finalPrice)}</td>
          <td class="orders-commission">${o.commission != null ? formatCurrency(o.commission) : '—'}</td>
          <td class="orders-provider-got">${o.providerReceived != null ? formatCurrency(o.providerReceived) : '—'}</td>
          <td style="text-align:center">
            <span class="orders-status-badge ${badge.cls}">${badge.label}</span>
          </td>
        </tr>
      `;
    }).join('');
  },

  _renderPaginationInfo() {
    const infoEl = document.getElementById('orders-pagination-info');
    if (!infoEl) return;
    if (this._totalCount === 0) {
      infoEl.textContent = 'No orders';
      return;
    }
    const start = (this._currentPage - 1) * this._pageSize + 1;
    const end = Math.min(this._currentPage * this._pageSize, this._totalCount);
    infoEl.textContent = `Showing ${start}\u2013${end} of ${this._totalCount} orders`;
  },

  _renderPaginationControls() {
    const controls = document.getElementById('orders-pagination-controls');
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

export default OrdersController;
