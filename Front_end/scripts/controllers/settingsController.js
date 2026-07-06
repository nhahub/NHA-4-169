/**
 * EKHDEMNI ADMIN — settingsController
 * Loads and saves system-wide settings.
 */

import AuthService from '../services/authService.js';
import api         from '../core/api.js';

const $ = id => document.getElementById(id);

let _original = {};

function _fmt(d) {
  if (!d) return '—';
  return new Date(d).toLocaleString('en-EG', {
    day:'numeric', month:'short', year:'numeric',
    hour:'2-digit', minute:'2-digit',
  });
}

async function _load() {
  try {
    const res = await api.get('/settings');
    const cfg = res?.data ?? res ?? {};
    _original = { ...cfg };

    const toggle = $('admin-settings-platform-toggle');
    if (toggle) toggle.checked = cfg.platformActive !== false;

    const feeInput = $('admin-settings-fee-input');
    if (feeInput && cfg.platformFee !== undefined) feeInput.value = cfg.platformFee;

    const roleSelect = $('admin-settings-default-role');
    if (roleSelect && cfg.defaultUserRole) roleSelect.value = cfg.defaultUserRole;

    const emailInput = $('admin-settings-support-email');
    if (emailInput && cfg.supportEmail) emailInput.value = cfg.supportEmail;

    const msgInput = $('admin-settings-maintenance-msg');
    if (msgInput && cfg.maintenanceMessage) msgInput.value = cfg.maintenanceMessage;

    const lastEl = $('admin-settings-last-updated');
    if (lastEl) lastEl.textContent = `Last updated: ${_fmt(cfg.updatedAt)}`;

    _updateStatusBadge(cfg.platformActive !== false);
  } catch(err) {
    console.error('[settingsController] load', err);
  }
}

function _updateStatusBadge(isActive) {
  const badge = $('admin-settings-status-badge');
  if (!badge) return;
  const label = badge.querySelector('.settings-status-badge__label');
  const dot   = badge.querySelector('.settings-status-badge__dot');
  if (isActive) {
    badge.style.background = 'rgba(105,246,184,0.12)';
    if (dot)   dot.style.background   = 'var(--color-secondary)';
    if (label) label.style.color      = 'var(--color-secondary)';
    if (label) label.textContent      = 'Live & Functional';
  } else {
    badge.style.background = 'rgba(251,81,81,0.10)';
    if (dot)   dot.style.background   = 'var(--color-error)';
    if (label) label.style.color      = 'var(--color-error)';
    if (label) label.textContent      = 'Maintenance Mode';
  }
}

async function _save() {
  const payload = {
    platformActive:    $('admin-settings-platform-toggle')?.checked ?? true,
    platformFee:       parseFloat($('admin-settings-fee-input')?.value ?? '0'),
    defaultUserRole:   $('admin-settings-default-role')?.value ?? 'customer',
    supportEmail:      $('admin-settings-support-email')?.value?.trim() ?? '',
    maintenanceMessage:$('admin-settings-maintenance-msg')?.value?.trim() ?? '',
  };

  try {
    $('admin-settings-save-btn').disabled = true;
    await api.put('/settings', payload);
    await _load();
  } catch(err) {
    console.error('[settingsController] save', err);
  } finally {
    $('admin-settings-save-btn').disabled = false;
  }
}

function _discard() {
  const toggle = $('admin-settings-platform-toggle');
  if (toggle) toggle.checked = _original.platformActive !== false;

  const feeInput = $('admin-settings-fee-input');
  if (feeInput) feeInput.value = _original.platformFee ?? '12.5';

  const roleSelect = $('admin-settings-default-role');
  if (roleSelect && _original.defaultUserRole) roleSelect.value = _original.defaultUserRole;

  const emailInput = $('admin-settings-support-email');
  if (emailInput) emailInput.value = _original.supportEmail ?? '';

  const msgInput = $('admin-settings-maintenance-msg');
  if (msgInput)   msgInput.value   = _original.maintenanceMessage ?? '';

  _updateStatusBadge(_original.platformActive !== false);
}

const Controller = {
  async init() {
    AuthService.requireAuth();
    await _load();

    $('admin-settings-save-btn')?.addEventListener('click', _save);
    $('admin-settings-discard-btn')?.addEventListener('click', _discard);

    // Live badge update on toggle change
    $('admin-settings-platform-toggle')?.addEventListener('change', e => {
      _updateStatusBadge(e.target.checked);
    });

    console.log('[settingsController] ready');
  },
};

export default Controller;
