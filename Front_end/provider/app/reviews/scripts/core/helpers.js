// ============================================
// PROVIDER HUB — Helper Utilities
// File: /scripts/core/helpers.js
// ============================================

/**
 * Select a single DOM element
 * @param {string} selector
 * @param {Element} [context=document]
 * @returns {Element|null}
 */
export const qs = (selector, context = document) =>
  context.querySelector(selector);

/**
 * Select multiple DOM elements
 * @param {string} selector
 * @param {Element} [context=document]
 * @returns {NodeList}
 */
export const qsAll = (selector, context = document) =>
  context.querySelectorAll(selector);

/**
 * Add event listener(s) to an element
 * @param {Element} el
 * @param {string} events  - space-separated event names
 * @param {Function} handler
 * @param {object} [options]
 */
export const on = (el, events, handler, options = {}) => {
  if (!el) return;
  events.split(' ').forEach(event => el.addEventListener(event, handler, options));
};

/**
 * Toggle a CSS class on an element
 */
export const toggleClass = (el, cls) => el?.classList.toggle(cls);

/**
 * Add CSS class
 */
export const addClass = (el, ...cls) => el?.classList.add(...cls);

/**
 * Remove CSS class
 */
export const removeClass = (el, ...cls) => el?.classList.remove(...cls);

/**
 * Check if element has a class
 */
export const hasClass = (el, cls) => el?.classList.contains(cls) ?? false;

/**
 * Debounce a function
 * @param {Function} fn
 * @param {number} delay - ms
 * @returns {Function}
 */
export const debounce = (fn, delay = 300) => {
  let timer;
  return (...args) => {
    clearTimeout(timer);
    timer = setTimeout(() => fn(...args), delay);
  };
};

/**
 * Format a relative timestamp string
 * @param {Date|string} date
 * @returns {string}
 */
export const relativeTime = (date) => {
  const diff = Date.now() - new Date(date).getTime();
  const mins  = Math.floor(diff / 60000);
  const hours = Math.floor(mins / 60);
  const days  = Math.floor(hours / 24);

  if (mins  < 60)  return `${mins} minute${mins !== 1 ? 's' : ''} ago`;
  if (hours < 24)  return `${hours} hour${hours !== 1 ? 's' : ''} ago`;
  if (days  < 7)   return `${days} day${days !== 1 ? 's' : ''} ago`;
  return new Date(date).toLocaleDateString('en-EG', { day: 'numeric', month: 'short' });
};

/**
 * Generate star rating HTML
 * @param {number} rating - 1–5
 * @param {number} [max=5]
 * @returns {string} HTML string
 */
export const starsHTML = (rating, max = 5) => {
  return Array.from({ length: max }, (_, i) => {
    const filled = i < Math.round(rating);
    return `<span class="material-symbols-outlined" style="font-variation-settings:'FILL' ${filled ? 1 : 0},'wght' 400,'GRAD' 0,'opsz' 24" aria-hidden="true">star</span>`;
  }).join('');
};

/**
 * Sanitize a string for safe HTML insertion
 * @param {string} str
 * @returns {string}
 */
export const sanitize = (str) => {
  const div = document.createElement('div');
  div.textContent = str;
  return div.innerHTML;
};
