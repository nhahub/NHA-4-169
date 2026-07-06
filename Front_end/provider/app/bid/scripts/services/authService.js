/**
 * BayTack — Service: Auth Service
 * Handles authentication state, login, logout, and token management.
 */

import api from '../core/api.js';

const TOKEN_KEY = 'baytack_token';
const USER_KEY  = 'baytack_user';

/**
 * Store auth token and user profile in sessionStorage.
 * @param {string} token
 * @param {Object} user
 */
function persistSession(token, user) {
  sessionStorage.setItem(TOKEN_KEY, token);
  sessionStorage.setItem(USER_KEY, JSON.stringify(user));
}

/** Clear auth session. */
function clearSession() {
  sessionStorage.removeItem(TOKEN_KEY);
  sessionStorage.removeItem(USER_KEY);
}

/**
 * Get the current auth token, or null if not logged in.
 * @returns {string|null}
 */
export function getToken() {
  return sessionStorage.getItem(TOKEN_KEY);
}

/**
 * Get the current user object, or null.
 * @returns {Object|null}
 */
export function getCurrentUser() {
  const raw = sessionStorage.getItem(USER_KEY);
  return raw ? JSON.parse(raw) : null;
}

/** Check whether there is an active session. */
export function isAuthenticated() {
  return Boolean(getToken());
}

/**
 * Log in with email + password.
 * @param {string} email
 * @param {string} password
 * @returns {Promise<Object>} user object
 */
export async function login(email, password) {
  const { token, user } = await api.post('/auth/login', { email, password });
  persistSession(token, user);
  return user;
}

/**
 * Log out the current user.
 * @returns {Promise<void>}
 */
export async function logout() {
  try {
    await api.post('/auth/logout', {});
  } finally {
    clearSession();
  }
}

/**
 * Register a new user account.
 * @param {Object} payload
 * @returns {Promise<Object>} new user object
 */
export async function register(payload) {
  const { token, user } = await api.post('/auth/register', payload);
  persistSession(token, user);
  return user;
}
