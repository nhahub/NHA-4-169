/**
 * BAYTACK ADMIN — Verification Review Controller
 * Handles the individual provider review page.
 * Loads real data via ?id=<providerProfileId> from
 * GET /admin/verification/{id} (Controllers/Admin/VerificationController.cs).
 */
import AuthService         from '../services/authService.js';
import VerificationService from '../services/verificationService.js';
import { showToast } from '../core/helpers.js';

const VerificationReviewController = {
  _selectedDecision: null,
  _providerId: null,
  _entry: null,

  async init() {
    AuthService.requireAuth();
    this._providerId = new URLSearchParams(window.location.search).get('id');
    this._bindDecisionBtns();
    this._bindSubmit();
    await this._loadDetail();
    console.log('[VerificationReviewController] ready');
  },

  async _loadDetail() {
    if (!this._providerId) {
      showToast('No provider selected — open this page from the Verification queue.', 'error');
      return;
    }
    try {
      const detail = await VerificationService.getById(this._providerId);
      this._entry = detail.summary;
      this._renderSummary(detail.summary);
      this._renderDocuments(detail.documents);
    } catch (err) {
      console.warn('[VerificationReviewController] failed to load detail', err);
      showToast('Could not load this provider\u2019s verification details', 'error');
    }
  },

  _renderSummary(summary) {
    if (!summary) return;

    const breadcrumb = document.getElementById('vr-breadcrumb-current');
    if (breadcrumb) breadcrumb.textContent = `Review #${summary.id.slice(0, 8)}`;

    const nameEl = document.getElementById('vr-identity-name-text');
    if (nameEl) nameEl.textContent = summary.name;

    const roleEl = document.getElementById('vr-identity-role-text');
    if (roleEl) roleEl.textContent = `${summary.providerType} \u00b7 ${summary.category}`;

    const avatarEl = document.getElementById('vr-identity-avatar-img');
    if (avatarEl) {
      avatarEl.src = summary.imageUrl || `https://ui-avatars.com/api/?name=${encodeURIComponent(summary.name)}&background=0050d4&color=fff&size=200`;
      avatarEl.alt = summary.name;
    }

    const statusLabel = document.getElementById('vr-status-chip-label');
    if (statusLabel) {
      statusLabel.textContent = summary.status === 'UnderReview' ? 'Under Review'
        : summary.status === 'Pending' ? 'Pending Review'
        : summary.status;
    }
  },

  _renderDocuments(documents) {
    const grid = document.getElementById('vr-docs-grid');
    if (!grid) return;
    if (!documents || !documents.length) {
      grid.innerHTML = `<p style="padding:1rem;color:var(--color-on-surface-variant)">No documents submitted.</p>`;
      return;
    }
    grid.innerHTML = documents.map(doc => `
      <div class="vr-doc-card">
        <div class="vr-doc-img-wrap">
          <img src="${doc.url}" alt="${doc.documentType}" class="vr-doc-img"/>
          <a class="vr-doc-overlay" href="${doc.url}" target="_blank" rel="noopener">
            <span class="material-symbols-outlined">zoom_in</span>
          </a>
        </div>
        <div class="vr-doc-meta">
          <span class="vr-doc-name">${doc.documentType}</span>
          <span class="vr-doc-badge vr-doc-badge--ok">${doc.status}</span>
        </div>
      </div>
    `).join('');
  },

  _bindDecisionBtns() {
    const decisions = {
      'vr-approve-btn':  'approve',
      'vr-clarify-btn':  'clarify',
      'vr-reject-btn':   'reject',
    };

    Object.entries(decisions).forEach(([id, decision]) => {
      const btn = document.getElementById(id);
      if (!btn) return;
      btn.addEventListener('click', () => {
        this._selectedDecision = decision;
        // Highlight selected
        Object.keys(decisions).forEach(k => {
          document.getElementById(k)?.classList.remove('vr-decision-btn--selected');
        });
        btn.classList.add('vr-decision-btn--selected');
      });
    });
  },

  _bindSubmit() {
    const btn = document.getElementById('vr-submit-btn');
    if (!btn) return;
    btn.addEventListener('click', async () => {
      if (!this._selectedDecision) {
        alert('Please select a decision (Approve, Clarify, or Reject).');
        return;
      }
      if (!this._providerId) {
        showToast('No provider selected.', 'error');
        return;
      }

      const notes = document.getElementById('vr-notes')?.value?.trim() || '';

      try {
        if (this._selectedDecision === 'approve') {
          await VerificationService.approve(this._providerId);
        } else if (this._selectedDecision === 'clarify') {
          // No dedicated "request clarification" endpoint on the backend yet -
          // the closest real action is putting the profile back under review.
          await VerificationService.markUnderReview(this._providerId);
        } else if (this._selectedDecision === 'reject') {
          await VerificationService.reject(this._providerId, notes || 'Rejected by admin reviewer');
        }
        this._showModal(this._selectedDecision);
      } catch (err) {
        console.warn('[VerificationReviewController] decision submit failed', err);
        showToast('Could not submit this decision', 'error');
      }
    });
  },

  _showModal(decision) {
    const modal    = document.getElementById('vr-success-modal');
    const title    = document.getElementById('vr-modal-title');
    const sub      = document.getElementById('vr-modal-sub');
    const iconWrap = modal?.querySelector('.vr-modal-icon');
    if (!modal) return;

    const map = {
      approve: {
        title: 'Application Approved',
        sub:   'The provider has been approved and will be notified.',
        cls:   'vr-modal-icon--approve',
        icon:  'check_circle',
      },
      clarify: {
        title: 'Clarification Requested',
        sub:   'The provider will be notified to submit additional documents.',
        cls:   'vr-modal-icon--approve',
        icon:  'help',
      },
      reject: {
        title: 'Application Rejected',
        sub:   'The provider has been notified of the rejection.',
        cls:   'vr-modal-icon--reject',
        icon:  'cancel',
      },
    };

    const cfg = map[decision];
    if (title) title.textContent = cfg.title;
    if (sub) sub.textContent = cfg.sub;
    if (iconWrap) {
      iconWrap.className = `vr-modal-icon ${cfg.cls}`;
      const icon = iconWrap.querySelector('.material-symbols-outlined');
      if (icon) icon.textContent = cfg.icon;
    }

    modal.hidden = false;
  },
};

export default VerificationReviewController;
VerificationReviewController.init();
