/**
 * BayTack — main.js
 * Application entry point.
 * Bootstraps global components and the correct page controller.
 */

import { initHeader } from './components/header.js';
import { init as initJobDetails } from './controllers/jobDetailsController.js';

/** Map data-page attribute values to their controller init functions. */
const PAGE_CONTROLLERS = {
  'job-details': initJobDetails,
};

document.addEventListener('DOMContentLoaded', () => {
  // ── Global components ──
  initHeader();

  // ── Page-specific controller ──
  const page = document.body.dataset.page;
  const controller = PAGE_CONTROLLERS[page];

  if (controller) {
    controller();
  } else if (page) {
    console.warn(`[BayTack] No controller registered for page: "${page}"`);
  }
});
