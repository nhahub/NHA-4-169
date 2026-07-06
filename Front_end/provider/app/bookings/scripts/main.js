/**
 * main.js — Application entry point
 * The Radiant Marketplace — Provider Hub
 *
 * Bootstraps all components and page controllers.
 * Loaded as <script type="module"> at the bottom of every page.
 */

import SidebarComponent       from './components/sidebar.js';
import AvailabilityToggle     from './components/availabilityToggle.js';
import ViewToggle             from './components/viewToggle.js';
import BookingsController     from './controllers/bookingsController.js';

/** Run after the DOM is ready. */
function bootstrap() {
  // ── Layout components (run on every page) ──
  SidebarComponent.init();
  AvailabilityToggle.init();

  // ── View toggle (bookings page) ──
  ViewToggle.init();

  // ── Page-level controllers ──
  // Only init if the page contains the relevant root element.
  if (document.querySelector('[data-page="bookings"]')) {
    BookingsController.init();
  }
}

if (document.readyState === 'loading') {
  document.addEventListener('DOMContentLoaded', bootstrap);
} else {
  bootstrap();
}
