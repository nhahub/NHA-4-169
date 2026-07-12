/**
 * BAYTACK ADMIN — Verification Controller
 * Renders the verification queue from the real backend
 * (Controllers/Admin/VerificationController.cs) with approve/reject/review actions.
 */
import AuthService        from '../services/authService.js';
import VerificationService from '../services/verificationService.js';
import Modal               from '../components/modal.js';
import { showToast } from '../core/helpers.js';

const VerificationController = {
  _entries: [],

  async init() {
    AuthService.requireAuth();
    await this._loadQueue();
    this._bindSearch();
    console.log('[VerificationController] ready');
  },

  async _loadQueue() {
    try {
      this._entries = await VerificationService.getQueue();
    } catch (err) {
      console.warn('[VerificationController] failed to load queue', err);
      showToast('Could not load the verification queue', 'error');
      this._entries = [];
    }
    this._renderTable();
    this._updateStats();
  },

  _renderTable() {
    const tbody = document.getElementById('vd-tbody');
    if (!tbody) return;

    if (this._entries.length === 0) {
      tbody.innerHTML = `<tr><td colspan="5" style="text-align:center;padding:3rem;color:var(--color-on-surface-variant)">No pending submissions.</td></tr>`;
      return;
    }

    tbody.innerHTML = this._entries.map(p => {
      const date  = p.submittedAt ? new Date(p.submittedAt).toLocaleDateString('en-EG', { day:'numeric', month:'short', year:'numeric' }) : '—';
      const label = p.category || '—';
      const typeLabel = (p.providerType || '').toLowerCase() === 'company' ? '🏢 Company' : '👤 Individual';
      const isReview  = (p.status || '').toLowerCase() === 'underreview';
      const statusCls = isReview ? 'vd-status--review' : 'vd-status--pending';
      const statusTxt = isReview ? 'Under Review' : 'Pending';
      const img = p.imageUrl || `https://ui-avatars.com/api/?name=${encodeURIComponent(p.name)}&background=0050d4&color=fff`;

      return `
        <tr data-id="${p.id}">
          <td>
            <div class="provider-cell">
              <div class="provider-cell__avatar vd-avatar-wrap">
                <img src="${img}" alt="${p.name}" class="vd-avatar-img" width="40" height="40"/>
              </div>
              <div>
                <p class="provider-cell__name">${p.name}</p>
                <p class="provider-cell__meta">ID: #${p.id.slice(0, 8)} · ${typeLabel}</p>
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
        const entry = this._entries.find(p => p.id === id);
        if (!entry) return;

        if (action === 'review') {
          window.location.href = `verification-review.html?id=${encodeURIComponent(id)}`;
        }

        if (action === 'approve') {
          Modal.open({
            title: 'Approve Provider',
            content: `<p>Approve <strong>${entry.name}</strong>? Their account will become active immediately.</p>`,
            confirmLabel: 'Approve',
            onConfirm: async () => {
              try {
                await VerificationService.approve(id);
                showToast(`${entry.name} has been approved!`, 'success');
                await this._loadQueue();
              } catch (err) {
                console.warn('[VerificationController] approve failed', err);
                showToast('Could not approve this provider', 'error');
              }
            },
          });
        }

        if (action === 'reject') {
          Modal.open({
            title: 'Reject Application',
            content: `<p>Reject <strong>${entry.name}</strong>'s application? They will be notified and can re-apply.</p>`,
            confirmLabel: 'Reject',
            onConfirm: async () => {
              try {
                await VerificationService.reject(id, 'Rejected from the verification queue');
                showToast(`${entry.name}'s application rejected.`, 'error');
                await this._loadQueue();
              } catch (err) {
                console.warn('[VerificationController] reject failed', err);
                showToast('Could not reject this provider', 'error');
              }
            },
          });
        }
      });
    });
  },

  _updateStats() {
    const pending = this._entries.filter(p => (p.status || '').toLowerCase() === 'pending').length;
    const review  = this._entries.filter(p => (p.status || '').toLowerCase() === 'underreview').length;

    const cards = document.querySelectorAll('.vd-bento-card');
    // Card order on verification.html: [0] Pending Review, [1] Under Review, [2] Weekly Performance (no number), [3] Approved This Month (no live data source yet)
    if (cards[0]) { const n = cards[0].querySelector('.vd-bento-number'); if (n) n.textContent = pending; }
    if (cards[1]) { const n = cards[1].querySelector('.vd-bento-number'); if (n) n.textContent = review; }
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
