/**
 * charCounter.js — Live character counter for textareas
 * Radiant Pro / BayTack
 */

import { $$ } from "../core/helpers.js";

/**
 * Attach a live character counter to all elements matching
 * [data-char-counter] attribute.
 *
 * Markup contract:
 *   <textarea data-char-counter data-max="500" ...></textarea>
 *   <span data-char-counter-display="<textarea-id>">0 / 500</span>
 *
 * Or simpler: use the .form-label__hint[data-char-counter-target] pattern.
 */
export function initCharCounters() {
  const textareas = $$("textarea[data-char-counter]");

  textareas.forEach((textarea) => {
    const max = parseInt(textarea.dataset.max || "500", 10);
    const displayId = textarea.dataset.counterDisplay;
    const display = displayId ? document.getElementById(displayId) : null;

    if (!display) return;

    // Set initial state
    _update(textarea, display, max);

    // Update on input
    textarea.addEventListener("input", () => _update(textarea, display, max));
  });
}

function _update(textarea, display, max) {
  const count = textarea.value.length;
  display.textContent = `${count} / ${max}`;
  display.style.color =
    count > max * 0.9
      ? "var(--color-error)"
      : "var(--color-on-surface-variant)";
}
