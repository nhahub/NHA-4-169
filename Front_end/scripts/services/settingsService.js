/**
 * EKHDEMNI ADMIN — Settings Service
 * Fetches and persists system-wide configuration.
 */

import api from '../core/api.js';

const SettingsService = {
  async get()           { return api.get('/settings'); },
  async update(payload) { return api.put('/settings', payload); },
};

export default SettingsService;
