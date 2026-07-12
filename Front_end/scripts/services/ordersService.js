/**
 * BAYTACK ADMIN — Orders Service
 *
 * IMPORTANT: there is currently no admin-wide "list all orders" endpoint on
 * the backend. Controllers/BookingsController.cs (GetAllBookingsQuery) is
 * provider-scoped — it requires a providerId and returns only that
 * provider's bookings, which isn't useful for an admin-wide Orders page.
 * The previous repo version had Controllers/Admin/OrdersController.cs
 * (backed by GetAllOrdersAdminQuery / IAdminOrdersReadRepository) which no
 * longer exists in this version.
 *
 * This service is wired to the conventional '/orders' path so the Orders
 * page keeps working off api.js's mock fallback, and will start working for
 * real the moment an admin Orders endpoint exists on the backend again.
 */

import api from '../core/api.js';

const BASE = '/orders';

const OrdersService = {
  /** @param {{page?, perPage?, status?, search?, paymentMethod?}} params */
  async getAll(params = {}) {
    return api.get(BASE, {
      page:    params.page ?? 1,
      perPage: params.perPage ?? 10,
      status:  params.status,
      search:  params.search,
    });
  },

  async getById(id) {
    return api.get(`${BASE}/${id}`);
  },

  async cancel(id, reason) {
    return api.patch(`${BASE}/${id}/cancel`, { reason });
  },
};

export default OrdersService;
