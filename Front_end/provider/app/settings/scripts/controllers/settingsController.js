/**
 * controllers/settingsController.js
 * ─────────────────────────────────────────────────────────
 * Orchestrates the Provider Settings page:
 *   - Tracks unsaved changes
 *   - Handles form submission (profile, schedule, preferences)
 *   - Controls the floating save bar visibility
 *   - Wires schedule checkbox toggling
 *   - Confirms dangerous actions
 */

import {
  getProfile, updateProfile,
  getSchedule, saveSchedule,
  getPreferences, savePreferences,
  pauseServiceAvailability,
} from '../services/providerService.js';
import Toast from '../components/toast.js';

/* ── State ───────────────────────────────────────────── */
let isDirty = false;

/* ── DOM References ──────────────────────────────────── */
function el(id) { return document.getElementById(id); }

const refs = {
  saveBar:       () => el('save-bar'),
  discardBtn:    () => el('btn-discard'),
  saveAllBtn:    () => el('btn-save-all'),
  saveScheduleBtn: () => el('btn-save-schedule'),
  pauseBtn:      () => el('btn-pause-service'),
  scheduleRows:  () => document.querySelectorAll('.schedule-row'),
};

/* ── Unsaved-changes tracking ────────────────────────── */

function markDirty() {
  if (!isDirty) {
    isDirty = true;
    const bar = refs.saveBar();
    if (bar) {
      bar.style.display = 'flex';
      bar.classList.add('save-bar--visible');
    }
  }
}

function markClean() {
  isDirty = false;
  const bar = refs.saveBar();
  if (bar) {
    bar.style.display = 'none';
    bar.classList.remove('save-bar--visible');
  }
}

/* ── Schedule Row Toggle ─────────────────────────────── */

function initScheduleRows() {
  refs.scheduleRows().forEach((row) => {
    const checkbox = row.querySelector('.checkbox');
    if (!checkbox) return;

    checkbox.addEventListener('change', () => {
      row.classList.toggle('schedule-row--unavailable', !checkbox.checked);
      markDirty();
    });

    // Also mark dirty on time changes
    row.querySelectorAll('.input--time').forEach((input) => {
      input.addEventListener('change', markDirty);
    });
  });
}

/* ── Collect form data ───────────────────────────────── */

function collectProfile() {
  return {
    displayName:  el('field-display-name')?.value.trim()    || '',
    title:        el('field-title')?.value.trim()            || '',
    bio:          el('field-bio')?.value.trim()              || '',
    experience:   Number(el('field-experience')?.value)     || 0,
    streetAddress: el('field-street')?.value.trim()         || '',
    district:     el('field-district')?.value               || '',
  };
}

function collectSchedule() {
  const rows = [];
  refs.scheduleRows().forEach((row) => {
    const day      = row.dataset.day;
    const checkbox = row.querySelector('.checkbox');
    const [startEl, endEl] = row.querySelectorAll('.input--time');
    rows.push({
      day,
      enabled: checkbox?.checked ?? false,
      start:   startEl?.value || '09:00',
      end:     endEl?.value   || '17:00',
    });
  });
  return rows;
}

function collectPreferences() {
  return {
    pushNotifications: el('toggle-push')?.checked   ?? false,
    emailSummaries:    el('toggle-email')?.checked  ?? false,
    smsReminders:      el('toggle-sms')?.checked    ?? false,
  };
}

/* ── Save Handlers ───────────────────────────────────── */

async function handleSaveAll() {
  const saveBtn = refs.saveAllBtn();
  if (saveBtn) saveBtn.textContent = 'Saving…';

  try {
    await Promise.all([
      updateProfile(collectProfile()),
      saveSchedule(collectSchedule()),
      savePreferences(collectPreferences()),
    ]);
    markClean();
    Toast.show('All settings saved successfully!', 'success');
  } catch (err) {
    console.error('[settingsController] Save failed:', err);
    Toast.show('Failed to save. Please try again.', 'error');
  } finally {
    if (saveBtn) saveBtn.textContent = 'Save All Settings';
  }
}

async function handleSaveSchedule() {
  const btn = refs.saveScheduleBtn();
  if (btn) { btn.textContent = 'Saving…'; btn.disabled = true; }

  try {
    await saveSchedule(collectSchedule());
    markClean();
    Toast.show('Schedule saved!', 'success');
  } catch {
    Toast.show('Could not save schedule.', 'error');
  } finally {
    if (btn) { btn.textContent = 'Save Schedule Changes'; btn.disabled = false; }
  }
}

function handleDiscard() {
  if (!confirm('Discard all unsaved changes?')) return;
  // Reload the page to restore original state
  window.location.reload();
}

async function handlePauseService() {
  const confirmed = confirm(
    'Are you sure you want to permanently pause your service availability? ' +
    'This will hide your listing from the marketplace.'
  );
  if (!confirmed) return;

  try {
    await pauseServiceAvailability();
    Toast.show('Your service has been paused.', 'warning');
  } catch {
    Toast.show('Could not pause service. Try again.', 'error');
  }
}

/* ── Input change listeners ──────────────────────────── */

function initChangeListeners() {
  const watchedIds = [
    'field-display-name', 'field-title', 'field-bio',
    'field-experience',   'field-street', 'field-district',
    'toggle-push', 'toggle-email', 'toggle-sms',
  ];

  watchedIds.forEach((id) => {
    const elem = el(id);
    if (elem) elem.addEventListener('change', markDirty);
  });
}

/* ── Init ────────────────────────────────────────────── */

export async function initSettingsController() {
  // Wire action buttons
  refs.saveAllBtn()     ?.addEventListener('click', handleSaveAll);
  refs.discardBtn()     ?.addEventListener('click', handleDiscard);
  refs.saveScheduleBtn()?.addEventListener('click', handleSaveSchedule);
  refs.pauseBtn()       ?.addEventListener('click', handlePauseService);

  // Start hidden
  markClean();

  // Track changes
  initChangeListeners();
  initScheduleRows();

  // (Optional) pre-fill fields from API/cache — uncomment when backend is ready
  // const profile = await getProfile();
  // if (profile.displayName && el('field-display-name')) {
  //   el('field-display-name').value = profile.displayName;
  // }
}
