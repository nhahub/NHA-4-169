/**
 * authService.js — Authentication & session management
 * The Radiant Marketplace — Provider Hub
 */

import api from '../core/api.js';

const AuthService = {
  /**
   * Log in with email + password; stores returned token.
   * @param {string} email
   * @param {string} password
   * @returns {Promise<{token: string, user: User}>}
   */
  async login(email, password) {
    const result = await api.post('/auth/login', { email, password });
    if (result?.token) {
      localStorage.setItem('auth_token', result.token);
      localStorage.setItem('auth_user', JSON.stringify(result.user));
    }
    return result;
  },

  /**
   * Log out — clears local session data.
   */
  async logout() {
    try {
      await api.post('/auth/logout', {});
    } catch (_) {
      // Best-effort; always clear local state
    } finally {
      localStorage.removeItem('auth_token');
      localStorage.removeItem('auth_user');
      window.location.href = '/login.html';
    }
  },

  /**
   * Return the stored user object, or null if not logged in.
   * @returns {User|null}
   */
  getCurrentUser() {
    try {
      const raw = localStorage.getItem('auth_user');
      return raw ? JSON.parse(raw) : null;
    } catch {
      return null;
    }
  },

  /**
   * Is there an active session?
   * @returns {boolean}
   */
  isAuthenticated() {
    return Boolean(localStorage.getItem('auth_token'));
  },
};

export default AuthService;

/**
 * @typedef {object} User
 * @property {string} id
 * @property {string} name
 * @property {string} email
 * @property {string} avatarUrl
 * @property {string} role
 */
