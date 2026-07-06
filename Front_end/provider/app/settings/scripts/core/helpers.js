/**
 * core/helpers.js
 * ─────────────────────────────────────────────────────────
 * Pure utility functions used across the app.
 * No DOM dependencies — safe to import anywhere.
 */

/**
 * Debounce: delays calling `fn` until `ms` ms have passed
 * since the last invocation.
 * @param {Function} fn
 * @param {number} ms
 * @returns {Function}
 */
export function debounce(fn, ms = 300) {
  let timer;
  return (...args) => {
    clearTimeout(timer);
    timer = setTimeout(() => fn(...args), ms);
  };
}

/**
 * Throttle: ensures `fn` is called at most once per `ms` ms.
 * @param {Function} fn
 * @param {number} ms
 * @returns {Function}
 */
export function throttle(fn, ms = 100) {
  let last = 0;
  return (...args) => {
    const now = Date.now();
    if (now - last >= ms) {
      last = now;
      fn(...args);
    }
  };
}

/**
 * Safely parse JSON from localStorage.
 * Returns `fallback` if the key doesn't exist or JSON is invalid.
 * @param {string} key
 * @param {*} fallback
 * @returns {*}
 */
export function getStorageJSON(key, fallback = null) {
  try {
    const raw = localStorage.getItem(key);
    return raw ? JSON.parse(raw) : fallback;
  } catch {
    return fallback;
  }
}

/**
 * Stringify and store a value in localStorage.
 * @param {string} key
 * @param {*} value
 */
export function setStorageJSON(key, value) {
  try {
    localStorage.setItem(key, JSON.stringify(value));
  } catch (err) {
    console.warn('[helpers] localStorage write failed:', err);
  }
}

/**
 * Format a 24h time string (HH:MM) into a human-readable 12h string.
 * e.g. '14:00' → '2:00 PM'
 * @param {string} time24
 * @returns {string}
 */
export function formatTime12h(time24) {
  const [h, m] = time24.split(':').map(Number);
  const period = h >= 12 ? 'PM' : 'AM';
  const hour   = h % 12 || 12;
  return `${hour}:${String(m).padStart(2, '0')} ${period}`;
}

/**
 * Clamp a number between min and max.
 * @param {number} value
 * @param {number} min
 * @param {number} max
 * @returns {number}
 */
export function clamp(value, min, max) {
  return Math.min(Math.max(value, min), max);
}

/**
 * Generate a lightweight unique ID (not crypto-secure).
 * @returns {string}
 */
export function uid() {
  return Math.random().toString(36).slice(2, 10);
}

/**
 * Check if a value is a non-empty string.
 * @param {*} val
 * @returns {boolean}
 */
export function isNonEmptyString(val) {
  return typeof val === 'string' && val.trim().length > 0;
}
