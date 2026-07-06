/**
 * helpers.js — Pure utility functions (no DOM side effects)
 * The Radiant Marketplace — Provider Hub
 */

import Config from './config.js';

/**
 * Format a number as Egyptian Pounds.
 * @param {number} amount
 * @returns {string}  e.g. "1,200 ج.م"
 */
export function formatCurrency(amount) {
  return `${amount.toLocaleString(Config.LOCALE)} ${Config.CURRENCY}`;
}

/**
 * Format a date string into a human-readable label.
 * @param {string|Date} dateInput
 * @param {object} [options]
 * @returns {string}
 */
export function formatDate(dateInput, options = { month: 'short', day: 'numeric', year: 'numeric' }) {
  const d = dateInput instanceof Date ? dateInput : new Date(dateInput);
  return d.toLocaleDateString('en-EG', options);
}

/**
 * Return initials from a full name (up to 2 chars).
 * @param {string} name
 * @returns {string}
 */
export function getInitials(name = '') {
  return name
    .trim()
    .split(/\s+/)
    .slice(0, 2)
    .map(w => w[0].toUpperCase())
    .join('');
}

/**
 * Debounce a function call.
 * @param {Function} fn
 * @param {number} delay  ms
 * @returns {Function}
 */
export function debounce(fn, delay = 300) {
  let timer;
  return (...args) => {
    clearTimeout(timer);
    timer = setTimeout(() => fn(...args), delay);
  };
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
 * Truncate text to a max character count, appending "…".
 * @param {string} str
 * @param {number} [maxLen=80]
 * @returns {string}
 */
export function truncate(str, maxLen = 80) {
  return str.length > maxLen ? str.slice(0, maxLen - 1) + '…' : str;
}

/**
 * Capitalise the first letter of every word.
 * @param {string} str
 * @returns {string}
 */
export function titleCase(str) {
  return str.replace(/\b\w/g, c => c.toUpperCase());
}

/**
 * Safely get a nested object property by dot-path without throwing.
 * @param {object} obj
 * @param {string} path  e.g. 'user.address.city'
 * @param {*} [fallback]
 * @returns {*}
 */
export function getNestedValue(obj, path, fallback = undefined) {
  return path.split('.').reduce((acc, key) => (acc != null ? acc[key] : fallback), obj) ?? fallback;
}

/**
 * Generate a simple random ID string.
 * @param {number} [length=8]
 * @returns {string}
 */
export function generateId(length = 8) {
  return Math.random().toString(36).slice(2, 2 + length);
}
