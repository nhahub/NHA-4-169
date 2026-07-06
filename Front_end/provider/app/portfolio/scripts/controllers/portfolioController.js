/**
 * portfolioController.js — DOM logic for the Portfolio Editor page
 * Radiant Pro / BayTack
 *
 * Responsibilities:
 *  - Render portfolio cards
 *  - Wire up Add / Edit / Delete actions
 *  - Handle the edit panel form (save / discard)
 *  - Coordinate with portfolioService for data
 */

import { $, $$, delegate, showToast } from "../core/helpers.js";
import * as portfolioService from "../services/portfolioService.js";
import { APP_CONFIG } from "../core/config.js";

const SERVICE_LABELS = {
  plumbing: "Plumbing", electrical: "Electrical", cleaning: "Cleaning",
  carpentry: "Carpentry", painting: "Painting", ac_repair: "AC Repair",
};

// The <select> field uses its own value scheme (mostly matching, except
// AC repair, which is stored there as "hvac").
const SERVICE_SELECT_VALUES = {
  plumbing: "plumbing", electrical: "electrical", cleaning: "cleaning",
  carpentry: "carpentry", painting: "painting", ac_repair: "hvac",
};

/** Read the signed-in provider's own registered service category. */
function _getProviderCategory() {
  try {
    const signup = JSON.parse(sessionStorage.getItem("ek_provider_signup") || "{}");
    const sess = JSON.parse(localStorage.getItem("ek_user_session") || "{}");
    const accounts = JSON.parse(localStorage.getItem("ek_accounts") || "[]");
    const phone = signup.phone || sess.phone || "";
    const account = accounts.find((a) => a.phone === phone) || signup || sess || {};
    return SERVICE_SELECT_VALUES[account.service || account.category] || "";
  } catch (e) {
    return "";
  }
}

/* ── State ── */
let _editingId = null; // null → new item, number → existing item id

/* ── Init ── */

/**
 * Bootstrap the portfolio page.
 * Call this once the DOM is ready.
 */
export async function initPortfolioPage() {
  _bindStaticActions();
  _bindEditPanel();
  _bindDescriptionCounter();
  await _renderCards();
}

/* ── Card Rendering ── */

async function _renderCards() {
  const grid = $("#portfolio-grid");
  if (!grid) return;

  try {
    const items = await portfolioService.fetchItems();
    // Clear existing dynamic cards (keep the "add" placeholder)
    grid.querySelectorAll(".js-portfolio-card").forEach((c) => c.remove());

    items.forEach((item) => {
      const card = _buildCard(item);
      grid.appendChild(card);
    });
  } catch (err) {
    console.error("[portfolioController] Failed to load items:", err);
    showToast("Failed to load portfolio items.", "error");
  }
}

/**
 * Build a portfolio card DOM element from item data.
 * @param {PortfolioItem} item
 * @returns {HTMLElement}
 */
function _buildCard(item) {
  const isPublic = item.status === APP_CONFIG.portfolio.statuses.PUBLIC;

  const card = document.createElement("article");
  card.className = "card js-portfolio-card";
  card.dataset.id = item.id;

  card.innerHTML = `
    <div class="card__image-wrapper">
      <img class="card__image" src="${item.imageUrl}" alt="${item.title}" loading="lazy" />
      <span class="card__badge card__badge--${isPublic ? "public" : "draft"}">
        <span class="material-symbols-outlined" style="font-size:0.9rem; ${isPublic ? 'font-variation-settings:"FILL" 1' : ''}" aria-hidden="true">
          ${isPublic ? "check_circle" : "edit_note"}
        </span>
        ${isPublic ? "Public" : "Draft"}
      </span>
    </div>
    <div class="card__body">
      <div class="card__header-row">
        <h3 class="card__title">${item.title}</h3>
        <div class="card__actions">
          <button class="btn-icon btn-icon--ghost js-edit-btn" data-id="${item.id}" aria-label="Edit ${item.title}">
            <span class="material-symbols-outlined" aria-hidden="true">edit</span>
          </button>
          <button class="btn-icon btn-icon--danger js-delete-btn" data-id="${item.id}" aria-label="Delete ${item.title}">
            <span class="material-symbols-outlined" aria-hidden="true">delete</span>
          </button>
        </div>
      </div>
      <p class="card__description">${item.description}</p>
      <div class="card__footer">
        <div>
          <p class="card__price-label">Base Price</p>
          <p class="card__price-value">${item.price.toLocaleString("ar-EG")} <span class="card__price-currency">EGP</span></p>
        </div>
      </div>
    </div>
  `;

  return card;
}

