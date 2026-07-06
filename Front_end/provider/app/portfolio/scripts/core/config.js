/**
 * config.js — App-wide configuration constants
 * Radiant Pro / BayTack
 */

export const APP_CONFIG = {
  /** App name displayed in UI */
  appName: "Radiant Pro",

  /** API base URL — update for each environment */
  apiBase: "/api/v1",

  /** Breakpoints (match CSS variables) */
  breakpoints: {
    mobile: 768,
    tablet: 1024,
  },

  /** Portfolio settings */
  portfolio: {
    maxImages: 5,
    maxDescriptionLength: 500,
    statuses: {
      PUBLIC: "public",
      DRAFT: "draft",
    },
  },

  /** Toast / notification durations (ms) */
  toastDuration: 3500,
};

export default APP_CONFIG;
