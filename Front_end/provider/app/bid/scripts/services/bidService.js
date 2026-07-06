/**
 * BayTack — Service: Bid Service
 * All bid-related business logic and API calls.
 */

import api from '../core/api.js';
import { isPositiveNumber } from '../core/helpers.js';

/**
 * @typedef {Object} BidPayload
 * @property {number} jobId
 * @property {number} price
 * @property {number} durationDays
 * @property {string} availability  'immediate' | 'tomorrow' | 'weekend'
 * @property {string} notes
 */

/**
 * Validate a bid payload before submission.
 * @param {BidPayload} payload
 * @returns {{ valid: boolean, errors: string[] }}
 */
export function validateBid(payload) {
  const errors = [];

  if (!isPositiveNumber(payload.price)) {
    errors.push('Please enter a valid price.');
  }

  if (!isPositiveNumber(payload.durationDays)) {
    errors.push('Please enter the estimated duration in days.');
  }

  if (!payload.availability) {
    errors.push('Please select your availability.');
  }

  if (!payload.notes || payload.notes.trim().length < 20) {
    errors.push('Please add proposal notes (at least 20 characters).');
  }

  return { valid: errors.length === 0, errors };
}

/**
 * Submit a bid to the API.
 *
 * NOTE: This demo build has no live backend — /bids always 404s. We store
 * the bid in localStorage (scoped to the signed-in provider) so submitting
 * a bid actually works instead of always failing.
 * @param {BidPayload} payload
 * @returns {Promise<{ success: boolean, bidId: string }>}
 */
export async function submitBid(payload) {
  try {
    return await api.post('/bids', payload);
  } catch (err) {
    return _submitBidLocally(payload);
  }
}

function _currentProviderPhone() {
  try {
    const signup = JSON.parse(sessionStorage.getItem('ek_provider_signup') || '{}');
    const sess = JSON.parse(localStorage.getItem('ek_user_session') || '{}');
    return signup.phone || sess.phone || 'guest';
  } catch (e) {
    return 'guest';
  }
}

function _submitBidLocally(payload) {
  const key = `ek_bids_${_currentProviderPhone()}`;
  let bids = [];
  try { bids = JSON.parse(localStorage.getItem(key) || '[]'); } catch (e) {}
  const bidId = `BID-${Date.now().toString().slice(-6)}`;
  bids.push({ ...payload, bidId, submittedAt: new Date().toISOString(), status: 'pending' });
  try { localStorage.setItem(key, JSON.stringify(bids)); } catch (e) {}
  return { success: true, bidId };
}

/**
 * Fetch all bids for a given job.
 * @param {string|number} jobId
 * @returns {Promise<BidPayload[]>}
 */
export async function getBidsForJob(jobId) {
  return api.get(`/jobs/${jobId}/bids`);
}

/**
 * Retract / cancel a submitted bid.
 * @param {string|number} bidId
 * @returns {Promise<void>}
 */
export async function retractBid(bidId) {
  return api.delete(`/bids/${bidId}`);
}
