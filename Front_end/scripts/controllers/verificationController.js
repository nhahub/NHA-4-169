/**
 * EKHDEMNI ADMIN — Verification Controller
 * Renders real submitted providers from localStorage (ek_admin_providers)
 * merged with static demo rows, with approve/reject/review actions.
 */
import AuthService from '../services/authService.js';
import Modal       from '../components/modal.js';
import { showToast } from '../core/helpers.js';

const SERVICE_LABELS = {
  plumbing:'Plumbing', electrical:'Electrical', cleaning:'Cleaning',
  carpentry:'Carpentry', painting:'Painting', ac_repair:'AC Repair',
};

const VerificationController = {
  _providers: [],

  async init() {
    AuthService.requireAuth();
    this._loadProviders();
    this._renderTable();
    this._bindSearch();
    this._updateStats();
    console.log('[VerificationController] ready');
  },

  _loadProviders() {
    const DEMO = [
      { id:'PV-8892', name:'Khaled Mansour',  category:'electrical', providerType:'individual', status:'pending', submittedAt:'2024-10-24', img:'https://ui-avatars.com/api/?name=Khaled+Mansour&background=0050d4&color=fff' },
      { id:'PV-8901', name:'Sara El-Din',     category:'cleaning',   providerType:'individual', status:'review',  submittedAt:'2024-10-23', img:'https://ui-avatars.com/api/?name=Sara+ElDin&background=6f5900&color=fff' },
      { id:'PV-8877', name:'Mustafa Kamel',   category:'plumbing',   providerType:'individual', status:'pending', submittedAt:'2024-10-22', img:'https://ui-avatars.com/api/?name=Mustafa+Kamel&background=595c5e&color=fff' },
      { id:'PV-8899', name:'Hassan Zaki',     category:'carpentry',  providerType:'company',    status:'pending', submittedAt:'2024-10-21', img:'https://ui-avatars.com/api/?name=Hassan+Zaki&background=b31b25&color=fff' },
    ];
    try {
      const stored = JSON.parse(localStorage.getItem('ek_admin_providers') || '[]');
      // Only show pending / review in verification
      const relevant = stored.filter(p => p.status === 'pending' || p.status === 'review');
      const demoIds  = new Set(relevant.map(p => p.id));
      this._providers = [...relevant, ...DEMO.filter(d => !demoIds.has(d.id))];
    } catch { this._providers = DEMO; }
  },

  _renderTable() {
    const tbody = document.getElementById('vd-tbody');
    if (!tbody) return;

    if (this._providers.length === 0) {
      tbody.innerHTML = `<tr><td colspan="5" style="text-align:center;padding:3rem;color:var(--color-on-surface-variant)">No pending submissions.</td></tr>`;
      return;
    }

    tbody.innerHTML = this._providers.map(p => {
      const date  = p.submittedAt ? new Date(p.submittedAt).toLocaleDateString('en-EG', { day:'numeric', month:'short', year:'numeric' }) : '—';
      const label = SERVICE_LABELS[p.category] || p.category || '—';
      const typeLabel = p.providerType === 'company' ? '🏢 Company' : '👤 Individual';
      const statusCls = p.status === 'review' ? 'vd-status--review' : 'vd-status--pending';
      const statusTxt = p.status === 'review' ? 'Under Review' : 'Pending';

      return `
        <tr data-id="${p.id}">
          <td>
            <div class="provider-cell">
              <div class="provider-cell__avatar vd-avatar-wrap">
                <img src="${p.img || `https://ui-avatars.com/api/?name=${encodeURIComponent(p.name)}&background=0050d4&color=fff`}"
                     alt="${p.name}" class="vd-avatar-img" width="40" height="40"/>
              </div>
              <div>
                <p class="provider-cell__name">${p.name}</p>
                <p class="provider-cell__meta">ID: #${p.id} · ${typeLabel}${p.phone ? ' · ' + p.phone : ''}</p>
              </div>
            </div>
          </td>
          <td><span class="provider-cell__meta">${date}</span></td>
          <td><span class="vd-type-chip">${label}</span></td>
          <td>
            <div class="vd-status ${statusCls}">
              <span class="vd-status-dot"></span>
              ${statusTxt}
            </div>
          </td>
          <td style="text-align:right">
            <div style="display:flex;gap:0.5rem;justify-content:flex-end;flex-wrap:wrap">
              <button class="btn btn--primary btn--sm vd-review-btn" data-action="review" data-id="${p.id}">
                Review Application
              </button>
              <button class="btn btn--sm" style="background:var(--color-success-container,#d1fae5);color:var(--color-success,#059669);font-weight:700"
                      data-action="approve" data-id="${p.id}">
                ✓ Approve
              </button>
              <button class="btn btn--sm" style="background:var(--color-error-container,#fde8e8);color:var(--color-error,#b91c1c);font-weight:700"
                      data-action="reject" data-id="${p.id}">
                ✗ Reject
              </button>
            </div>
          </td>
        </tr>
      `;
    }).join('');

    this._bindActions();
  },

  _bindActions() {
    document.querySelectorAll('[data-action]').forEach(btn => {
      btn.addEventListener('click', () => {
        const { action, id } = btn.dataset;
        const provider = this._providers.find(p => p.id === id);
        if (!provider) return;

        if (action === 'review') {
          // Save provider data and navigate to review page
          sessionStorage.setItem('bt_review_provider', JSON.stringify(provider));
          window.location.href = 'verification-review.html';
        }

        if (action === 'approve') {
          Modal.open({
            title: 'Approve Provider',
            content: `<p>Approve <strong>${provider.name}</strong>? Their account will become active immediately.</p>`,
            confirmLabel: 'Approve',
            onConfirm: () => {
              this._updateStatus(id, 'verified');
              this._providers = this._providers.filter(p => p.id !== id);
              this._renderTable();
              this._updateStats();
              showToast(`${provider.name} has been approved!`, 'success');
            },
          });
        }

        if (action === 'reject') {
          Modal.open({
            title: 'Reject Application',
            content: `<p>Reject <strong>${provider.name}</strong>'s application? They will be notified and can re-apply.</p>`,
            confirmLabel: 'Reject',
            onConfirm: () => {
              this._updateStatus(id, 'suspended');
              this._providers = this._providers.filter(p => p.id !== id);
              this._renderTable();
              this._updateStats();
              showToast(`${provider.name}'s application rejected.`, 'error');
            },
          });
        }
      });
    });
  },

  _updateStatus(id, newStatus) {
    try {
      // Update ek_admin_providers
      const stored = JSON.parse(localStorage.getItem('ek_admin_providers') || '[]');
      const idx = stored.findIndex(p => p.id === id);
      if (idx !== -1) {
        stored[idx].status = newStatus;
        localStorage.setItem('ek_admin_providers', JSON.stringify(stored));
        // Sync to ek_accounts so provider dashboard reflects change immediately
        const phone = stored[idx].phone;
        if (phone) {
          const accounts = JSON.parse(localStorage.getItem('ek_accounts') || '[]');
          const aIdx = accounts.findIndex(a => a.phone === phone);
          if (aIdx !== -1) {
            accounts[aIdx].status = newStatus;
            localStorage.setItem('ek_accounts', JSON.stringify(accounts));
          }
        }
      }
    } catch(e) {}
  },

  _updateStats() {
    const total   = this._providers.length;
    const pending = this._providers.filter(p => p.status === 'pending').length;
    const review  = this._providers.filter(p => p.status === 'review').length;
    const el = document.querySelector('.vd-bento-label');
    if (el) el.closest('.vd-bento-card')?.querySelector('.vd-bento-number') && null;
    // Update bento stats if elements exist
    document.querySelectorAll('.vd-bento-card').forEach((card, i) => {
      const numEl = card.querySelector('.vd-bento-number');
      if (!numEl) return;
      if (i === 0) numEl.textContent = total;
      if (i === 1) numEl.textContent = pending;
      if (i === 2) numEl.textContent = review;
    });
  },

  _bindSearch() {
    const input = document.getElementById('vd-search');
    if (!input) return;
    input.addEventListener('input', () => {
      const q = input.value.trim().toLowerCase();
      document.querySelectorAll('#vd-tbody tr[data-id]').forEach(row => {
        row.style.display = !q || row.textContent.toLowerCase().includes(q) ? '' : 'none';
      });
    });
  },
};

export default VerificationController;