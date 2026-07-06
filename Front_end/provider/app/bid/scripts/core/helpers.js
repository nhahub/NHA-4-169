/**
 * BayTack — Core: Helpers
 * Pure utility functions — no DOM dependencies.
 */

/**
 * Safely query a single DOM element. Throws a clear error if not found.
 * @param {string} selector
 * @param {Element|Document} [root=document]
 * @returns {Element}
 */
export function qs(selector, root = document) {
  const el = root.querySelector(selector);
  if (!el) throw new Error(`[BayTack] Element not found: "${selector}"`);
  return el;
}

/**
 * Query all matching DOM elements.
 * @param {string} selector
 * @param {Element|Document} [root=document]
 * @returns {Element[]}
 */
export function qsa(selector, root = document) {
  return Array.from(root.querySelectorAll(selector));
}

/**
 * Delegate an event from a parent to matching children.
 * @param {Element} parent
 * @param {string} eventType
 * @param {string} childSelector
 * @param {Function} handler
 */
export function delegate(parent, eventType, childSelector, handler) {
  parent.addEventListener(eventType, (e) => {
    const target = e.target.closest(childSelector);
    if (target && parent.contains(target)) {
      handler(e, target);
    }
  });
}

/**
 * Format a number as Egyptian Pounds.
 * @param {number} amount
 * @returns {string}
 */
export function formatEGP(amount) {
  return new Intl.NumberFormat('ar-EG', {
    style: 'currency',
    currency: 'EGP',
    maximumFractionDigits: 0,
  }).format(amount);
}

/**
 * Debounce a function call.
 * @param {Function} fn
 * @param {number} wait ms
 * @returns {Function}
 */
export function debounce(fn, wait) {
  let timer;
  return (...args) => {
    clearTimeout(timer);
    timer = setTimeout(() => fn(...args), wait);
  };
}

/**
 * Simple deep-clone via JSON (no circular refs / functions).
 * @param {*} obj
 * @returns {*}
 */
export function clone(obj) {
  return JSON.parse(JSON.stringify(obj));
}

/**
 * Validate that a numeric string is a positive number.
 * @param {string} value
 * @returns {boolean}
 */
export function isPositiveNumber(value) {
  const n = parseFloat(value);
  return !isNaN(n) && n > 0;
}

/**
 * Truncate a string to a max length, appending ellipsis.
 * @param {string} str
 * @param {number} max
 * @returns {string}
 */
export function truncate(str, max = 120) {
  return str.length <= max ? str : str.slice(0, max).trimEnd() + '…';
}
