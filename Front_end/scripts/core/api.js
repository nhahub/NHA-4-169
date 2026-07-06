/**
 * EKHDEMNI ADMIN — API Client
 * Graceful mock fallback when no backend is present (demo / static mode).
 */

import Config  from './config.js';
import Storage from './storage.js';

/* ── Mock responses per endpoint ── */
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

class ApiClient {
  constructor(baseURL) { this.baseURL = baseURL || ''; }

  async _request(method, endpoint, body) {
    // If no real base URL configured, use mock
    if (!this.baseURL || this.baseURL === '/api' || this.baseURL === '') {
      return this._mockRequest(method, endpoint, body);
    }
    try {
      const opts = { method, headers: _getHeaders() };
      if (body) opts.body = JSON.stringify(body);
      const res = await fetch(`${this.baseURL}${endpoint}`, opts);
      if (!res.ok) throw new Error(`HTTP ${res.status}`);
      return res.json();
    } catch {
      // Fallback to mock on network error
      return this._mockRequest(method, endpoint, body);
    }
  }

  _mockRequest(method, endpoint, body) {
    const mock = _getMockForEndpoint(endpoint);
    if (method === 'GET')    return Promise.resolve(mock ?? []);
    if (method === 'DELETE') return Promise.resolve({ success: true });
    if (body)                return Promise.resolve({ ...body, id: Date.now(), updatedAt: new Date().toISOString() });
    return Promise.resolve({ success: true });
  }

  get(endpoint)          { return this._request('GET',    endpoint); }
  post(endpoint, body)   { return this._request('POST',   endpoint, body); }
  put(endpoint, body)    { return this._request('PUT',    endpoint, body); }
  patch(endpoint, body)  { return this._request('PATCH',  endpoint, body); }
  delete(endpoint)       { return this._request('DELETE', endpoint); }
}

const api = new ApiClient(Config.API_BASE_URL);
export default api;
