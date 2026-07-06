/**
 * EKHDEMNI — Shared Data Store
 * ============================================================
 * Single source of truth shared between the provider sign-up
 * flow and the admin dashboard via localStorage.
 *
 * Keys:
 *   ek_accounts          — registered user accounts (provider/customer)
 *   ek_admin_providers   — provider entries visible to admin
 *   ek_admin_access_token— admin session token
 *   ek_admin_user        — admin user object
 *   ek_user_session      — logged-in provider/customer session
 * ============================================================
 */

const EK_KEYS = Object.freeze({
  ACCOUNTS:       'ek_accounts',
  ADMIN_PROVIDERS:'ek_admin_providers',
  ADMIN_TOKEN:    'ek_admin_access_token',
  ADMIN_USER:     'ek_admin_user',
  USER_SESSION:   'ek_user_session',
  PROVIDER_SIGNUP:'ek_provider_signup',  // sessionStorage
});

const DataStore = {
  /* ── Generic helpers ── */
  _get(key, fallback = null, storage = localStorage) {
    try {
      const raw = storage.getItem(key);
      if (raw === null) return fallback;
      try { return JSON.parse(raw); } catch { return raw; }
    } catch { return fallback; }
  },
  _set(key, val, storage = localStorage) {
    try { storage.setItem(key, typeof val === 'string' ? val : JSON.stringify(val)); } catch {}
  },
  _remove(key, storage = localStorage) {
    try { storage.removeItem(key); } catch {}
  },

  /* ── Accounts ── */
  getAccounts()      { return this._get(EK_KEYS.ACCOUNTS, []); },
  saveAccount(acct)  {
    const list = this.getAccounts();
    const idx  = list.findIndex(a => a.phone === acct.phone);
    if (idx !== -1) list[idx] = { ...list[idx], ...acct };
    else list.push(acct);
    this._set(EK_KEYS.ACCOUNTS, list);
  },
  findAccount(phone, password) {
    return this.getAccounts().find(a => a.phone === phone && a.password === password) ?? null;
  },
  findByPhone(phone) {
    return this.getAccounts().find(a => a.phone === phone) ?? null;
  },

  /* ── Admin Providers ── */
  getAdminProviders()     { return this._get(EK_KEYS.ADMIN_PROVIDERS, []); },
  saveAdminProvider(p) {
    const list = this.getAdminProviders();
    const idx  = list.findIndex(a => a.id === p.id || a.phone === p.phone);
    if (idx !== -1) list[idx] = { ...list[idx], ...p };
    else list.unshift(p);
    this._set(EK_KEYS.ADMIN_PROVIDERS, list);
  },
  updateProviderStatus(id, status) {
    const list = this.getAdminProviders();
    const idx  = list.findIndex(p => p.id === id || p.phone === id);
    if (idx !== -1) {
      list[idx].status = status;
      this._set(EK_KEYS.ADMIN_PROVIDERS, list);
      // Also update account
      const acct = this.getAccounts().find(a => a.phone === list[idx].phone);
      if (acct) { acct.status = status; this.saveAccount(acct); }
    }
  },
  deleteProvider(id) {
    const filtered = this.getAdminProviders().filter(p => p.id !== id);
    this._set(EK_KEYS.ADMIN_PROVIDERS, filtered);
  },

  /* ── Admin session ── */
  isAdminLoggedIn()  { return Boolean(this._get(EK_KEYS.ADMIN_TOKEN)); },
  setAdminSession(token, user) {
    this._set(EK_KEYS.ADMIN_TOKEN, token);
    this._set(EK_KEYS.ADMIN_USER,  user);
  },
  getAdminUser()     { return this._get(EK_KEYS.ADMIN_USER, null); },
  logoutAdmin() {
    this._remove(EK_KEYS.ADMIN_TOKEN);
    this._remove(EK_KEYS.ADMIN_USER);
  },

  /* ── Provider session (sessionStorage) ── */
  getProviderSignup() { return this._get(EK_KEYS.PROVIDER_SIGNUP, {}, sessionStorage); },
  setProviderSignup(data) { this._set(EK_KEYS.PROVIDER_SIGNUP, data, sessionStorage); },

  /* ── User session ── */
  getUserSession()   { return this._get(EK_KEYS.USER_SESSION, null); },
  setUserSession(s)  { this._set(EK_KEYS.USER_SESSION, s); },
  logoutUser()       { this._remove(EK_KEYS.USER_SESSION); },

  /* ── Register provider (called from step3 submit) ── */
  registerProvider(signupData) {
    // 1. Create account
    this.saveAccount({
      phone:    signupData.phone,
      password: signupData.password,
      name:     signupData.name,
      role:     'provider',
      status:   'pending_review',
    });
    // 2. Create admin-visible entry
    const entry = {
      id:           `PRV-${Date.now().toString().slice(-5)}`,
      name:         signupData.name,
      phone:        signupData.phone,
      email:        signupData.email || '',
      category:     signupData.service,
      providerType: signupData.providerType || 'individual',
      experience:   signupData.experience ? `${signupData.experience} yrs` : '—',
      governorate:  signupData.governorate || '',
      bio:          signupData.bio || '',
      skills:       signupData.skills || [],
      status:       'pending',
      submittedAt:  new Date().toISOString(),
      companyName:  signupData.companyName  || null,
      commercialReg:signupData.commercialReg || null,
      companyAddress:signupData.companyAddress || null,
      img: `https://ui-avatars.com/api/?name=${encodeURIComponent(signupData.name || 'Provider')}&background=0050d4&color=fff`,
    };
    this.saveAdminProvider(entry);
    return entry;
  },
};

export { EK_KEYS };
export default DataStore;
