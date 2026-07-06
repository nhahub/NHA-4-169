/**
 * EKHDEMNI ADMIN — Providers Controller
 * Reads real registered providers from localStorage (ek_admin_providers)
 * merged with demo data, and adds provider Type column (Individual / Company).
 */

import AuthService from '../services/authService.js';
import Modal       from '../components/modal.js';
import { showToast, debounce } from '../core/helpers.js';

/* ── Stats ── */
const MOCK_STATS = { total: 1284, verified: 976, pending: 48, suspended: 12 };

/* ── Load providers: merge localStorage registrations with demo data ── */
function _resolveCategory(p) {
  // Normalize: some records use 'service', some use 'category'
  return p.category || p.service || '';
}

function _loadProviders() {
  const DEMO = [
    { id:'PRV-001', name:'Karim Naguib',   category:'plumbing',   providerType:'individual', experience:'5+ yrs',  status:'verified',  img:'https://ui-avatars.com/api/?name=Karim+Naguib&background=0050d4&color=fff' },
    { id:'PRV-002', name:'Fatma El-Sayed', category:'electrical', providerType:'company',    experience:'3–5 yrs', status:'pending',   img:'https://ui-avatars.com/api/?name=Fatma+ElSayed&background=6f5900&color=fff' },
    { id:'PRV-003', name:'Mostafa Khalil', category:'cleaning',   providerType:'individual', experience:'1–3 yrs', status:'review',    img:'https://ui-avatars.com/api/?name=Mostafa+Khalil&background=595c5e&color=fff' },
    { id:'PRV-004', name:'Noura Hassan',   category:'carpentry',  providerType:'individual', experience:'5+ yrs',  status:'suspended', img:'https://ui-avatars.com/api/?name=Noura+Hassan&background=b31b25&color=fff' },
  ];
  try {
    const stored = JSON.parse(localStorage.getItem('ek_admin_providers') || '[]');
    const storedIds = new Set(stored.map(p => p.id));
    return [...stored, ...DEMO.filter(p => !storedIds.has(p.id))];
  } catch { return DEMO; }
}

const STATUS_CONFIG = {
  verified:  { label: 'Verified',      icon: 'verified',      cls: 'verification-status--verified'  },
  pending:   { label: 'Pending',       icon: 'schedule',      cls: 'verification-status--pending'   },
  review:    { label: 'Under Review',  icon: 'manage_search', cls: 'verification-status--review'    },
  suspended: { label: 'Suspended',     icon: 'block',         cls: 'verification-status--suspended' },
};

const CATEGORY_CLASS = {
  plumbing:   'category-badge--plumbing',
  electrical: 'category-badge--electrical',
  cleaning:   'category-badge--cleaning',
  carpentry:  'category-badge--carpentry',
  painting:   'category-badge--painting',
  ac_repair:  'category-badge--ac',
};

const SERVICE_LABELS = {
  plumbing:   'Plumbing',
  electrical: 'Electrical',
  cleaning:   'Cleaning',
  carpentry:  'Carpentry',
  painting:   'Painting',
  ac_repair:  'AC Repair',
};

