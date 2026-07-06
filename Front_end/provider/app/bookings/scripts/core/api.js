/**
 * api.js — Lightweight HTTP client
 * The Radiant Marketplace — Provider Hub
 *
 * Wraps fetch() with:
 *  - Base URL injection
 *  - Auth token injection (reads from localStorage)
 *  - Consistent error normalisation
 *  - JSON response parsing
 */

import Config from './config.js';

/**
 * Core request helper.
 * @param {string} endpoint   e.g. '/bookings'
 * @param {object} [options]  Fetch init options
 * @returns {Promise<any>}
 */
async function request(endpoint, options = {}) {
  const token = localStorage.getItem('auth_token');

  const headers = {
    'Content-Type': 'application/json',
    ...(token ? { Authorization: `Bearer ${token}` } : {}),
    ...(options.headers || {}),
  };

  const url = `${Config.API_BASE_URL}${endpoint}`;

  try {
    const response = await fetch(url, { ...options, headers });

    if (!response.ok) {
      const errorBody = await response.json().catch(() => ({}));
      throw new ApiError(response.status, errorBody.message || response.statusText, errorBody);
    }

    // 204 No Content
    if (response.status === 204) return null;

    return response.json();
  } catch (err) {
    if (err instanceof ApiError) throw err;
    // Network failure
    throw new ApiError(0, 'Network error — please check your connection.', {});
  }
}

/** Typed API error for downstream handling. */
export class ApiError extends Error {
  constructor(status, message, data = {}) {
    super(message);
    this.name  = 'ApiError';
    this.status = status;
    this.data   = data;
  }
}

/** HTTP verb shortcuts */
const api = {
  get:    (endpoint, options = {})        => request(endpoint, { method: 'GET',    ...options }),
  post:   (endpoint, body, options = {})  => request(endpoint, { method: 'POST',   body: JSON.stringify(body), ...options }),
  put:    (endpoint, body, options = {})  => request(endpoint, { method: 'PUT',    body: JSON.stringify(body), ...options }),
  patch:  (endpoint, body, options = {})  => request(endpoint, { method: 'PATCH',  body: JSON.stringify(body), ...options }),
  delete: (endpoint, options = {})        => request(endpoint, { method: 'DELETE', ...options }),
};

export default api;
