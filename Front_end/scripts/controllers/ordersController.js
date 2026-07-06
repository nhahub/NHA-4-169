/**
 * BAYTACK ADMIN — Orders Controller
 * Handles the Orders & Payments page with working pagination.
 */
import AuthService from '../services/authService.js';

const OrdersController = {
  _currentPage: 1,
  _rowsPerPage: 10,
  _allRows: [],
  _currentStatus: '',
  _searchQuery: '',

  async init() {
    AuthService.requireAuth();
    this._allRows = Array.from(document.querySelectorAll('#orders-tbody tr'));
    this._bindSearch();
    this._bindTimeTabs();
    this._bindFilter();
    this._bindPagination();
    this._renderPage();
    console.log('[OrdersController] ready');
  },

  _bindSearch() {
    const input = document.getElementById('orders-search');
    if (!input) return;
    input.addEventListener('input', () => {
      this._searchQuery  = input.value.trim().toLowerCase();
      this._currentPage  = 1;
      this._renderPage();
    });
  },

  _bindTimeTabs() {
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
      this._currentStatus = document.getElementById('orders-filter-status')?.value.toLowerCase() || '';
      this._currentPage   = 1;
      this._renderPage();
    });
  },

  _bindPagination() {
    const controls = document.querySelector('.pagination-controls');
    if (!controls) return;
        controls.addEventListener('click', (e) => {
      const btn = e.target.closest('.pagination-btn');
      if (!btn || btn.disabled) return;
      const page = parseInt(btn.dataset.page);
      if (!isNaN(page)) {
        this._currentPage = page;
        this._renderPage();
      }
    });
  },

  _getVisibleRows() {
    return this._allRows.filter(row => {
      const matchesQuery  = !this._searchQuery  || row.textContent.toLowerCase().includes(this._searchQuery);
      const badge         = row.querySelector('.orders-status-badge');
      const matchesStatus = !this._currentStatus || (badge && badge.textContent.toLowerCase().includes(this._currentStatus));
      return matchesQuery && matchesStatus;
    });
  },

  _renderPage() {
    const visible = this._getVisibleRows();
    const total   = visible.length;
    const pages   = Math.max(1, Math.ceil(total / this._rowsPerPage));
    if (this._currentPage > pages) this._currentPage = pages;
    const start = (this._currentPage - 1) * this._rowsPerPage;
    const end   = start + this._rowsPerPage;

    this._allRows.forEach(row => row.style.display = 'none');
    visible.slice(start, end).forEach(row => row.style.display = '');

    // Update info
    const infoEl = document.querySelector('.table-pagination__info');
    if (infoEl) {
      const showing = Math.min(this._rowsPerPage, total - start);
      infoEl.textContent = `Showing ${start + 1}–${start + showing} of ${total} orders`;
    }

    // Rebuild buttons
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
    this._getPageNumbers(this._currentPage, pages).forEach(p => {
      if (p === '...') {
        const el = document.createElement('span');
        el.className = 'pagination-ellipsis'; el.textContent = '…';
        controls.appendChild(el);
      } else {
        controls.appendChild(mkBtn(p, p, p === this._currentPage));
      }
    });
    controls.appendChild(mkBtn('', this._currentPage + 1, false, this._currentPage === pages, 'chevron_right'));
  },

  _getPageNumbers(current, total) {
    if (total <= 7) return Array.from({ length: total }, (_, i) => i + 1);
    if (current <= 4) return [1, 2, 3, 4, 5, '...', total];
    if (current >= total - 3) return [1, '...', total - 4, total - 3, total - 2, total - 1, total];
    return [1, '...', current - 1, current, current + 1, '...', total];
  },
};

export default OrdersController;