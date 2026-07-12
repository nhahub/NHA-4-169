/**
 * BAYTACK ADMIN — Global Configuration
 */

const Config = Object.freeze({
  // .NET backend (BayTack.API), see backend(.net)/BayTack/BayTack.API/Properties/launchSettings.json.
  API_BASE_URL:    'http://localhost:5025/api',
  REQUEST_TIMEOUT: 15_000,

  STORAGE_KEYS: {
    ACCESS_TOKEN:  'ek_admin_access_token',
    REFRESH_TOKEN: 'ek_admin_refresh_token',
    USER:          'ek_admin_user',
    THEME:         'ek_admin_theme',
  },

  PAGES: {
    ANALYTICS:       'analytics',
    USER_MANAGEMENT: 'user-management',
    CATEGORIES:      'categories',
    ORDERS:          'orders',
    VERIFICATION:    'verification',
    ROLES:           'roles',
    SETTINGS:        'settings',
    LOGIN:           'login',
  },

  ROUTES: {
    analytics:        'analytics.html',
    'user-management':'user-management.html',
    categories:       'categories.html',
    orders:           'orders.html',
    verification:     'verification.html',
    roles:            'roles.html',
    settings:         'settings.html',
    providers:        'providers.html',
    login:            '../index.html',
    // Provider flow routes
    'provider-signup':      '../provider/index.html',
    'provider-step1':       '../provider/step1/index.html',
    'provider-step2':       '../provider/step2/index.html',
    'provider-step3':       '../provider/step3-verification/index.html',
    'provider-dashboard':   '../provider/dashboard/index.html',
  },

  /* ── Services offered ── */
  SERVICES: [
    { value: 'carpentry',   label: 'Carpentry',  icon: 'carpenter' },
    { value: 'painting',    label: 'Painting',   icon: 'format_paint' },
    { value: 'ac_repair',   label: 'AC Repair',  icon: 'ac_unit' },
    { value: 'electrical',  label: 'Electrical', icon: 'bolt' },
    { value: 'plumbing',    label: 'Plumbing',   icon: 'plumbing' },
    { value: 'cleaning',    label: 'Cleaning',   icon: 'cleaning_services' },
  ],

  LOADING_PAGE: '../loading.html',

  PAGINATION: {
    DEFAULT_PAGE:     1,
    DEFAULT_PER_PAGE: 20,
  },

  /* ── Admin Credentials (demo-only) ── */
  ADMIN: {
    USERNAME: 'admin',
    PASSWORD: 'admin012@',
    MIN_PASSWORD_LENGTH: 8,
  },
});

export default Config;
