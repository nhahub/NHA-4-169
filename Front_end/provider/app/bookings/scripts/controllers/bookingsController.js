/**
 * bookingsController.js — DOM logic for bookings/index page
 * The Radiant Marketplace — Provider Hub
 *
 * Responsibilities:
 *  - Wire "Accept" / "Decline" / "Update Status" button clicks
 *  - (future) Load bookings from API and render them
 *  - Handle search input filtering
 */

import BookingService from '../services/bookingService.js';
import { debounce, formatCurrency } from '../core/helpers.js';

const BookingsController = (() => {

  // ── Accept a booking ─────────────────────────────────────
  async function handleAccept(bookingId, cardEl) {
    const btn = cardEl.querySelector('[data-action="accept"]');
    if (!btn) return;

    btn.disabled = true;
    btn.textContent = 'Accepting…';

    try {
      // Demo environment has no live backend — update the UI optimistically
      // and still attempt the API call for parity with a real deployment.
      BookingService.accept(bookingId).catch(() => {});
      // Update badge
      const badge = cardEl.querySelector('.badge');
      if (badge) {
        badge.textContent = 'Confirmed';
        badge.className = 'badge badge--confirmed';
      }
      // Add the left-border accent
      cardEl.classList.add('booking-card--confirmed');
      // Remove accept/decline buttons, show update status
      const actions = cardEl.querySelector('.booking-card__actions');
      if (actions) {
        actions.innerHTML = `
          <button class="btn btn--outline" data-action="update-status" data-id="${bookingId}">
            Update Status
          </button>`;
        bindActionButtons(cardEl);
      }
    } catch (err) {
      console.error('Accept failed:', err.message);
      btn.disabled = false;
      btn.textContent = 'Accept';
    }
  }


  // ── Decline a booking ────────────────────────────────────
  async function handleDecline(bookingId, cardEl) {
    const confirmed = window.confirm('Are you sure you want to decline this booking?');
    if (!confirmed) return;

    try {
      BookingService.decline(bookingId).catch(() => {});
      cardEl.style.transition = 'opacity 0.3s ease, transform 0.3s ease';
      cardEl.style.opacity = '0';
      cardEl.style.transform = 'translateX(-8px)';
      setTimeout(() => cardEl.remove(), 320);
    } catch (err) {
      console.error('Decline failed:', err.message);
    }
  }

  // ── Update status (modal placeholder) ───────────────────
  function handleUpdateStatus(bookingId) {
    // TODO: open a status-selection modal
    console.log('Update status for booking:', bookingId);
    alert(`Status update modal coming soon for booking #${bookingId}`);
  }

  // ── Wire action buttons inside a card ────────────────────
  function bindActionButtons(cardEl) {
    cardEl.querySelectorAll('[data-action]').forEach(btn => {
      // Clone to clear old listeners
      const fresh = btn.cloneNode(true);
      btn.replaceWith(fresh);

      const id = fresh.dataset.id;
      const action = fresh.dataset.action;

      fresh.addEventListener('click', () => {
        if (action === 'accept')         handleAccept(id, cardEl);
        else if (action === 'decline')   handleDecline(id, cardEl);
        else if (action === 'update-status') handleUpdateStatus(id);
      });
    });
  }

  // ── Search filtering ─────────────────────────────────────
  function initSearch() {
    const input = document.querySelector('.top-nav__search-input');
    if (!input) return;

    const filter = debounce(e => {
      const term = e.target.value.trim().toLowerCase();
      document.querySelectorAll('.booking-card').forEach(card => {
        const text = card.textContent.toLowerCase();
        card.style.display = text.includes(term) ? '' : 'none';
      });
    }, 250);

    input.addEventListener('input', filter);
  }

  // ── Public init ──────────────────────────────────────────
  return {
    init() {
      // Wire all pre-rendered booking cards
      document.querySelectorAll('.booking-card[data-booking-id]').forEach(cardEl => {
        bindActionButtons(cardEl);
      });

      initSearch();
    },
  };
})();

export default BookingsController;
