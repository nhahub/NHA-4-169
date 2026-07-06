/**
 * bookingService.js — Booking-domain business logic
 * The Radiant Marketplace — Provider Hub
 *
 * Abstracts all booking-related API calls from the UI layer.
 * Controllers should only call these methods, never `api` directly.
 */

import api from '../core/api.js';
import Config from '../core/config.js';

const BookingService = {
  /**
   * Fetch all bookings for the authenticated provider.
   * @returns {Promise<Booking[]>}
   */
  async getAll() {
    return api.get('/bookings');
  },

  /**
   * Fetch a single booking by ID.
   * @param {string} id
   * @returns {Promise<Booking>}
   */
  async getById(id) {
    return api.get(`/bookings/${id}`);
  },

  /**
   * Accept a pending booking.
   * @param {string} id
   * @returns {Promise<Booking>}
   */
  async accept(id) {
    return api.patch(`/bookings/${id}/accept`, { status: Config.STATUS.CONFIRMED });
  },

  /**
   * Decline a pending booking.
   * @param {string} id
   * @returns {Promise<Booking>}
   */
  async decline(id) {
    return api.patch(`/bookings/${id}/decline`, { status: Config.STATUS.DECLINED });
  },

  /**
   * Mark a confirmed booking as completed.
   * @param {string} id
   * @returns {Promise<Booking>}
   */
  async complete(id) {
    return api.patch(`/bookings/${id}/complete`, { status: Config.STATUS.COMPLETED });
  },

  /**
   * Update a booking's status with a custom value.
   * @param {string} id
   * @param {string} status  One of Config.STATUS values
   * @returns {Promise<Booking>}
   */
  async updateStatus(id, status) {
    return api.patch(`/bookings/${id}`, { status });
  },

  /**
   * Fetch completed bookings for the current week.
   * @returns {Promise<Booking[]>}
   */
  async getCompletedThisWeek() {
    return api.get('/bookings?status=completed&range=week');
  },

  /**
   * Create a manual (offline) job entry.
   * @param {ManualJob} jobData
   * @returns {Promise<Booking>}
   */
  async createManual(jobData) {
    return api.post('/bookings/manual', jobData);
  },
};

export default BookingService;

/* ── JSDoc type stubs ── */

/**
 * @typedef {object} Booking
 * @property {string}   id
 * @property {string}   clientName
 * @property {string}   serviceType
 * @property {string}   location
 * @property {string}   status
 * @property {string}   dateTime
 * @property {string}   duration
 * @property {number}   amount
 */

/**
 * @typedef {object} ManualJob
 * @property {string} clientName
 * @property {string} serviceType
 * @property {string} location
 * @property {string} dateTime
 * @property {number} amount
 */
