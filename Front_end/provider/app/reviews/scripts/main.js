// ============================================
// PROVIDER HUB — App Entry Point
// File: /scripts/main.js
// ============================================

import Sidebar           from './components/sidebar.js';
import ReviewsController from './controllers/reviewsController.js';

/**
 * Determine which page is currently active
 * @returns {string}
 */
const getCurrentPage = () => {
  const path = window.location.pathname;
  if (path.includes('bookings'))  return 'bookings';
  if (path.includes('earnings'))  return 'earnings';
  if (path.includes('settings'))  return 'settings';
  return 'reviews'; // default
};

/**
 * Bootstrap the application
 */
const init = async () => {
  const page = getCurrentPage();

  // Always initialize shared components
  Sidebar.init();
  Sidebar.setActivePage(page);

  // Sync mobile nav active state
  document.querySelectorAll('.mobile-nav__link').forEach(link => {
    const isActive = link.dataset.page === page;
    link.classList.toggle('mobile-nav__link--active', isActive);
  });

  // Initialize page-specific controller
  switch (page) {
    case 'reviews':
      await ReviewsController.init();
      break;

    // Add controllers as pages are built:
    // case 'bookings':
    //   await BookingsController.init();
    //   break;
    // case 'earnings':
    //   await EarningsController.init();
    //   break;
    // case 'settings':
    //   await SettingsController.init();
    //   break;

    default:
      console.warn(`[Main] No controller registered for page: ${page}`);
  }

  console.info(`[Provider Hub] Initialized — page: ${page}`);
};

// Boot when DOM is ready
if (document.readyState === 'loading') {
  document.addEventListener('DOMContentLoaded', init);
} else {
  init();
}
