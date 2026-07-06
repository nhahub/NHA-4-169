/**
 * BAYTACK ADMIN — Login Controller
 */

import AuthService from '../services/authService.js';

const LoginController = {
  init() {
    if (AuthService.isAuthenticated()) {
      window.location.href = '../admin/analytics.html';
      return;
    }
    this._bindForm();
  },

  _bindForm() {
    const form = document.getElementById('login-form');
    if (!form) return;

    form.addEventListener('submit', async (e) => {
      e.preventDefault();
      const email    = form.querySelector('#email').value.trim();
      const password = form.querySelector('#password').value;
      const btn      = form.querySelector('[type="submit"]');
      const errEl    = document.getElementById('login-error');

      btn.disabled    = true;
      btn.textContent = 'Signing in…';
      if (errEl) errEl.hidden = true;

      try {
        await AuthService.login(email, password);
        window.location.href = '../admin/analytics.html';
      } catch (err) {
        if (errEl) {
          errEl.textContent = err.message ?? 'Login failed. Please try again.';
          errEl.hidden = false;
        }
      } finally {
        btn.disabled    = false;
        btn.textContent = 'Sign In';
      }
    });
  },
};

export default LoginController;
