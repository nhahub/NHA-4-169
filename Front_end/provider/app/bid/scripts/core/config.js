/**
 * BayTack — Core: Config
 * App-wide constants and environment settings.
 */

const Config = Object.freeze({
  APP_NAME: 'The Radiant Marketplace',
  APP_VERSION: '1.0.0',

  // API
  API_BASE_URL: '/api/v1',
  API_TIMEOUT_MS: 10_000,

  // Breakpoints (keep in sync with CSS)
  BREAKPOINTS: {
    MOBILE: 768,
    TABLET: 1024,
    DESKTOP: 1280,
  },

  // Transition durations (ms)
  TRANSITION: {
    FAST: 150,
    BASE: 300,
    SLOW: 500,
  },

  // Routes
  ROUTES: {
    HOME:    '/',
    BIDS:    '/bids',
    PROFILE: '/profile',
    JOBS:    '/jobs',
  },
});

export default Config;
