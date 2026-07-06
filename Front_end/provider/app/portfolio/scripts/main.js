/**
 * main.js — Application entry point
 * Radiant Pro / BayTack
 *
 * Bootstraps components and page controllers after DOM is ready.
 */

import { initToggles } from "./components/toggle.js";
import { initPortfolioPage } from "./controllers/portfolioController.js";

const SERVICE_LABELS = {
  plumbing: "Plumbing", electrical: "Electrical", cleaning: "Cleaning",
  carpentry: "Carpentry", painting: "Painting", ac_repair: "AC Repair",
};

/** Read the signed-in provider's own registration data. */
function getProviderAccount() {
  try {
    const signup = JSON.parse(sessionStorage.getItem("ek_provider_signup") || "{}");
    const sess = JSON.parse(localStorage.getItem("ek_user_session") || "{}");
    const accounts = JSON.parse(localStorage.getItem("ek_accounts") || "[]");
    const phone = signup.phone || sess.phone || "";
    return accounts.find((a) => a.phone === phone) || signup || sess || {};
  } catch (e) {
    return {};
  }
}

/** Fill the sidebar profile block with the provider's real name/service. */
function personalizeSidebar(account) {
  const name = account.name || "Provider";
  const serviceLabel = SERVICE_LABELS[account.service || account.category] || "Service Provider";

  const nameEl = document.getElementById("sidebar-name");
  const roleEl = document.getElementById("sidebar-role");
  const avatarEl = document.getElementById("sidebar-avatar");
  const subtitleEl = document.getElementById("page-header-subtitle");

  if (nameEl) nameEl.textContent = name;
  if (roleEl) roleEl.textContent = serviceLabel;
  if (avatarEl) {
    avatarEl.src = `https://ui-avatars.com/api/?name=${encodeURIComponent(name)}&background=0050d4&color=fff`;
    avatarEl.alt = name;
  }
  if (subtitleEl) subtitleEl.textContent = `Manage how clients see your ${serviceLabel.toLowerCase()} work, ${name.split(" ")[0]}.`;
}

document.addEventListener("DOMContentLoaded", async () => {
  /* ── 1. Personalize the inline sidebar/header with the provider's own signup data ── */
  personalizeSidebar(getProviderAccount());

  /* ── 2. Init UI Components ── */
  initToggles();

  /* ── 3. Wire mobile sidebar toggle (hamburger), if present ── */
  document.querySelector(".top-nav__hamburger")?.addEventListener("click", () => {
    document.getElementById("app-sidebar")?.classList.toggle("is-open");
  });

  /* ── 4. Bootstrap the current page controller ── */
  await initPortfolioPage();
});
