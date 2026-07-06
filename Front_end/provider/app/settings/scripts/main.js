/**
 * scripts/main.js
 * ─────────────────────────────────────────────────────────
 * Application entry point.
 * Bootstraps all components and page controllers.
 */

import { initSidebar } from './components/sidebar.js';
import { initSettingsController } from './controllers/settingsController.js';

document.addEventListener('DOMContentLoaded', () => {
  // 1. Shared UI Components
  initSidebar();

  // 2. Page-specific Controllers
  // Detect the current page and init the right controller
  const page = document.body.dataset.page;

  switch (page) {
    case 'settings':
      initSettingsController();
      break;

    // Future pages:
    // case 'bookings':   initBookingsController();   break;
    // case 'earnings':   initEarningsController();   break;
    // case 'reviews':    initReviewsController();    break;

    default:
      // No page-specific controller needed
      break;
  }
});
