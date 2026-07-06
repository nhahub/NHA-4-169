// ============================================
// PROVIDER HUB — API Core
// File: /scripts/core/api.js
// ============================================

import Config from './config.js';

/**
 * Generic fetch wrapper with timeout & error handling
 * @param {string} endpoint
 * @param {RequestInit} [options]
 * @returns {Promise<any>}
 */
const request = async (endpoint, options = {}) => {
  const controller = new AbortController();
  const timeoutId  = setTimeout(() => controller.abort(), Config.API_TIMEOUT);

  const defaultHeaders = {
    'Content-Type': 'application/json',
    'Accept':       'application/json',
  };

  // Attach auth token if present
  const token = localStorage.getItem(Config.STORAGE_KEYS.AUTH_TOKEN);
  if (token) defaultHeaders['Authorization'] = `Bearer ${token}`;

  try {
    const response = await fetch(`${Config.API_BASE_URL}${endpoint}`, {
      ...options,
      headers: { ...defaultHeaders, ...options.headers },
      signal: controller.signal,
    });

    clearTimeout(timeoutId);

    if (!response.ok) {
      const errorData = await response.json().catch(() => ({}));
      throw new ApiError(response.status, errorData.message || response.statusText, errorData);
    }

    return await response.json();

  } catch (err) {
    clearTimeout(timeoutId);
    if (err.name === 'AbortError') {
      throw new ApiError(408, 'Request timed out');
    }
    throw err;
  }
};

/** Custom API Error class */
class ApiError extends Error {
  constructor(status, message, data = {}) {
    super(message);
    this.name    = 'ApiError';
    this.status  = status;
    this.data    = data;
  }
}

const Api = {
  get:    (endpoint, options = {})         => request(endpoint, { method: 'GET',    ...options }),
  post:   (endpoint, body, options = {})   => request(endpoint, { method: 'POST',   body: JSON.stringify(body), ...options }),
  put:    (endpoint, body, options = {})   => request(endpoint, { method: 'PUT',    body: JSON.stringify(body), ...options }),
  patch:  (endpoint, body, options = {})   => request(endpoint, { method: 'PATCH',  body: JSON.stringify(body), ...options }),
  delete: (endpoint, options = {})         => request(endpoint, { method: 'DELETE', ...options }),
};

export { Api, ApiError };
export default Api;
