// ============================================
// PROVIDER HUB — App Configuration
// File: /scripts/core/config.js
// ============================================

const Config = Object.freeze({
  APP_NAME:    'Provider Hub',
  APP_TAGLINE: 'Managing Egyptian Homes',

  // API
  API_BASE_URL: 'https://api.baytack.com/v1',
  API_TIMEOUT:  10000, // ms

  // Pagination
  REVIEWS_PER_PAGE: 50,

  // Breakpoints (keep in sync with CSS)
  BREAKPOINTS: {
    MOBILE:  768,
    TABLET:  1024,
  },

  // Local storage keys
  STORAGE_KEYS: {
    AUTH_TOKEN:   'ph_auth_token',
    USER_PROFILE: 'ph_user_profile',
    THEME:        'ph_theme',
  },

  // Filter options
  REVIEW_FILTERS: ['all', 'latest', '5stars', 'critical'],
});

export default Config;
