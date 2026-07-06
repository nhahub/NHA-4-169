/**
 * BAYTACK ADMIN — Verification Review Controller
 * Handles the individual provider review page.
 */
import AuthService from '../services/authService.js';

const VerificationReviewController = {
  _selectedDecision: null,

  async init() {
    AuthService.requireAuth();
    this._bindDecisionBtns();
    this._bindSubmit();
    console.log('[VerificationReviewController] ready');
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
    btn.addEventListener('click', () => {
      if (!this._selectedDecision) {
        alert('Please select a decision (Approve, Clarify, or Reject).');
        return;
      }
      this._showModal(this._selectedDecision);
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
