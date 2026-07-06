/**
 * toggle.js — Accessible toggle switch UI component
 * Radiant Pro / BayTack
 */

import { $$ } from "../core/helpers.js";

/**
 * Initialise all toggle switches in the DOM.
 *
 * Markup contract:
 *   <div class="toggle__track [toggle__track--on]"
 *        role="switch"
 *        aria-checked="true|false"
 *        data-toggle-key="<uniqueKey>"
 *        tabindex="0">
 *     <div class="toggle__thumb"></div>
 *   </div>
 */
export function initToggles() {
  const tracks = $$("[data-toggle-key]");

  tracks.forEach((track) => {
    track.addEventListener("click", () => _flip(track));
    track.addEventListener("keydown", (e) => {
      if (e.key === " " || e.key === "Enter") {
        e.preventDefault();
        _flip(track);
      }
    });
  });
}

function _flip(track) {
  const isOn = track.classList.toggle("toggle__track--on");
  track.setAttribute("aria-checked", String(isOn));

  // Emit a custom event so controllers can react
  track.dispatchEvent(
    new CustomEvent("toggle:change", {
      bubbles: true,
      detail: { key: track.dataset.toggleKey, value: isOn },
    })
  );
}