/* ── Static Actions ── */

function _bindStaticActions() {
  const grid = $("#portfolio-grid");
  if (!grid) return;

  // "Add project" button (header)
  $("#btn-add-project")?.addEventListener("click", _openNewItemForm);

  // "Add project" placeholder card
  $("#card-add-placeholder")?.addEventListener("click", _openNewItemForm);

  // Delegate edit/delete from cards rendered into the grid
  delegate(grid, "click", ".js-edit-btn", async (e, btn) => {
    const id = btn.dataset.id;
    await _openEditForm(id);
  });

  delegate(grid, "click", ".js-delete-btn", async (e, btn) => {
    const id = btn.dataset.id;
    await _handleDelete(id);
  });
}

/* ── Edit Panel ── */

function _bindEditPanel() {
  $("#btn-save-changes")?.addEventListener("click", _handleSave);
  $("#btn-discard")?.addEventListener("click", _handleDiscard);
}

function _openNewItemForm() {
  _editingId = null;
  _clearForm();
  // Default the category to the provider's own registered service —
  // most of their projects will be in that category.
  const category = $("#field-category");
  if (category) category.value = _getProviderCategory();
  _scrollToEditPanel();
}

async function _openEditForm(id) {
  _editingId = id;
  try {
    const item = await portfolioService.fetchItem(id);
    _populateForm(item);
    _scrollToEditPanel();
  } catch (err) {
    console.error("[portfolioController] Failed to load item for edit:", err);
    showToast("Could not load project details.", "error");
  }
}

async function _handleSave() {
  const data = _collectFormData();
  try {
    if (_editingId) {
      await portfolioService.updateItem(_editingId, data);
      showToast("Project updated successfully.", "success");
    } else {
      await portfolioService.createItem(data);
      showToast("New project added.", "success");
    }
    _editingId = null;
    _clearForm();
    await _renderCards();
  } catch (err) {
    showToast(err.message || "Save failed.", "error");
  }
}

function _handleDiscard() {
  _editingId = null;
  _clearForm();
}

async function _handleDelete(id) {
  if (!confirm("Are you sure you want to delete this project?")) return;
  try {
    await portfolioService.deleteItem(id);
    showToast("Project deleted.", "info");
    await _renderCards();
  } catch (err) {
    showToast("Delete failed. Please try again.", "error");
  }
}

/* ── Form Helpers ── */

function _collectFormData() {
  return {
    title: $("#field-title")?.value.trim() || "",
    description: $("#field-description")?.value.trim() || "",
    price: parseFloat($("#field-price")?.value) || 0,
    category: $("#field-category")?.value || "",
  };
}

function _populateForm(item) {
  const title = $("#field-title");
  const description = $("#field-description");
  const price = $("#field-price");
  const category = $("#field-category");

  if (title) title.value = item.title || "";
  if (description) description.value = item.description || "";
  if (price) price.value = item.price || "";
  if (category) category.value = item.category || "";

  // Trigger counter update
  description?.dispatchEvent(new Event("input"));
}

function _clearForm() {
  ["#field-title", "#field-description", "#field-price"].forEach((sel) => {
    const el = $(sel);
    if (el) el.value = "";
  });
  // Reset counter
  $("#field-description")?.dispatchEvent(new Event("input"));
}

function _scrollToEditPanel() {
  $("#edit-panel")?.scrollIntoView({ behavior: "smooth", block: "start" });
}

/* ── Description Counter ── */

function _bindDescriptionCounter() {
  const textarea = $("#field-description");
  const counter = $("#description-counter");
  if (!textarea || !counter) return;

  const max = parseInt(textarea.dataset.max || "500", 10);

  const update = () => {
    counter.textContent = `${textarea.value.length} / ${max} chars`;
  };

  textarea.addEventListener("input", update);
  update();
}
