/**
 * BAYTACK ADMIN — Verification Service
 * Talks to Controllers/Admin/VerificationController.cs.
 * NOTE: unlike Admin/ProvidersController, this controller has no explicit
 * [Route] override, so it resolves to the default api/[controller] = /verification
 * (not /admin/verification).
 */

import api from '../core/api.js';

const BASE = '/verification';

const VerificationService = {
  /** @param {string|null} status  null = all pending/under-review entries */
  async getQueue(status = null) {
    return api.get(BASE, status ? { status } : {});
  },

  async getById(id) {
    return api.get(`${BASE}/${id}`);
  },

  async markUnderReview(id) {
    return api.patch(`${BASE}/${id}/review`);
  },

  async approve(id) {
    return api.patch(`${BASE}/${id}/approve`);
  },

  async reject(id, reason) {
    return api.patch(`${BASE}/${id}/reject`, { reason });
  },
};

export default VerificationService;
