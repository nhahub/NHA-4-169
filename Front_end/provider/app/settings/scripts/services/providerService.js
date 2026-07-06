/**
 * services/providerService.js
 * ─────────────────────────────────────────────────────────
 * Business logic for provider profile, schedule, and preferences.
 * All API calls go through this service — controllers never call api.js directly.
 */

import api from '../core/api.js';
import { setStorageJSON, getStorageJSON } from '../core/helpers.js';
import AppConfig from '../core/config.js';

const { STORAGE_KEYS } = AppConfig;

/* ── Profile ─────────────────────────────────────────── */

/**
 * Fetch the current provider's profile from the API.
 * Falls back to localStorage if the request fails.
 * @returns {Promise<object>}
 */
export async function getProfile() {
  try {
    const data = await api.get('/provider/profile');
    setStorageJSON(STORAGE_KEYS.PROFILE, data);
    return data;
  } catch {
    return getStorageJSON(STORAGE_KEYS.PROFILE, {});
  }
}

/**
 * Persist updated profile fields.
 * Falls back to localStorage if the demo backend is unavailable.
 * @param {object} payload - { displayName, title, bio, experience }
 * @returns {Promise<object>}
 */
export async function updateProfile(payload) {
  try {
    const data = await api.put('/provider/profile', payload);
    setStorageJSON(STORAGE_KEYS.PROFILE, data);
    return data;
  } catch {
    const merged = { ...getStorageJSON(STORAGE_KEYS.PROFILE, {}), ...payload };
    setStorageJSON(STORAGE_KEYS.PROFILE, merged);
    return merged;
  }
}

/* ── Schedule ────────────────────────────────────────── */

/**
 * Fetch the provider's weekly schedule.
 * @returns {Promise<object[]>}
 */
export async function getSchedule() {
  try {
    const data = await api.get('/provider/schedule');
    setStorageJSON(STORAGE_KEYS.SCHEDULE, data);
    return data;
  } catch {
    return getStorageJSON(STORAGE_KEYS.SCHEDULE, []);
  }
}

/**
 * Save the full weekly schedule.
 * Falls back to localStorage if the demo backend is unavailable.
 * @param {object[]} schedule - Array of { day, enabled, start, end }
 * @returns {Promise<object[]>}
 */
export async function saveSchedule(schedule) {
  try {
    const data = await api.put('/provider/schedule', { schedule });
    setStorageJSON(STORAGE_KEYS.SCHEDULE, data);
    return data;
  } catch {
    setStorageJSON(STORAGE_KEYS.SCHEDULE, schedule);
    return schedule;
  }
}

/* ── Preferences ─────────────────────────────────────── */

/**
 * Fetch notification preferences.
 * @returns {Promise<object>}
 */
export async function getPreferences() {
  try {
    const data = await api.get('/provider/preferences');
    setStorageJSON(STORAGE_KEYS.PREFERENCES, data);
    return data;
  } catch {
    return getStorageJSON(STORAGE_KEYS.PREFERENCES, {
      pushNotifications: true,
      emailSummaries:    false,
      smsReminders:      true,
    });
  }
}

/**
 * Save notification preferences.
 * Falls back to localStorage if the demo backend is unavailable.
 * @param {object} prefs - { pushNotifications, emailSummaries, smsReminders }
 * @returns {Promise<object>}
 */
export async function savePreferences(prefs) {
  try {
    const data = await api.patch('/provider/preferences', prefs);
    setStorageJSON(STORAGE_KEYS.PREFERENCES, data);
    return data;
  } catch {
    const merged = { ...getStorageJSON(STORAGE_KEYS.PREFERENCES, {}), ...prefs };
    setStorageJSON(STORAGE_KEYS.PREFERENCES, merged);
    return merged;
  }
}

/* ── Account ─────────────────────────────────────────── */

/**
 * Pause/deactivate the provider's service listing permanently.
 * Falls back to a local flag if the demo backend is unavailable.
 * @returns {Promise<void>}
 */
export async function pauseServiceAvailability() {
  try {
    return await api.post('/provider/pause');
  } catch {
    setStorageJSON(STORAGE_KEYS.PREFERENCES, {
      ...getStorageJSON(STORAGE_KEYS.PREFERENCES, {}),
      paused: true,
    });
  }
}
