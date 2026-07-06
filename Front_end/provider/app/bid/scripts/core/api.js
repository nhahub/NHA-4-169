/**
 * BayTack — Core: API
 * Centralised HTTP client. All network calls go through here.
 */

import Config from './config.js';

class ApiError extends Error {
  constructor(message, status, data) {
    super(message);
    this.name = 'ApiError';
    this.status = status;
    this.data = data;
  }
}

/**
 * Core fetch wrapper with timeout, JSON parsing, and error handling.
 * @param {string} endpoint  Relative path, e.g. '/bids'
 * @param {RequestInit} [options]
 * @returns {Promise<any>}
 */
async function request(endpoint, options = {}) {
  const controller = new AbortController();
  const timeoutId = setTimeout(() => controller.abort(), Config.API_TIMEOUT_MS);

  const url = `${Config.API_BASE_URL}${endpoint}`;

  const defaults = {
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json',
    },
    signal: controller.signal,
  };

  try {
    const res = await fetch(url, { ...defaults, ...options });
    clearTimeout(timeoutId);

    const data = await res.json().catch(() => null);

    if (!res.ok) {
      throw new ApiError(
        data?.message ?? `HTTP ${res.status}`,
        res.status,
        data,
      );
    }

    return data;
  } catch (err) {
    clearTimeout(timeoutId);
    if (err.name === 'AbortError') {
      throw new ApiError('Request timed out', 408, null);
    }
    throw err;
  }
}

const api = {
  get:    (endpoint, opts = {}) => request(endpoint, { method: 'GET', ...opts }),
  post:   (endpoint, body, opts = {}) => request(endpoint, { method: 'POST',   body: JSON.stringify(body), ...opts }),
  put:    (endpoint, body, opts = {}) => request(endpoint, { method: 'PUT',    body: JSON.stringify(body), ...opts }),
  patch:  (endpoint, body, opts = {}) => request(endpoint, { method: 'PATCH',  body: JSON.stringify(body), ...opts }),
  delete: (endpoint, opts = {})       => request(endpoint, { method: 'DELETE', ...opts }),
};

export { api, ApiError };
export default api;
