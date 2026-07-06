/**
 * BAYTACK ADMIN — User Management Controller
 * Shows Customers + Providers. Providers loaded from localStorage (ek_admin_providers).
 */
import AuthService from '../services/authService.js';

const SERVICE_LABELS = {
  plumbing:'Plumbing', electrical:'Electrical', cleaning:'Cleaning',
  carpentry:'Carpentry', painting:'Painting', ac_repair:'AC Repair',
};

const UserManagementController = {
  _currentTab:  'all',
  _currentPage: 1,
  _rowsPerPage: 10,
  _allRows:     [],

  async init() {
    AuthService.requireAuth();

    // Inject real providers from localStorage before reading rows
    this._injectRealProviders();

    this._allRows = Array.from(document.querySelectorAll('#um-tbody tr'));
    this._bindTabs();
    this._bindSearch();
    this._bindPagination();
    this._bindActions();
    this._renderPage();
    console.log('[UserManagementController] ready — rows:', this._allRows.length);
  },

  /* ── Inject providers from localStorage into the table ── */
  _injectRealProviders() {
    const tbody = document.getElementById('um-tbody');
    if (!tbody) return;
    try {
      const stored = JSON.parse(localStorage.getItem('ek_admin_providers') || '[]');
      if (!stored.length) return;

      stored.forEach(p => {
        // Skip if already in table (by phone or id)
        const exists = Array.from(tbody.querySelectorAll('tr')).some(row =>
          row.textContent.includes(p.phone || '') || row.textContent.includes(p.id || '')
        );
        if (exists) return;

        const statusMap = {
          verified:     { cls: 'status-badge--verified',  label: 'Verified'  },
          pending:      { cls: 'status-badge--pending',   label: 'Pending'   },
          pending_review:{ cls:'status-badge--pending',   label: 'Pending'   },
          review:       { cls: 'status-badge--review',    label: 'Review'    },
          suspended:    { cls: 'status-badge--suspended', label: 'Suspended' },
        };
        const st  = statusMap[p.status] ?? statusMap.pending;
        const svc = SERVICE_LABELS[p.category || p.service] || p.category || p.service || '—';
        const typeLabel = p.providerType === 'company' ? '🏢 Co.' : '';
        const joined = p.submittedAt
          ? new Date(p.submittedAt).toLocaleDateString('en-EG',{day:'numeric',month:'short',year:'numeric'})
          : '—';

        const tr = document.createElement('tr');
        tr.dataset.providerId = p.id;
        tr.innerHTML = `
          <td>
            <div class="provider-cell">
              <div class="provider-cell__avatar" style="background:rgba(0,105,71,0.10);color:var(--color-secondary)">
                <span class="material-symbols-outlined" style="font-variation-settings:'FILL' 1">engineering</span>
              </div>
              <div>
                <p class="provider-cell__name">${p.name || '—'} ${typeLabel}</p>
                <p class="provider-cell__meta">ID: #${p.id}</p>
              </div>
            </div>
          </td>
          <td><span class="um-badge um-badge--provider">Provider</span></td>
          <td><span class="provider-cell__meta">${p.email || p.phone || '—'}<br/><small>${svc}</small></span></td>
          <td><span class="provider-cell__meta">${joined}</span></td>
          <td><span class="status-badge ${st.cls}">${st.label}</span></td>
          <td style="text-align:right">
            <div class="action-group">
              <button class="btn btn--icon um-action" data-action="view"    data-id="${p.id}" title="View Profile"><span class="material-symbols-outlined">visibility</span></button>
              <button class="btn btn--icon um-action" data-action="approve" data-id="${p.id}" title="Approve" style="color:var(--color-secondary)"><span class="material-symbols-outlined">check_circle</span></button>
              <button class="btn btn--icon btn--icon-danger um-action" data-action="suspend" data-id="${p.id}" title="Suspend"><span class="material-symbols-outlined">block</span></button>
            </div>
          </td>
        `;
        tbody.insertBefore(tr, tbody.firstChild);
      });
    } catch (e) {
      console.warn('[UM] Could not load providers from localStorage:', e);
    }
  },

  /* ── Bind action buttons ── */
  _bindActions() {
    document.getElementById('um-tbody')?.addEventListener('click', e => {
      const btn = e.target.closest('.um-action');
      if (!btn) return;
      const { action, id } = btn.dataset;
      const row = btn.closest('tr');
      const name = row?.querySelector('.provider-cell__name')?.textContent.trim() || 'this user';

      if (action === 'view') {
        // Navigate to verification-review with id
        try {
          const providers = JSON.parse(localStorage.getItem('ek_admin_providers') || '[]');
          const p = providers.find(x => x.id === id);
          if (p) sessionStorage.setItem('bt_review_provider', JSON.stringify(p));
        } catch {}
        window.location.href = 'verification-review.html';
      }

      if (action === 'approve') {
        if (!confirm(`Approve ${name}?`)) return;
        this._updateProviderStatus(id, 'verified');
        const badge = row?.querySelector('.status-badge');
        if (badge) { badge.className = 'status-badge status-badge--verified'; badge.textContent = 'Verified'; }
      }

      if (action === 'suspend') {
        if (!confirm(`Suspend ${name}?`)) return;
        this._updateProviderStatus(id, 'suspended');
        const badge = row?.querySelector('.status-badge');
        if (badge) { badge.className = 'status-badge status-badge--suspended'; badge.textContent = 'Suspended'; }
      }
    });
  },

  _updateProviderStatus(id, status) {
    try {
      const list = JSON.parse(localStorage.getItem('ek_admin_providers') || '[]');
      const idx  = list.findIndex(p => p.id === id);
      if (idx !== -1) { list[idx].status = status; localStorage.setItem('ek_admin_providers', JSON.stringify(list)); }
    } catch {}
  },

  /* ── Tabs ── */
  _bindTabs() {
    document.querySelectorAll('.um-tab').forEach(tab => {
      tab.addEventListener('click', () => {
        this._currentTab  = tab.dataset.tab;
        this._currentPage = 1;
        document.querySelectorAll('.um-tab').forEach(t => { t.classList.remove('um-tab--active'); t.setAttribute('aria-selected','false'); });
        tab.classList.add('um-tab--active');
        tab.setAttribute('aria-selected','true');
        this._renderPage();
      });
    });
  },

  /* ── Search ── */
  _bindSearch() {
    document.getElementById('um-search')?.addEventListener('input', () => {
      this._currentPage = 1;
      this._renderPage();
    });
  },

  /* ── Pagination ── */
  _bindPagination() {
    document.getElementById('um-pagination-controls')?.addEventListener('click', e => {
      const btn = e.target.closest('.pagination-btn');
      if (!btn || btn.disabled) return;
      const page = parseInt(btn.dataset.page);
      if (!isNaN(page)) { this._currentPage = page; this._renderPage(); }
    });
  },

  _getVisibleRows() {
    const query = document.getElementById('um-search')?.value.trim().toLowerCase() || '';
    return this._allRows.filter(row => {
      const typeCell = row.querySelector('.um-badge');
      const typeText = typeCell ? typeCell.textContent.toLowerCase() : '';
      const matchesTab =
        this._currentTab === 'all'
        || (this._currentTab === 'customers' && typeText.includes('customer'))
        || (this._currentTab === 'providers' && typeText.includes('provider'));
      const matchesQuery = !query || row.textContent.toLowerCase().includes(query);
      return matchesTab && matchesQuery;
    });
  },

  _renderPage() {
    const visible = this._getVisibleRows();
    const total   = visible.length;
    const pages   = Math.max(1, Math.ceil(total / this._rowsPerPage));
    if (this._currentPage > pages) this._currentPage = pages;
    const start = (this._currentPage - 1) * this._rowsPerPage;
    const end   = start + this._rowsPerPage;

    this._allRows.forEach(r => r.style.display = 'none');
    visible.slice(start, end).forEach(r => r.style.display = '');

    const infoEl = document.getElementById('um-pagination-info');
    if (infoEl) {
      const showing = Math.min(this._rowsPerPage, total - start);
      infoEl.textContent = total === 0 ? 'No users found' : `Showing ${start + 1}–${start + showing} of ${total} users`;
    }

    const controls = document.getElementById('um-pagination-controls');
    if (!controls) return;
    controls.innerHTML = '';

    const mkBtn = (label, pg, active=false, disabled=false, icon=null) => {
      const b = document.createElement('button');
      b.className = 'pagination-btn' + (active ? ' pagination-btn--active' : '');
      b.disabled  = disabled;
      if (pg !== null) b.dataset.page = pg;
      b.innerHTML = icon ? `<span class="material-symbols-outlined">${icon}</span>` : label;
      return b;
    };

    controls.appendChild(mkBtn('', this._currentPage-1, false, this._currentPage===1, 'chevron_left'));
    this._getPageNumbers(this._currentPage, pages).forEach(p => {
      if (p === '...') { const s=document.createElement('span'); s.className='pagination-ellipsis'; s.textContent='…'; controls.appendChild(s); }
      else controls.appendChild(mkBtn(p, p, p===this._currentPage));
    });
    controls.appendChild(mkBtn('', this._currentPage+1, false, this._currentPage===pages, 'chevron_right'));
  },

  _getPageNumbers(cur, total) {
    if (total <= 7) return Array.from({length:total},(_,i)=>i+1);
    if (cur <= 4)         return [1,2,3,4,5,'...',total];
    if (cur >= total-3)  return [1,'...',total-4,total-3,total-2,total-1,total];
    return [1,'...',cur-1,cur,cur+1,'...',total];
  },
};

export default UserManagementController;
