/**
 * config.js — App-wide constants and environment configuration
 * The Radiant Marketplace — Provider Hub
 */

const Config = Object.freeze({
  /** API base URL — swap for production endpoint */
  API_BASE_URL: 'https://api.radiantmarketplace.com/v1',

  /** App name */
  APP_NAME: 'The Radiant Marketplace',

  /** Default locale for number/currency formatting */
  LOCALE: 'ar-EG',

  /** Currency symbol */
  CURRENCY: 'ج.م',

  /** Booking statuses */
  STATUS: {
    PENDING:   'pending',
    CONFIRMED: 'confirmed',
    ACTIVE:    'active',
    COMPLETED: 'completed',
    DECLINED:  'declined',
  },

  /** Breakpoints (mirrors CSS media queries) */
  BREAKPOINTS: {
    MOBILE:  768,
    TABLET:  1024,
  },

  /** Transition durations in ms */
  TRANSITION: {
    FAST:   150,
    NORMAL: 300,
    SLOW:   500,
  },
});

export default Config;
