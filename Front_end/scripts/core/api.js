/**
 * BAYTACK ADMIN — API Client
 * Talks to the real BayTack.API backend. Falls back to mock data only when
 * the backend can't be reached at all (network error / server not running),
 * so the UI stays usable in a pure front-end demo/offline setting.
 *
 * NOTE: most admin endpoints live at the API root (api/[controller]), e.g.
 * api.get('/categories'), NOT under /admin/*. The one confirmed exception
 * is Providers (Controllers/Admin/ProvidersController.cs has an explicit
 * [Route("api/admin/providers")]) — see providersService.js.
 */

import Config  from './config.js';
import Storage from './storage.js';

/* ── Mock responses per endpoint (offline fallback only) ── */
const MOCK_DATA = {
  '/settings': {
    siteName: 'Ekhdemni', supportEmail: 'support@ekhdemni.com',
    currency: 'EGP', maintenanceMode: false,
    commissionRate: 15, notificationsEnabled: true,
  },
  '/roles': [
    { id:1, name:'Super Admin',     permissions:['all'],                                              userCount:1  },
    { id:2, name:'Operations Admin',permissions:['users.read','orders.read','providers.manage'],      userCount:3  },
    { id:3, name:'Support Agent',   permissions:['users.read','orders.read'],                         userCount:8  },
    { id:4, name:'Finance Manager', permissions:['orders.read','analytics.read','payouts.manage'],    userCount:2  },
  ],
  '/permissions': [
    { id:'users.read',      label:'View Users',       group:'Users'     },
    { id:'users.manage',    label:'Manage Users',     group:'Users'     },
    { id:'orders.read',     label:'View Orders',      group:'Orders'    },
    { id:'orders.manage',   label:'Manage Orders',    group:'Orders'    },
    { id:'providers.manage',label:'Manage Providers', group:'Providers' },
    { id:'analytics.read',  label:'View Analytics',   group:'Analytics' },
    { id:'payouts.manage',  label:'Manage Payouts',   group:'Finance'   },
    { id:'categories.manage',label:'Manage Categories',group:'Content'  },
    { id:'roles.manage',    label:'Manage Roles',     group:'Admin'     },
    { id:'all',             label:'Full Access',      group:'Admin'     },
  ],
  '/categories': [
    { id:1, name:'Carpentry',  icon:'carpenter',         description:'Furniture making, custom woodwork', isActive:true,  createdAt:'2024-01-10' },
    { id:2, name:'Painting',   icon:'format_paint',      description:'Interior & exterior painting',      isActive:true,  createdAt:'2024-01-10' },
    { id:3, name:'AC Repair',  icon:'ac_unit',           description:'AC installation & maintenance',     isActive:true,  createdAt:'2024-01-10' },
    { id:4, name:'Electrical', icon:'bolt',              description:'Wiring, panels, lighting',          isActive:true,  createdAt:'2024-01-10' },
    { id:5, name:'Plumbing',   icon:'plumbing',          description:'Pipe repair, bathroom fitting',     isActive:true,  createdAt:'2024-01-10' },
    { id:6, name:'Cleaning',   icon:'cleaning_services', description:'Deep cleaning, move-in/out',        isActive:true,  createdAt:'2024-01-10' },
  ],
  '/admin/providers/recent': [
    { id:'p1', name:'Cairo Electric Pros',  status:'Verified',    createdAt:'2024-06-01' },
    { id:'p2', name:'Nile Cleaning Co.',    status:'Pending',     createdAt:'2024-06-02' },
    { id:'p3', name:'Master Plumbers Giza', status:'UnderReview', createdAt:'2024-06-03' },
  ],
  '/verification': [
    { id:'v1', name:'Khaled Mansour',  category:'Electrical', providerType:'Individual', status:'Pending',     submittedAt:'2024-10-24', imageUrl:null },
    { id:'v2', name:'Sara El-Din',     category:'Cleaning',   providerType:'Individual', status:'UnderReview', submittedAt:'2024-10-23', imageUrl:null },
    { id:'v3', name:'Mustafa Kamel',   category:'Plumbing',   providerType:'Individual', status:'Pending',     submittedAt:'2024-10-22', imageUrl:null },
    { id:'v4', name:'Hassan Zaki',     category:'Carpentry',  providerType:'Company',    status:'Pending',     submittedAt:'2024-10-21', imageUrl:null },
  ],
  // NOTE: there's no admin-wide "all orders" endpoint on the backend yet
  // (BookingsController.GetAll requires a providerId — it's provider-scoped,
  // not an admin list). This mock keeps the Orders page usable until that
  // endpoint exists; see ordersService.js for details.
  '/orders': {
    items: [
      { id:'ekd-9821', jobTitle:'Master Plumber',    providerName:'Ahmed Mansour', customerName:'Samy El-Sayed', finalPrice:1250, commission:187.5, providerReceived:1062.5, status:'Completed', createdAt:'2024-10-20' },
      { id:'ekd-9822', jobTitle:'Electrical Repair', providerName:'Khaled Ibrahim',customerName:'Mona Hassan',   finalPrice:850,  commission:127.5, providerReceived:722.5,  status:'Pending',   createdAt:'2024-10-19' },
      { id:'ekd-9823', jobTitle:'Deep Cleaning',     providerName:'Fatma Ali',     customerName:'Omar Farouk',   finalPrice:2100, commission:315,   providerReceived:1785,    status:'Cancelled', createdAt:'2024-10-18' },
    ],
    pageIndex: 1, totalPages: 1, totalCount: 3, hasPreviousPage: false, hasNextPage: false,
  },
  // NOTE: no AnalyticsController exists on the backend yet at all (it was
  // present in an earlier version of this repo but isn't in this one) — see
  // analyticsService.js. Mocked here so the Analytics page stays usable.
  '/analytics/kpis': {
    totalRevenue: 1_248_500, activeUsers: 42_890, completedOrders: 12_604, newProviders: 856,
  },
  '/analytics/revenue-trend': [
    { month:'Jan', revenue:72000  }, { month:'Feb', revenue:58000  }, { month:'Mar', revenue:65000  },
    { month:'Apr', revenue:95000  }, { month:'May', revenue:84000  }, { month:'Jun', revenue:118000 },
    { month:'Jul', revenue:107000 }, { month:'Aug', revenue:141000 }, { month:'Sep', revenue:132000 },
    { month:'Oct', revenue:158000 }, { month:'Nov', revenue:147000 }, { month:'Dec', revenue:152400 },
  ],
  '/analytics/categories': [
    { categoryId:'3', categoryName:'AC Repair',  revenue:320000, orderCount:410 },
    { categoryId:'5', categoryName:'Plumbing',   revenue:210000, orderCount:355 },
    { categoryId:'4', categoryName:'Electrical', revenue:180000, orderCount:298 },
  ],
  '/analytics/transactions/top': [
    { orderId:'ek-9482', customerName:'Ahmed Mansour', serviceTitle:'Full Villa AC Revamp', amount:12400, status:'Completed'  },
    { orderId:'ek-9477', customerName:'Laila Hassan',  serviceTitle:'Smart Home Wiring',    amount:8950,  status:'InProgress' },
    { orderId:'ek-9412', customerName:'Youssef Zaid',  serviceTitle:'Emergency Plumbing',   amount:4200,  status:'Completed'  },
  ],
};

