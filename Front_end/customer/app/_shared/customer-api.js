/**
 * BAYTACK — Customer App Real API Client
 * ============================================================
 * Talks to the real BayTack.API backend. Replaces the old BTData
 * mock/localStorage layer one call at a time.
 *
 * IMPORTANT — path prefixes are NOT uniform across controllers:
 *   - Auth lives under  /api/Auth/...
 *   - Everything else here lives WITHOUT an /api prefix:
 *       /services, /customer/orders, /customer/messages,
 *       /customer/notifications, /customer/requests, /customer/saved,
 *       /customer/profile, /cities
 * This mirrors the backend's actual [Route(...)] attributes - see
 * BayTack-Customer-Portal-Audit.md for the full endpoint table.
 *
 * All calls use credentials:'include' so the httpOnly accessToken
 * cookie set at login is sent automatically - no manual token
 * handling needed here.
 * ============================================================
 */
(function (global) {
  'use strict';

  const ROOT = 'http://localhost:5025'; // no trailing slash, no /api - see note above

  /** Unwraps the backend's ApiResponse<T> envelope { data, isSuccess, errors, statusCode }. */
  async function call(path, options) {
    let res;
    try {
      res = await fetch(`${ROOT}${path}`, {
        credentials: 'include',
        headers: { 'Content-Type': 'application/json' },
        ...options,
      });
    } catch (networkErr) {
      throw new Error('Could not reach the server. Please check your connection and try again.');
    }

    let json = null;
    try { json = await res.json(); } catch (e) { /* empty body, e.g. some 204s */ }

    if (!res.ok || (json && json.isSuccess === false)) {
      const message = (json && json.errors) || `Request failed (${res.status}).`;
      const err = new Error(message);
      err.status = res.status;
      throw err;
    }
    return json ? json.data : null;
  }

  const get = (path) => call(path, { method: 'GET' });
  const post = (path, body) => call(path, { method: 'POST', body: body !== undefined ? JSON.stringify(body) : undefined });
  const patch = (path, body) => call(path, { method: 'PATCH', body: body !== undefined ? JSON.stringify(body) : undefined });
  const del = (path) => call(path, { method: 'DELETE' });

  const CustomerApi = {
    services: {
      /** GET /services?category=&search= */
      all: (opts) => {
        const params = new URLSearchParams();
        if (opts && opts.category) params.set('category', opts.category);
        if (opts && opts.search) params.set('search', opts.search);
        const qs = params.toString();
        return get(`/services${qs ? `?${qs}` : ''}`);
      },
      /** GET /services/{id} */
      byId: (id) => get(`/services/${encodeURIComponent(id)}`),
    },

    orders: {
      /** GET /customer/orders?status=active|completed|cancelled */
      all: (status) => get(`/customer/orders${status ? `?status=${encodeURIComponent(status)}` : ''}`),
      /** GET /customer/orders/{id} */
      byId: (id) => get(`/customer/orders/${encodeURIComponent(id)}`),
      /** POST /customer/orders  { serviceId, tier } */
      create: (serviceId, tier) => post('/customer/orders', { serviceId, tier }),
      /** PATCH /customer/orders/{id}/cancel */
      cancel: (id) => patch(`/customer/orders/${encodeURIComponent(id)}/cancel`),
    },

    saved: {
      /** GET /customer/saved -> full Service objects (different shape than /services - see audit doc) */
      all: () => get('/customer/saved'),
      /** POST /customer/saved/{serviceId} */
      add: (serviceId) => post(`/customer/saved/${encodeURIComponent(serviceId)}`),
      /** DELETE /customer/saved/{serviceId} */
      remove: (serviceId) => del(`/customer/saved/${encodeURIComponent(serviceId)}`),
    },

    messages: {
      /** GET /customer/messages */
      all: () => get('/customer/messages'),
      /** GET /customer/messages/{conversationId} */
      byId: (conversationId) => get(`/customer/messages/${encodeURIComponent(conversationId)}`),
      /** POST /customer/messages/{conversationId}  { text } */
      send: (conversationId, text) => post(`/customer/messages/${encodeURIComponent(conversationId)}`, { text }),
    },

    notifications: {
      /** GET /customer/notifications */
      all: () => get('/customer/notifications'),
      /** PATCH /customer/notifications/{id}/read */
      markRead: (id) => patch(`/customer/notifications/${encodeURIComponent(id)}/read`),
      /** PATCH /customer/notifications/read-all */
      markAllRead: () => patch('/customer/notifications/read-all'),
    },

    requests: {
      /** GET /customer/requests */
      all: () => get('/customer/requests'),
      /** GET /customer/requests/{id} */
      byId: (id) => get(`/customer/requests/${encodeURIComponent(id)}`),
      /** POST /customer/requests - body: { serviceId, title, description, locationDetails, cityId, areaId, budget, deadline, preferredPayment } */
      create: (payload) => post('/customer/requests', payload),
      /** DELETE /customer/requests/{id} */
      remove: (id) => del(`/customer/requests/${encodeURIComponent(id)}`),
      /** POST /customer/requests/{id}/offers/{offerId}/accept */
      acceptOffer: (id, offerId) => post(`/customer/requests/${encodeURIComponent(id)}/offers/${encodeURIComponent(offerId)}/accept`),
    },

    auth: {
      /** POST /api/Auth/change-password  { currentPassword, newPassword } */
      changePassword: (currentPassword, newPassword) =>
        call('/api/Auth/change-password', { method: 'POST', body: JSON.stringify({ currentPassword, newPassword }) }),
      /** POST /api/Auth/logout - revokes the refresh token server-side and clears cookies */
      logout: () => call('/api/Auth/logout', { method: 'POST' }),
    },

    profile: {
      /** GET /customer/profile */
      get: () => get('/customer/profile'),
      /** PATCH /customer/profile  { fullName, phone } */
      update: (fullName, phone) => patch('/customer/profile', { fullName, phone }),
      /** DELETE /customer/profile - soft-deletes the account server-side */
      remove: () => del('/customer/profile'),
    },

    locations: {
      /** GET /cities -> City[] { id, name, areas: [{ id, name }] } */
      cities: () => get('/cities'),
    },
  };

  global.CustomerApi = CustomerApi;
})(window);
