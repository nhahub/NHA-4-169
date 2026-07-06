/**
 * core/config.js
 * ─────────────────────────────────────────────────────────
 * Application-wide configuration constants.
 * Import this file wherever you need shared app config.
 */

const AppConfig = Object.freeze({
  /** App metadata */
  APP_NAME:    'The Radiant Marketplace',
  HUB_NAME:   'Provider Hub',
  TAGLINE:    'Managing Egyptian Homes',

  /** API (swap BASE_URL when connecting to real backend) */
  API_BASE_URL: 'https://api.baytack.com/v1',

  /** Local storage keys */
  STORAGE_KEYS: {
    SCHEDULE:      'baytack_schedule',
    PREFERENCES:   'baytack_prefs',
    PROFILE:       'baytack_profile',
    UNSAVED_FLAG:  'baytack_unsaved',
  },

  /** UI timing */
  DEBOUNCE_MS:        400,
  TOAST_DURATION_MS: 3000,

  /** Districts available in the location selector */
  DISTRICTS: ['Maadi', 'Zamalek', 'Heliopolis', 'New Cairo', 'Dokki', 'Mohandessin', '6th of October'],

  /** Days of the week used in the schedule builder */
  WEEK_DAYS: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],

  /** Default working hours */
  DEFAULT_HOURS: { start: '09:00', end: '17:00' },
});

export default AppConfig;