function _getMockForEndpoint(endpoint) {
  const base = endpoint.split('?')[0];
  for (const [key, val] of Object.entries(MOCK_DATA)) {
    if (base === key || base.startsWith(key + '/')) return val;
  }
  return null;
}

function _getHeaders() {
  const token = Storage.get(Config.STORAGE_KEYS.ACCESS_TOKEN);
  return {
    'Content-Type': 'application/json',
    ...(token ? { Authorization: `Bearer ${token}` } : {}),
  };
}

/**
 * Append a query-params object to an endpoint path.
 * Skips undefined/null/empty-string values so optional filters don't get
 * sent as literal "undefined" strings.
 */
function _withQuery(endpoint, params) {
  if (!params || typeof params !== 'object') return endpoint;
  const qs = new URLSearchParams();
  for (const [key, value] of Object.entries(params)) {
    if (value === undefined || value === null || value === '') continue;
    qs.set(key, value);
  }
  const query = qs.toString();
  return query ? `${endpoint}?${query}` : endpoint;
}

/**
 * The backend wraps every response in:
 *   { data, isSuccess, errors, errorCode, statusCode, traceId }
 * (see BayTack.Application.Common.DTO.ApiResponse<T>). Unwrap it here so
 * services/controllers just deal with plain data, and throw a real Error
 * (with the backend's message) on failure. Responses that don't match this
 * shape (e.g. offline mock data) pass through as-is.
 */
function _unwrap(json) {
  if (json && typeof json === 'object' && typeof json.isSuccess === 'boolean') {
    if (!json.isSuccess) {
      const err = new Error(json.errors || 'Request failed');
      err.statusCode = json.statusCode;
      err.errorCode  = json.errorCode;
      throw err;
    }
    return json.data;
  }
  return json;
}

class ApiClient {
  constructor(baseURL) { this.baseURL = baseURL || ''; }

  async _request(method, endpoint, bodyOrParams) {
    const isGet = method === 'GET';
    const path  = isGet ? _withQuery(endpoint, bodyOrParams) : endpoint;
    const body  = isGet ? undefined : bodyOrParams;

    // No backend configured at all -> straight to mock.
    if (!this.baseURL) {
      return this._mockRequest(method, path, body);
    }

    let res;
    try {
      const opts = { method, headers: _getHeaders() };
      if (body !== undefined) opts.body = JSON.stringify(body);
      res = await fetch(`${this.baseURL}${path}`, opts);
    } catch (networkErr) {
      // Server unreachable (not running / CORS / offline) — fall back to mock
      // so the UI is still usable, but make it loud in the console.
      console.warn(`[api] Network error calling ${method} ${path} — falling back to mock.`, networkErr);
      return this._mockRequest(method, path, body);
    }

    let json = null;
    try { json = await res.json(); } catch { /* empty body, e.g. 204 */ }

    if (!res.ok) {
      // Prefer the backend's own error envelope if present.
      if (json && typeof json.isSuccess === 'boolean') return _unwrap(json);
      throw new Error(`HTTP ${res.status} calling ${method} ${path}`);
    }

    return _unwrap(json);
  }

  _mockRequest(method, endpoint, body) {
    const mock = _getMockForEndpoint(endpoint);
    if (method === 'GET')    return Promise.resolve(mock ?? []);
    if (method === 'DELETE') return Promise.resolve({ success: true });
    if (body)                return Promise.resolve({ ...body, id: Date.now(), updatedAt: new Date().toISOString() });
    return Promise.resolve({ success: true });
  }

  /** @param {string} endpoint  @param {object} [params]  query-string params */
  get(endpoint, params)  { return this._request('GET',    endpoint, params); }
  post(endpoint, body)   { return this._request('POST',   endpoint, body); }
  put(endpoint, body)    { return this._request('PUT',    endpoint, body); }
  patch(endpoint, body)  { return this._request('PATCH',  endpoint, body); }
  delete(endpoint)       { return this._request('DELETE', endpoint); }
}

const api = new ApiClient(Config.API_BASE_URL);
export default api;
