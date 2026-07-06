/**
 * helpers.js — General-purpose utility functions
 * Radiant Pro / BayTack
 */

/**
 * Shorthand selector
 * @param {string} selector
 * @param {Element} [root=document]
 * @returns {Element|null}
 */
export const $ = (selector, root = document) => root.querySelector(selector);

/**
 * Shorthand selector (all)
 * @param {string} selector
 * @param {Element} [root=document]
 * @returns {NodeList}
 */
export const $$ = (selector, root = document) => root.querySelectorAll(selector);

/**
 * Delegate event listener — attaches to a parent, fires only when
 * target matches the given selector.
 *
 * @param {Element} parent
 * @param {string} event
 * @param {string} selector
 * @param {Function} handler
 */
export function delegate(parent, event, selector, handler) {
  parent.addEventListener(event, (e) => {
    const target = e.target.closest(selector);
    if (target && parent.contains(target)) {
      handler(e, target);
    }
  });
}

/**
 * Load an HTML partial and inject it into a container.
 *
 * @param {string} url        - Path to the .html partial
 * @param {string} containerId - ID of the target element
 * @returns {Promise<void>}
 */
export async function loadComponent(url, containerId) {
  const container = document.getElementById(containerId);
  if (!container) {
    console.warn(`[loadComponent] Container #${containerId} not found.`);
    return;
  }
  try {
    const res = await fetch(url);
    if (!res.ok) throw new Error(`HTTP ${res.status}`);
    container.innerHTML = await res.text();
  } catch (err) {
    console.error(`[loadComponent] Failed to load "${url}":`, err);
  }
}

/**
 * Format a number as Egyptian Pounds.
 * @param {number} amount
 * @returns {string}  e.g. "12,500 EGP"
 */
export function formatEGP(amount) {
  return `${Number(amount).toLocaleString("ar-EG")} EGP`;
}

/**
 * Clamp a string to a max length with ellipsis.
 * @param {string} str
 * @param {number} max
 * @returns {string}
 */
export function truncate(str, max) {
  return str.length > max ? str.slice(0, max).trimEnd() + "…" : str;
}

/**
 * Debounce a function.
 * @param {Function} fn
 * @param {number} delay
 * @returns {Function}
 */
export function debounce(fn, delay) {
  let timer;
  return (...args) => {
    clearTimeout(timer);
    timer = setTimeout(() => fn(...args), delay);
  };
}

/**
 * Show a temporary toast notification.
 * Requires a #toast element in the DOM.
 *
 * @param {string} message
 * @param {'success'|'error'|'info'} [type='info']
 * @param {number} [duration=3500]
 */
export function showToast(message, type = "info", duration = 3500) {
  const toast = document.getElementById("toast");
  if (!toast) return;
  toast.textContent = message;
  toast.dataset.type = type;
  toast.classList.add("is-visible");
  setTimeout(() => toast.classList.remove("is-visible"), duration);
}
