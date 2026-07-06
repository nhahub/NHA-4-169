/**
 * api.js — Data layer for the Portfolio page
 * Radiant Pro / BayTack
 *
 * NOTE: This demo build has no live backend. The previous version of this
 * file called a real HTTP endpoint (APP_CONFIG.apiBase + "/portfolio/items"),
 * which always failed (404 / network error) and left the Portfolio page
 * permanently stuck on its two hardcoded sample cards with a
 * "Failed to load portfolio items" error toast on every visit.
 *
 * This version persists portfolio items in localStorage, scoped to the
 * signed-in provider's phone number — and seeds a provider's very first
 * portfolio item directly from the data they filled in during registration
 * (their bio, service category, hourly rate, and any work photos they
 * uploaded in Step 2), so the Portfolio page reflects real signup data
 * instead of generic placeholders.
 */

const SERVICE_LABELS = {
  plumbing: "Plumbing", electrical: "Electrical", cleaning: "Cleaning",
  carpentry: "Carpentry", painting: "Painting", ac_repair: "AC Repair",
};

// The category <select> in the edit form uses its own value scheme
// (mostly matching, except AC repair which is stored there as "hvac").
const SERVICE_SELECT_VALUES = {
  plumbing: "plumbing", electrical: "electrical", cleaning: "cleaning",
  carpentry: "carpentry", painting: "painting", ac_repair: "hvac",
};

/** Read the signed-in provider's own registration/account data. */
function getProviderAccount() {
  try {
    const signup = JSON.parse(sessionStorage.getItem("ek_provider_signup") || "{}");
    const sess = JSON.parse(localStorage.getItem("ek_user_session") || "{}");
    const accounts = JSON.parse(localStorage.getItem("ek_accounts") || "[]");
    const phone = signup.phone || sess.phone || "";
    return { phone, ...(accounts.find((a) => a.phone === phone) || signup || sess || {}) };
  } catch (e) {
    return { phone: "guest" };
  }
}

function storageKey(phone) {
  return `ek_portfolio_${phone || "guest"}`;
}

/** Build the provider's first portfolio item(s) from their own signup data. */
function seedFromSignup(account) {
  const serviceLabel = SERVICE_LABELS[account.service || account.category] || "General Services";
  const categoryValue = SERVICE_SELECT_VALUES[account.service || account.category] || "";
  const photos = Array.isArray(account.photos) ? account.photos : [];

  if (photos.length) {
    // Real photos uploaded during registration → real portfolio item.
    return [
      {
        id: 1,
        title: `${serviceLabel} — Work Samples`,
        description: account.bio || `Photos I uploaded when I registered as a ${serviceLabel} provider on BayTack.`,
        price: Number(account.hourlyRate) || 0,
        category: categoryValue,
        imageUrl: photos[0],
        status: "public",
      },
    ];
  }

  // No photos on file yet — a single starter placeholder in their own
  // category, so the page still reflects who they registered as.
  return [
    {
      id: 1,
      title: `My First ${serviceLabel} Project`,
      description: account.bio || "Add photos and details of a recent job to showcase your work to customers.",
      price: Number(account.hourlyRate) || 0,
      category: categoryValue,
      imageUrl: "https://images.unsplash.com/photo-1581578731548-c64695cc6952?w=800",
      status: "draft",
    },
  ];
}

function readItems(phone) {
  try {
    const raw = localStorage.getItem(storageKey(phone));
    return raw ? JSON.parse(raw) : null;
  } catch (e) {
    return null;
  }
}

function writeItems(phone, items) {
  try {
    localStorage.setItem(storageKey(phone), JSON.stringify(items));
  } catch (e) {
    console.error("[api] Failed to persist portfolio items:", e);
  }
}

/* ─────────────────────────────────────────────
   Portfolio Endpoints (localStorage-backed)
───────────────────────────────────────────── */

export async function getPortfolioItems() {
  const account = getProviderAccount();
  let items = readItems(account.phone);
  if (!items) {
    items = seedFromSignup(account);
    writeItems(account.phone, items);
  }
  return items;
}

export async function getPortfolioItem(id) {
  const items = await getPortfolioItems();
  const item = items.find((i) => String(i.id) === String(id));
  if (!item) throw new Error("Project not found.");
  return item;
}

export async function createPortfolioItem(data) {
  const account = getProviderAccount();
  const items = await getPortfolioItems();
  const nextId = items.length ? Math.max(...items.map((i) => Number(i.id) || 0)) + 1 : 1;
  const item = {
    id: nextId,
    title: data.title || "",
    description: data.description || "",
    price: Number(data.price) || 0,
    category: data.category || "",
    imageUrl: data.imageUrl || "https://images.unsplash.com/photo-1581578731548-c64695cc6952?w=800",
    status: "draft",
  };
  items.push(item);
  writeItems(account.phone, items);
  return item;
}

export async function updatePortfolioItem(id, data) {
  const account = getProviderAccount();
  const items = await getPortfolioItems();
  const idx = items.findIndex((i) => String(i.id) === String(id));
  if (idx === -1) throw new Error("Project not found.");
  items[idx] = { ...items[idx], ...data, price: Number(data.price) || items[idx].price };
  writeItems(account.phone, items);
  return items[idx];
}

export async function deletePortfolioItem(id) {
  const account = getProviderAccount();
  const items = await getPortfolioItems();
  writeItems(account.phone, items.filter((i) => String(i.id) !== String(id)));
}

/* ─────────────────────────────────────────────
   Image Upload — kept as a client-side no-op stub
   (no backend to upload to in this demo build)
───────────────────────────────────────────── */

export async function uploadImages(itemId, formData) {
  return { urls: [] };
}

export default {
  getPortfolioItems,
  getPortfolioItem,
  createPortfolioItem,
  updatePortfolioItem,
  deletePortfolioItem,
  uploadImages,
};
