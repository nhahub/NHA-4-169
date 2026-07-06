/**
 * core/api.js
 * ─────────────────────────────────────────────────────────
 * Centralized HTTP client for all API calls.
 * Handles auth headers, error normalization, and JSON parsing.
 */

import AppConfig from './config.js';

/** Retrieve stored auth token (update strategy to match backend) */
function getAuthToken() {
  return localStorage.getItem('baytack_token') || '';
}

/**
 * Core fetch wrapper.
 * @param {string} endpoint  - e.g. '/provider/profile'
 * @param {RequestInit} options
 * @returns {Promise<any>}
 */
async function request(endpoint, options = {}) {
  const url = `${AppConfig.API_BASE_URL}${endpoint}`;

  const defaultHeaders = {
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${getAuthToken()}`,
  };

  const config = {
    ...options,
    headers: { ...defaultHeaders, ...(options.headers || {}) },
  };

  try {
    const response = await fetch(url, config);

    if (!response.ok) {
      const errorBody = await response.json().catch(() => ({}));
      throw new ApiError(response.status, errorBody.message || response.statusText, errorBody);
    }

    // 204 No Content
    if (response.status === 204) return null;

    return await response.json();
  } catch (err) {
    if (err instanceof ApiError) throw err;
    throw new ApiError(0, 'Network error or request failed', { originalError: err });
  }
}

/** Custom error class for API failures */
class ApiError extends Error {
  constructor(status, message, data = {}) {
    super(message);
    this.name    = 'ApiError';
    this.status  = status;
    this.data    = data;
  }
}

/** Convenience HTTP methods */
export const api = {
  get:    (endpoint, opts = {}) => request(endpoint, { method: 'GET',    ...opts }),
  post:   (endpoint, body, opts = {}) => request(endpoint, { method: 'POST',   body: JSON.stringify(body), ...opts }),
  put:    (endpoint, body, opts = {}) => request(endpoint, { method: 'PUT',    body: JSON.stringify(body), ...opts }),
  patch:  (endpoint, body, opts = {}) => request(endpoint, { method: 'PATCH',  body: JSON.stringify(body), ...opts }),
  delete: (endpoint, opts = {}) => request(endpoint, { method: 'DELETE',  ...opts }),
};

export { ApiError };
export default api;
