/**
 * BayTack — Controller: Job Details
 * Handles all DOM interactions for the job-details page.
 */

import { qs, qsa, delegate } from '../core/helpers.js';
import { validateBid, submitBid } from '../services/bidService.js';

// ── State ──────────────────────────────────────────────────
const state = {
  jobId: null,
  isSubmitting: false,
};

// ── Init ───────────────────────────────────────────────────
export function init() {
  state.jobId = document.body.dataset.jobId ?? null;

  _bindBidForm();
  _bindPhotoGallery();
}

// ── Bid Form ───────────────────────────────────────────────
function _bindBidForm() {
  const form = document.getElementById('bid-form');
  if (!form) return;

  form.addEventListener('submit', async (e) => {
    e.preventDefault();
    if (state.isSubmitting) return;

    _clearErrors(form);

    const payload = {
      jobId:        state.jobId,
      price:        parseFloat(qs('#bid-price', form).value),
      durationDays: parseInt(qs('#bid-duration', form).value, 10),
      availability: qs('#bid-availability', form).value,
      notes:        qs('#bid-notes', form).value.trim(),
    };

    const { valid, errors } = validateBid(payload);
    if (!valid) {
      _showErrors(form, errors);
      return;
    }

    _setSubmitting(true, form);

    try {
      await submitBid(payload);
      _showSuccess(form);
    } catch (err) {
      _showErrors(form, [err.message ?? 'Something went wrong. Please try again.']);
    } finally {
      _setSubmitting(false, form);
    }
  });
}

function _setSubmitting(isSubmitting, form) {
  state.isSubmitting = isSubmitting;
  const btn = qs('#bid-submit-btn', form);
  btn.disabled = isSubmitting;
  btn.textContent = isSubmitting ? 'Submitting…' : 'Submit Bid';

  if (isSubmitting) {
    btn.classList.add('btn--loading');
  } else {
    btn.classList.remove('btn--loading');
  }
}

function _clearErrors(form) {
  qsa('.form-error-msg', form).forEach(el => el.remove());
  qsa('.form-input--error, .form-textarea--error', form)
    .forEach(el => el.classList.remove('form-input--error', 'form-textarea--error'));
}

function _showErrors(form, errors) {
  const container = qs('#form-errors', form);
  if (!container) return;

  container.innerHTML = errors
    .map(msg => `<p class="form-error-msg" role="alert">⚠ ${msg}</p>`)
    .join('');
  container.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
}

function _showSuccess(form) {
  form.innerHTML = `
    <div class="bid-success" role="status" aria-live="polite">
      <span class="material-symbols-outlined bid-success__icon">check_circle</span>
      <h3 class="bid-success__title">Bid Submitted!</h3>
      <p class="bid-success__msg">Ahmed M. will be notified immediately of your offer.</p>
    </div>
  `;
}

// ── Photo Gallery ──────────────────────────────────────────
function _bindPhotoGallery() {
  const gallery = document.getElementById('photo-gallery');
  if (!gallery) return;

  delegate(gallery, 'click', '.card--image', (e, card) => {
    const img = card.querySelector('img');
    if (!img) return;
    _openLightbox(img.src, img.alt);
  });
}

function _openLightbox(src, alt) {
  const existing = document.getElementById('lightbox-overlay');
  if (existing) existing.remove();

  const overlay = document.createElement('div');
  overlay.id = 'lightbox-overlay';
  overlay.setAttribute('role', 'dialog');
  overlay.setAttribute('aria-modal', 'true');
  overlay.setAttribute('aria-label', 'Photo preview');
  overlay.innerHTML = `
    <button class="lightbox__close" aria-label="Close preview" id="lightbox-close">
      <span class="material-symbols-outlined">close</span>
    </button>
    <img class="lightbox__img" src="${src}" alt="${alt}" />
  `;

  document.body.appendChild(overlay);
  requestAnimationFrame(() => overlay.classList.add('lightbox--visible'));

  const close = () => {
    overlay.classList.remove('lightbox--visible');
    overlay.addEventListener('transitionend', () => overlay.remove(), { once: true });
  };

  qs('#lightbox-close', overlay).addEventListener('click', close);
  overlay.addEventListener('click', (e) => { if (e.target === overlay) close(); });
  document.addEventListener('keydown', (e) => { if (e.key === 'Escape') close(); }, { once: true });
}
