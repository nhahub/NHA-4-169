/**
 * portfolioService.js — Business logic for portfolio management
 * Radiant Pro / BayTack
 *
 * Sits between controllers and the API layer.
 * Responsible for validation, data transformation, and caching.
 */

import * as api from "../core/api.js";
import { APP_CONFIG } from "../core/config.js";

/** In-memory cache for the current session */
let _cache = null;

/* ─────────────────────────────────────────────
   READ
───────────────────────────────────────────── */

/**
 * Return all portfolio items (with optional cache bypass).
 * @param {boolean} [force=false] - bypass cache
 * @returns {Promise<PortfolioItem[]>}
 */
export async function fetchItems(force = false) {
  if (_cache && !force) return _cache;
  const items = await api.getPortfolioItems();
  _cache = items;
  return items;
}

/**
 * Return a single item by ID.
 * @param {string|number} id
 * @returns {Promise<PortfolioItem>}
 */
export async function fetchItem(id) {
  return api.getPortfolioItem(id);
}

/* ─────────────────────────────────────────────
   WRITE
───────────────────────────────────────────── */

/**
 * Validate and create a new portfolio item.
 * @param {Partial<PortfolioItem>} data
 * @returns {Promise<PortfolioItem>}
 */
export async function createItem(data) {
  _validate(data);
  const item = await api.createPortfolioItem(data);
  invalidateCache();
  return item;
}

/**
 * Validate and update an existing portfolio item.
 * @param {string|number} id
 * @param {Partial<PortfolioItem>} data
 * @returns {Promise<PortfolioItem>}
 */
export async function updateItem(id, data) {
  _validate(data);
  const item = await api.updatePortfolioItem(id, data);
  invalidateCache();
  return item;
}

/**
 * Delete a portfolio item after confirmation.
 * @param {string|number} id
 * @returns {Promise<void>}
 */
export async function deleteItem(id) {
  await api.deletePortfolioItem(id);
  invalidateCache();
}

/* ─────────────────────────────────────────────
   IMAGES
───────────────────────────────────────────── */

/**
 * Upload images for an item; enforces max-image limit.
 * @param {string|number} itemId
 * @param {File[]} files
 * @returns {Promise<{ urls: string[] }>}
 */
export async function uploadItemImages(itemId, files) {
  const { maxImages } = APP_CONFIG.portfolio;
  if (files.length > maxImages) {
    throw new Error(`You can upload a maximum of ${maxImages} images.`);
  }
  const formData = new FormData();
  files.forEach((f) => formData.append("images", f));
  return api.uploadImages(itemId, formData);
}

/* ─────────────────────────────────────────────
   HELPERS
───────────────────────────────────────────── */

/** Clear the in-memory cache so the next fetch hits the network. */
export function invalidateCache() {
  _cache = null;
}

/**
 * Internal validation — throws on invalid data.
 * @param {Partial<PortfolioItem>} data
 */
function _validate(data) {
  const { maxDescriptionLength } = APP_CONFIG.portfolio;
  if (!data.title || !data.title.trim()) {
    throw new Error("Project title is required.");
  }
  if (data.description && data.description.length > maxDescriptionLength) {
    throw new Error(`Description must be ${maxDescriptionLength} characters or fewer.`);
  }
  if (data.price !== undefined && isNaN(Number(data.price))) {
    throw new Error("Price must be a valid number.");
  }
}