/* ── Controller ── */
const ProvidersController = {
  _allProviders: [],
  _filtered:     [],
  _currentPage:  1,
  _perPage:      10,

  async init() {
    AuthService.requireAuth();
    this._allProviders = _loadProviders();
    this._filtered     = [...this._allProviders];

    this._loadStats();
    this._renderTable();
    this._bindSearch();
    this._bindFilters();
    this._bindAddProvider();
  },

  /* ── Stats ── */
  _loadStats() {
    const total     = this._allProviders.length;
    const verified  = this._allProviders.filter(p => p.status === 'verified').length;
    const pending   = this._allProviders.filter(p => p.status === 'pending' || p.status === 'review').length;
    const suspended = this._allProviders.filter(p => p.status === 'suspended').length;

    this._setText('admin-stat-total',     (MOCK_STATS.total + total - 4).toLocaleString());
    this._setText('admin-stat-verified',  (MOCK_STATS.verified + verified - 1).toLocaleString());
    this._setText('admin-stat-pending',   (MOCK_STATS.pending + pending - 1).toLocaleString());
    this._setText('admin-stat-suspended', (MOCK_STATS.suspended + suspended).toLocaleString());
  },

  _setText(id, val) {
    const el = document.getElementById(id);
    if (el) el.textContent = val;
  },

  /* ── Render Table ── */
  _renderTable() {
    const tbody = document.getElementById('admin-providers-tbody');
    if (!tbody) return;

    const start = (this._currentPage - 1) * this._perPage;
    const page  = this._filtered.slice(start, start + this._perPage);

    if (page.length === 0) {
      tbody.innerHTML = `<tr><td colspan="6" style="text-align:center;padding:3rem;color:var(--color-on-surface-variant)">No providers found.</td></tr>`;
      this._renderPagination();
      return;
    }

    tbody.innerHTML = page.map(p => {
      const st    = STATUS_CONFIG[p.status]  ?? STATUS_CONFIG.pending;
      const cat   = CATEGORY_CLASS[_resolveCategory(p)] ?? '';
      const label = SERVICE_LABELS[_resolveCategory(p)] ?? (_resolveCategory(p) || '—');
      const isSuspended  = p.status === 'suspended';
      const typeLabel    = p.providerType === 'company' ? 'Company' : 'Individual';
      const typeIcon     = p.providerType === 'company' ? 'business' : 'person';
      const typeBg       = p.providerType === 'company'
        ? 'background:rgba(111,89,0,0.1);color:#6f5900'
        : 'background:rgba(0,80,212,0.1);color:var(--color-primary)';

      return `
        <tr>
          <td>
            <div class="provider-identity">
              <img class="provider-identity__avatar ${isSuspended ? 'provider-identity__avatar--suspended' : ''}"
                   src="${p.img}" alt="${p.name}" width="48" height="48"/>
              <div>
                <p class="provider-identity__name ${isSuspended ? 'provider-identity__name--suspended' : ''}">${p.name}</p>
                <p class="provider-identity__id">${p.id}${p.phone ? ' · ' + p.phone : ''}</p>
              </div>
            </div>
          </td>
          <td>
            <span class="category-badge ${cat}">${label}</span>
          </td>
          <td style="text-align:center">
            <span style="display:inline-flex;align-items:center;gap:0.3rem;padding:0.3rem 0.7rem;border-radius:9999px;font-size:0.75rem;font-weight:700;${typeBg}">
              <span class="material-symbols-outlined" style="font-size:0.9rem">${typeIcon}</span>
              ${typeLabel}
            </span>
          </td>
          <td style="text-align:center;font-weight:600">${p.experience ?? '—'}</td>
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
              ${p.status === 'pending' || p.status === 'review' ? `
                <button class="row-action-btn row-action-btn--success" data-action="approve" data-id="${p.id}" aria-label="Approve ${p.name}">
                  <span class="material-symbols-outlined" style="font-size:1.1rem">check_circle</span>
                </button>
              ` : ''}
              ${p.status !== 'suspended' ? `
                <button class="row-action-btn row-action-btn--danger" data-action="suspend" data-id="${p.id}" aria-label="Suspend ${p.name}">
                  <span class="material-symbols-outlined" style="font-size:1.1rem">block</span>
                </button>
              ` : `
                <button class="row-action-btn row-action-btn--success" data-action="reinstate" data-id="${p.id}" aria-label="Reinstate ${p.name}">
                  <span class="material-symbols-outlined" style="font-size:1.1rem">restore</span>
                </button>
              `}
              <button class="row-action-btn row-action-btn--danger" data-action="delete" data-id="${p.id}" aria-label="Delete ${p.name}">
                <span class="material-symbols-outlined" style="font-size:1.1rem">delete</span>
              </button>
            </div>
          </td>
        </tr>
      `;
    }).join('');

    this._renderPagination();
    this._bindRowActions();
  },

  /* ── Pagination ── */
  _renderPagination() {
    const info     = document.getElementById('admin-pagination-info');
    const controls = document.getElementById('admin-pagination-controls');
    if (!info || !controls) return;

    const total = this._filtered.length;
    const pages = Math.max(1, Math.ceil(total / this._perPage));
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
        this._currentPage = parseInt(btn.dataset.pg);
        this._renderTable();
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
    document.querySelectorAll('[data-action]').forEach(btn => {
      btn.addEventListener('click', () => {
        const { action, id } = btn.dataset;
        const provider = this._filtered.find(p => p.id === id);
        if (!provider) return;

        if (action === 'view') {
          const svcLabel  = SERVICE_LABELS[_resolveCategory(provider)] || _resolveCategory(provider) || '—';
          const typeLabel = provider.providerType === 'company' ? 'Company' : 'Individual';
          const typeIcon  = provider.providerType === 'company' ? 'business' : 'person';
          const statusCfg = {
            verified:  { bg:'rgba(26,122,74,0.1)',  color:'#1a7a4a', icon:'verified',    label:'Verified'      },
            pending:   { bg:'rgba(245,158,11,0.1)', color:'#d97706', icon:'schedule',    label:'Pending'       },
            review:    { bg:'rgba(245,158,11,0.1)', color:'#d97706', icon:'manage_search',label:'Under Review' },
            suspended: { bg:'rgba(186,26,26,0.1)',  color:'#ba1a1a', icon:'block',       label:'Suspended'     },
          };
          const st = statusCfg[provider.status] ?? statusCfg.pending;

          const skillsHtml = (provider.skills || []).length
            ? (provider.skills).map(s => `<span style="display:inline-block;padding:0.2rem 0.65rem;background:rgba(0,80,212,0.08);color:var(--color-primary);border-radius:9999px;font-size:0.75rem;font-weight:700;margin:0.15rem">${s}</span>`).join('')
            : '<span style="color:var(--color-on-surface-variant);font-size:0.82rem">—</span>';

          const companySection = provider.companyName ? `
            <div style="margin-top:var(--sp-4);padding:var(--sp-4);background:rgba(111,89,0,0.06);border-radius:var(--radius-md);border:1px solid rgba(111,89,0,0.15)">
              <p style="font-size:0.75rem;font-weight:700;text-transform:uppercase;letter-spacing:0.05em;color:#6f5900;margin-bottom:var(--sp-2)">Company Details</p>
              <div style="display:grid;grid-template-columns:1fr 1fr;gap:var(--sp-2);font-size:0.82rem">
                <div><span style="color:var(--color-on-surface-variant)">Name</span><br/><strong>${provider.companyName}</strong></div>
                ${provider.commercialReg ? `<div><span style="color:var(--color-on-surface-variant)">Reg. No.</span><br/><strong>${provider.commercialReg}</strong></div>` : ''}
                ${provider.companyAddress ? `<div style="grid-column:1/-1"><span style="color:var(--color-on-surface-variant)">Address</span><br/><strong>${provider.companyAddress}</strong></div>` : ''}
              </div>
            </div>` : '';

          const bioSection = provider.bio ? `
            <div style="margin-top:var(--sp-4);padding:var(--sp-4);background:var(--color-surface-container-low);border-radius:var(--radius-md)">
              <p style="font-size:0.75rem;font-weight:700;text-transform:uppercase;letter-spacing:0.05em;color:var(--color-on-surface-variant);margin-bottom:var(--sp-2)">Bio</p>
              <p style="font-size:0.85rem;line-height:1.65;color:var(--color-on-surface-variant)">${provider.bio}</p>
            </div>` : '';

          const joined = provider.submittedAt
            ? new Date(provider.submittedAt).toLocaleDateString('en-EG',{day:'numeric',month:'long',year:'numeric'})
            : '—';

          const cardHtml = `
            <div style="font-family:var(--font-body)">
              <!-- Header -->
              <div style="display:flex;align-items:center;gap:var(--sp-4);padding-bottom:var(--sp-5);border-bottom:1px solid var(--color-surface-container)">
                <img src="${provider.img || `https://ui-avatars.com/api/?name=${encodeURIComponent(provider.name)}&background=0050d4&color=fff&size=80`}"
                     style="width:4rem;height:4rem;border-radius:50%;object-fit:cover;border:2px solid var(--color-outline-variant)"
                     alt="${provider.name}" />
                <div style="flex:1;min-width:0">
                  <h4 style="font-size:1.05rem;font-weight:800;color:var(--color-on-surface);margin:0 0 0.25rem">${provider.name}</h4>
                  <div style="display:flex;align-items:center;flex-wrap:wrap;gap:0.4rem">
                    <span style="font-size:0.78rem;color:var(--color-on-surface-variant)">${provider.id}</span>
                    <span style="display:inline-flex;align-items:center;gap:0.25rem;padding:0.2rem 0.6rem;border-radius:9999px;font-size:0.72rem;font-weight:700;background:${st.bg};color:${st.color}">
                      <span class="material-symbols-outlined" style="font-size:0.85rem;font-variation-settings:'FILL' 1">${st.icon}</span>${st.label}
                    </span>
                    <span style="display:inline-flex;align-items:center;gap:0.2rem;padding:0.2rem 0.6rem;border-radius:9999px;font-size:0.72rem;font-weight:700;background:rgba(0,80,212,0.08);color:var(--color-primary)">
                      <span class="material-symbols-outlined" style="font-size:0.85rem">${typeIcon}</span>${typeLabel}
                    </span>
                  </div>
                </div>
              </div>

              <!-- Info grid -->
              <div style="display:grid;grid-template-columns:1fr 1fr;gap:var(--sp-3);margin-top:var(--sp-5)">
                <div style="padding:var(--sp-3);background:var(--color-surface-container-low);border-radius:var(--radius-md)">
                  <p style="font-size:0.72rem;font-weight:700;text-transform:uppercase;letter-spacing:0.05em;color:var(--color-on-surface-variant);margin-bottom:0.25rem">Service</p>
                  <p style="font-size:0.9rem;font-weight:700;color:var(--color-on-surface)">${svcLabel}</p>
                </div>
                <div style="padding:var(--sp-3);background:var(--color-surface-container-low);border-radius:var(--radius-md)">
                  <p style="font-size:0.72rem;font-weight:700;text-transform:uppercase;letter-spacing:0.05em;color:var(--color-on-surface-variant);margin-bottom:0.25rem">Experience</p>
                  <p style="font-size:0.9rem;font-weight:700;color:var(--color-on-surface)">${provider.experience || '—'}</p>
                </div>
                <div style="padding:var(--sp-3);background:var(--color-surface-container-low);border-radius:var(--radius-md)">
                  <p style="font-size:0.72rem;font-weight:700;text-transform:uppercase;letter-spacing:0.05em;color:var(--color-on-surface-variant);margin-bottom:0.25rem">Phone</p>
                  <p style="font-size:0.9rem;font-weight:700;color:var(--color-on-surface)">${provider.phone || '—'}</p>
                </div>
                <div style="padding:var(--sp-3);background:var(--color-surface-container-low);border-radius:var(--radius-md)">
                  <p style="font-size:0.72rem;font-weight:700;text-transform:uppercase;letter-spacing:0.05em;color:var(--color-on-surface-variant);margin-bottom:0.25rem">Location</p>
                  <p style="font-size:0.9rem;font-weight:700;color:var(--color-on-surface)">${provider.governorate || '—'}</p>
                </div>
                ${provider.email ? `
                <div style="padding:var(--sp-3);background:var(--color-surface-container-low);border-radius:var(--radius-md);grid-column:1/-1">
                  <p style="font-size:0.72rem;font-weight:700;text-transform:uppercase;letter-spacing:0.05em;color:var(--color-on-surface-variant);margin-bottom:0.25rem">Email</p>
                  <p style="font-size:0.9rem;font-weight:700;color:var(--color-on-surface)">${provider.email}</p>
                </div>` : ''}
                <div style="padding:var(--sp-3);background:var(--color-surface-container-low);border-radius:var(--radius-md);grid-column:1/-1">
                  <p style="font-size:0.72rem;font-weight:700;text-transform:uppercase;letter-spacing:0.05em;color:var(--color-on-surface-variant);margin-bottom:0.4rem">Skills</p>
                  <div>${skillsHtml}</div>
                </div>
                <div style="padding:var(--sp-3);background:var(--color-surface-container-low);border-radius:var(--radius-md);grid-column:1/-1">
                  <p style="font-size:0.72rem;font-weight:700;text-transform:uppercase;letter-spacing:0.05em;color:var(--color-on-surface-variant);margin-bottom:0.25rem">Joined</p>
                  <p style="font-size:0.85rem;color:var(--color-on-surface-variant)">${joined}</p>
                </div>
              </div>
              ${companySection}
              ${bioSection}
            </div>`;

          Modal.open({
            title: 'Provider Profile',
            content: cardHtml,
            confirmLabel: 'Close',
            onConfirm: () => {},
          });
        }

        if (action === 'approve') {
          Modal.open({
            title: 'Approve Provider',
            content: `<p>Approve <strong>${provider.name}</strong> as a verified provider?</p>`,
            confirmLabel: 'Approve',
            onConfirm: () => {
              provider.status = 'verified';
              this._syncToStorage();
              this._renderTable();
              showToast(`${provider.name} approved successfully.`, 'success');
            },
          });
        }

        if (action === 'suspend') {
          Modal.open({
            title: 'Suspend Provider',
            content: `<p>Suspend <strong>${provider.name}</strong>? They won't appear in search results.</p>`,
            confirmLabel: 'Suspend',
            onConfirm: () => {
              provider.status = 'suspended';
              this._syncToStorage();
              this._renderTable();
              showToast(`${provider.name} suspended.`, 'error');
            },
          });
        }

        if (action === 'reinstate') {
          provider.status = 'verified';
          this._syncToStorage();
          this._renderTable();
          showToast(`${provider.name} reinstated.`, 'success');
        }

        if (action === 'delete') {
          Modal.open({
            title: 'Delete Provider',
            content: `<p>Permanently delete <strong>${provider.name}</strong>? This cannot be undone.</p>`,
            confirmLabel: 'Delete',
            onConfirm: () => {
              this._filtered     = this._filtered.filter(p => p.id !== id);
              this._allProviders = this._allProviders.filter(p => p.id !== id);
              this._syncToStorage();
              this._renderTable();
              showToast(`${provider.name} deleted.`, 'error');
            },
          });
        }
      });
    });
  },

  /* Persist status changes back to localStorage */
  _syncToStorage() {
    try {
      const DEMO_IDS = ['PRV-001','PRV-002','PRV-003','PRV-004'];
      const realProviders = this._allProviders.filter(p => !DEMO_IDS.includes(p.id));
      localStorage.setItem('ek_admin_providers', JSON.stringify(realProviders));

      // Sync status back to ek_accounts so provider dashboard reflects changes
      const accounts = JSON.parse(localStorage.getItem('ek_accounts') || '[]');
      let changed = false;
      realProviders.forEach(p => {
        const idx = accounts.findIndex(a => a.phone === p.phone);
        if (idx !== -1 && accounts[idx].status !== p.status) {
          accounts[idx].status = p.status;
          changed = true;
        }
      });
      if (changed) localStorage.setItem('ek_accounts', JSON.stringify(accounts));
    } catch(e) {}
  },

  /* ── Search ── */
  _bindSearch() {
    const input = document.getElementById('admin-provider-search');
    if (!input) return;
    input.addEventListener('input', debounce(() => {
      this._applyFilters(input.value.toLowerCase().trim());
    }, 300));
  },

  /* ── Filters ── */
  _bindFilters() {
    ['admin-filter-category', 'admin-filter-status', 'admin-filter-experience'].forEach(id => {
      document.getElementById(id)?.addEventListener('change', () => this._applyFilters());
    });
    document.getElementById('admin-reset-filters-btn')?.addEventListener('click', () => {
      ['admin-filter-category', 'admin-filter-status', 'admin-filter-experience'].forEach(id => {
        const el = document.getElementById(id);
        if (el) el.value = '';
      });
      const s = document.getElementById('admin-provider-search');
      if (s) s.value = '';
      this._applyFilters('');
    });
  },

  _applyFilters(searchQuery) {
    const q        = searchQuery ?? document.getElementById('admin-provider-search')?.value.toLowerCase().trim() ?? '';
    const category = document.getElementById('admin-filter-category')?.value ?? '';
    const status   = document.getElementById('admin-filter-status')?.value ?? '';

    this._filtered = this._allProviders.filter(p => {
      const matchSearch   = !q        || p.name.toLowerCase().includes(q) || (p.id||'').toLowerCase().includes(q) || (p.phone||'').includes(q);
      const matchCategory = !category || p.category === category;
      const matchStatus   = !status   || p.status   === status;
      return matchSearch && matchCategory && matchStatus;
    });

    this._currentPage = 1;
    this._renderTable();
  },

  /* ── Add Provider ── */
  _bindAddProvider() {
    document.getElementById('admin-add-provider-btn')?.addEventListener('click', () => {
      Modal.open({
        title: 'Add New Provider',
        content: `
          <div style="display:flex;flex-direction:column;gap:var(--sp-4)">
            <input class="input-field" id="modal-provider-name" placeholder="Full Name" type="text"/>
            <input class="input-field" id="modal-provider-phone" placeholder="Phone Number" type="tel"/>
            <select class="select-field" id="modal-provider-category">
              <option value="">Select Service</option>
              <option value="plumbing">Plumbing</option>
              <option value="electrical">Electrical</option>
              <option value="cleaning">Cleaning</option>
              <option value="carpentry">Carpentry</option>
              <option value="painting">Painting</option>
              <option value="ac_repair">AC Repair</option>
            </select>
            <select class="select-field" id="modal-provider-type">
              <option value="individual">Individual</option>
              <option value="company">Company</option>
            </select>
            <select class="select-field" id="modal-provider-experience">
              <option value="1-3 yrs">1–3 Years</option>
              <option value="3-5 yrs">3–5 Years</option>
              <option value="5+ yrs">5+ Years</option>
            </select>
          </div>
        `,
        confirmLabel: 'Add Provider',
        onConfirm: () => {
          const name     = document.getElementById('modal-provider-name')?.value.trim();
          const phone    = document.getElementById('modal-provider-phone')?.value.trim();
          const category = document.getElementById('modal-provider-category')?.value;
          const type     = document.getElementById('modal-provider-type')?.value ?? 'individual';
          if (!name || !category) { showToast('Please fill in all required fields.', 'error'); return; }
          const newProvider = {
            id:           `PRV-${String(Date.now()).slice(-5)}`,
            name, phone,  category,
            providerType: type,
            experience:   document.getElementById('modal-provider-experience')?.value ?? '1-3 yrs',
            status:       'pending',
            img:          `https://ui-avatars.com/api/?name=${encodeURIComponent(name)}&background=0050d4&color=fff`,
            submittedAt:  new Date().toISOString(),
          };
          this._allProviders.unshift(newProvider);
          this._syncToStorage();
          this._applyFilters('');
          showToast(`${name} added successfully.`, 'success');
        },
      });
    });
  },
};

export default ProvidersController;