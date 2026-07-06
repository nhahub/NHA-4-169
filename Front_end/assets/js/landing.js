/**
 * BAYTACK — Landing Page JavaScript
 *
 * Handles:
 * - Modal open / close logic for Login, Register, Customer Sign, Provider Sign
 * - Body scroll lock when modal is open
 * - ESC key closes any open modal
 * - Click outside modal box closes it
 * - Admin link visibility based on stored auth token
 * - "Sign In" form → Admin (username) OR Provider/Customer (phone+password)
 */

const BayTack = (() => {

  /* ── Modal IDs ── */
  const MODALS = {
    login:        'bt-login-modal',
    register:     'bt-register-modal',
    customerSign: 'bt-customer-sign-modal',
    providerSign: 'bt-provider-sign-modal',
  };

  /* ── Admin credentials (static for demo) ── */
  const ADMIN_USERNAME = 'admin';
  const ADMIN_PASSWORD = 'admin012@';

  /* ── Internal helpers ── */
  function _show(id) {
    const el = document.getElementById(id);
    if (!el) return;
    el.classList.remove('hidden');
    el.classList.add('flex');
    document.body.style.overflow = 'hidden';
  }

  function _hide(id) {
    const el = document.getElementById(id);
    if (!el) return;
    el.classList.add('hidden');
    el.classList.remove('flex');
  }

  function _isPhoneNumber(value) {
    // Egyptian phone: starts with +20, 010, 011, 012, 015 or just digits
    return /^(\+20|0)?1[0125]\d{8}$/.test(value.replace(/\s/g, ''));
  }

  function _validatePassword(password) {
    return password.length >= 8;
  }

  /* ── Storage helpers for provider/customer accounts ── */
  function _getAccounts() {
    try { return JSON.parse(localStorage.getItem('ek_accounts') || '[]'); } catch { return []; }
  }

  function _saveAccount(account) {
    const accounts = _getAccounts();
    accounts.push(account);
    localStorage.setItem('ek_accounts', JSON.stringify(accounts));
  }

  function _findAccount(phone, password) {
    return _getAccounts().find(a => a.phone === phone && a.password === password);
  }

  /* ── Public API ── */
  const api = {
    openLogin() {
      api.closeAll();
      _show(MODALS.login);
    },

    openRegister() {
      api.closeAll();
      _show(MODALS.register);
    },

    openCustomerSign() {
      api.closeAll();
      _show(MODALS.customerSign);
    },

    openProviderSign() {
      api.closeAll();
      _show(MODALS.providerSign);
    },

    closeAll() {
      Object.values(MODALS).forEach(_hide);
      document.body.style.overflow = '';
    },

    /** Called by login form submit */
    handleLogin(event) {
      event.preventDefault();
      const errEl      = document.getElementById('bt-login-error');
      const form       = event.target;
      const identInput = document.getElementById('bt-login-identifier') || form.querySelector('[type="text"]');
      const passInput  = document.getElementById('bt-login-password')   || form.querySelector('[type="password"]');
      const identifier = identInput ? identInput.value.trim() : '';
      const password   = passInput  ? passInput.value : '';

      /* Clear previous errors */
      if (errEl) errEl.classList.add('hidden');

      if (!identifier || !password) {
        if (errEl) { errEl.textContent = 'Please fill in all fields.'; errEl.classList.remove('hidden'); }
        return;
      }

      /* Password length check */
      if (!_validatePassword(password)) {
        if (errEl) { errEl.textContent = 'Password must be at least 8 characters.'; errEl.classList.remove('hidden'); }
        [passInput].forEach(el => {
          if (!el) return;
          el.style.outline = '2px solid #b31b25';
          setTimeout(() => { el.style.outline = ''; }, 2000);
        });
        return;
      }

      /* ── Admin login by username ── */
      if (!_isPhoneNumber(identifier)) {
        // Only admin can use non-phone login
        if (identifier === ADMIN_USERNAME && password === ADMIN_PASSWORD) {
          localStorage.setItem('ek_admin_access_token', 'mock_token_baytack_admin');
          localStorage.setItem('ek_admin_user', JSON.stringify({
            name: 'Admin',
            username: 'admin',
            role: 'admin',
          }));
          sessionStorage.setItem('ek_admin_redirect_target', 'admin/analytics.html');
          window.location.href = 'loading.html';
          return;
        }
        if (errEl) {
          errEl.textContent = 'Non-admin users must log in with their phone number.';
          errEl.classList.remove('hidden');
        }
        [identInput].forEach(el => {
          if (!el) return;
          el.style.outline = '2px solid #b31b25';
          setTimeout(() => { el.style.outline = ''; }, 2000);
        });
        return;
      }

      /* ── Phone-based login (Provider / Customer) ── */
      const normalizedPhone = identifier.replace(/\s/g, '');
      const account = _findAccount(normalizedPhone, password);
      if (!account) {
        if (errEl) {
          errEl.textContent = 'Invalid phone number or password.';
          errEl.classList.remove('hidden');
        }
        [identInput, passInput].forEach(el => {
          if (!el) return;
          el.style.outline = '2px solid #b31b25';
          setTimeout(() => { el.style.outline = ''; }, 2000);
        });
        return;
      }

      /* Successful phone login */
      localStorage.setItem('ek_user_session', JSON.stringify({ phone: account.phone, name: account.name, role: account.role }));
      // Redirect based on role
      if (account.role === 'provider') {
        window.location.href = 'provider/dashboard/index.html';
      } else if (account.role === 'customer') {
        window.location.href = 'customer/dashboard/index.html';
      } else {
        window.location.href = '/';
      }
    },

    /** Register a new provider or customer account (called from sign-up flows) */
    registerAccount(data) {
      const existing = _getAccounts().find(a => a.phone === data.phone);
      if (existing) throw new Error('An account with this phone number already exists.');
      _saveAccount({
        phone:    data.phone,
        password: data.password,
        name:     data.name,
        role:     data.role || 'provider',
        status:   data.role === 'customer' ? 'active' : 'pending_review',
        createdAt: new Date().toISOString(),
      });
    },

    /** Handle Customer Sign-Up modal form submit */
    handleCustomerSignUp(event) {
      event.preventDefault();
      const errEl = document.getElementById('bt-customer-sign-error');
      if (errEl) errEl.classList.add('hidden');

      const name     = (document.getElementById('bt-cust-name')  || {}).value || '';
      const phone    = ((document.getElementById('bt-cust-phone') || {}).value || '').replace(/\s/g, '');
      const email    = (document.getElementById('bt-cust-email') || {}).value || '';
      const address  = (document.getElementById('bt-cust-address') || {}).value || '';
      const password = (document.getElementById('bt-cust-password') || {}).value || '';

      if (!name || !phone || !password) {
        if (errEl) { errEl.textContent = 'Please fill in all required fields.'; errEl.classList.remove('hidden'); }
        return false;
      }
      if (!_isPhoneNumber(phone)) {
        if (errEl) { errEl.textContent = 'Please enter a valid Egyptian phone number.'; errEl.classList.remove('hidden'); }
        return false;
      }
      if (!_validatePassword(password)) {
        if (errEl) { errEl.textContent = 'Password must be at least 8 characters.'; errEl.classList.remove('hidden'); }
        return false;
      }
      if (_getAccounts().find(a => a.phone === phone)) {
        if (errEl) { errEl.textContent = 'An account with this phone number already exists.'; errEl.classList.remove('hidden'); }
        return false;
      }

      try {
        api.registerAccount({ name, phone, password, email, address, role: 'customer' });
      } catch (e) {
        if (errEl) { errEl.textContent = e.message; errEl.classList.remove('hidden'); }
        return false;
      }

      /* Auto-login the new customer and go straight to their dashboard */
      localStorage.setItem('ek_user_session', JSON.stringify({ phone, name, role: 'customer' }));
      window.location.href = 'customer/dashboard/index.html';
      return false;
    },
  };

  /* ── Close on backdrop click ── */
  Object.values(MODALS).forEach(id => {
    const el = document.getElementById(id);
    if (!el) return;
    el.addEventListener('click', (e) => {
      if (e.target === el) api.closeAll();
    });
  });

  /* ── Close on ESC ── */
  document.addEventListener('keydown', (e) => {
    if (e.key === 'Escape') api.closeAll();
  });

  /* ── Show Admin link if already logged in ── */
  function _checkAdminLink() {
    const token = localStorage.getItem('ek_admin_access_token');
    const link  = document.getElementById('bt-admin-link');
    if (token && link) link.classList.remove('hidden');
  }

  /* ── Init ── */
  document.addEventListener('DOMContentLoaded', _checkAdminLink);

  return api;
})();

/* ── Public localStorage helpers (used by provider signup flow outside IIFE) ── */
function bt_getAccounts() {
  try { return JSON.parse(localStorage.getItem('ek_accounts') || '[]'); } catch { return []; }
}
function bt_saveAccount(account) {
  const list = bt_getAccounts();
  const idx  = list.findIndex(a => a.phone === account.phone);
  if (idx !== -1) list[idx] = { ...list[idx], ...account };
  else list.push(account);
  localStorage.setItem('ek_accounts', JSON.stringify(list));
}


/* Make openProviderModal available globally (used in original HTML) */
function openProviderModal() { BayTack.openProviderSign(); }

/* ══════════════════════════════════════════════
   PROVIDER MULTI-STEP SIGN-UP — Modal Flow (6 steps)
   0: Type  1: Basic Info+Email  2: Experience+Service
   3: Password  4: Documents  5: Success
   ══════════════════════════════════════════════ */

let _pvCurrentStep = 0;
let _pvType        = '';
let _pvDocs        = { 'id-front': false, 'id-back': false, cert: false };

/* ── Navigate to a step ── */
function pvGoStep(step) {
  document.querySelectorAll('.pv-step').forEach(el => el.classList.add('hidden'));
  const target = document.getElementById(`pv-step-${step}`);
  if (target) target.classList.remove('hidden');
  _pvCurrentStep = step;

  document.querySelectorAll('.pv-step-item').forEach(item => {
    const s   = parseInt(item.dataset.step);
    const dot = item.querySelector('.pv-dot');
    if (s < step) {
      item.classList.remove('opacity-50');
      if (dot) { dot.style.background='#1a7a4a'; dot.style.color='white'; dot.innerHTML='<span class="material-symbols-outlined" style="font-size:0.85rem">check</span>'; }
    } else if (s === step) {
      item.classList.remove('opacity-50');
      if (dot) { dot.style.background='var(--color-primary,#0050d4)'; dot.style.color='white'; dot.textContent=s+1; }
    } else {
      item.classList.add('opacity-50');
      if (dot) { dot.style.background=''; dot.style.color=''; dot.textContent=s+1; }
    }
  });

  // Scroll modal to top
  const modal = document.getElementById('bt-provider-sign-modal');
  if (modal) modal.scrollTop = 0;
}

/* ── Select type ── */
function pvSelectType(type) {
  _pvType = type;
  ['individual','company'].forEach(t => {
    const btn = document.getElementById(`pv-type-${t}`);
    if (btn) {
      btn.style.borderColor = t === type ? 'var(--color-primary)' : '';
      btn.style.background  = t === type ? 'rgba(0,80,212,0.05)' : '';
    }
  });
  const cf = document.getElementById('pv-company-fields');
  if (cf) cf.classList.toggle('hidden', type !== 'company');
}

/* ── Experience slider ── */
function pvUpdateExp(val) {
  const el = document.getElementById('pv-exp-value');
  if (el) el.textContent = val;
  const slider = document.getElementById('pv-exp-slider');
  if (slider) {
    const pct = ((val-1)/29)*100;
    slider.style.background = `linear-gradient(to right,#0050d4 ${pct}%,#e0e0e0 ${pct}%)`;
  }
}

/* ── Validate & advance ── */
function pvNextStep(step) {
  const errEl = document.getElementById(`pv-err-${step}`);
  if (errEl) errEl.classList.add('hidden');

  /* Step 0 — Type */
  if (step === 0) {
    if (!_pvType) {
      if (errEl) { errEl.textContent='Please select Individual or Company.'; errEl.classList.remove('hidden'); }
      return;
    }
    if (_pvType === 'company') {
      const n = document.getElementById('pv-company-name')?.value.trim();
      const r = document.getElementById('pv-commercial-reg')?.value.trim();
      const a = document.getElementById('pv-company-address')?.value.trim();
      if (!n||!r||!a) {
        if (errEl) { errEl.textContent='Please fill in all company fields.'; errEl.classList.remove('hidden'); }
        return;
      }
    }
    pvGoStep(1);
  }

  /* Step 1 — Basic Info */
  else if (step === 1) {
    const name  = document.getElementById('pv-name')?.value.trim();
    const phone = document.getElementById('pv-phone')?.value.trim().replace(/\s/g,'');
    if (!name || name.length < 2) {
      if (errEl) { errEl.textContent='Please enter your full name.'; errEl.classList.remove('hidden'); }
      return;
    }
    if (!/^(\+20|0)?1[0125]\d{8}$/.test(phone)) {
      if (errEl) { errEl.textContent='Please enter a valid Egyptian phone number (+20 1xx xxxx xxxx).'; errEl.classList.remove('hidden'); }
      return;
    }
    // Check duplicate phone
    const existing = bt_getAccounts().find(a => a.phone === phone);
    if (existing) {
      if (errEl) { errEl.textContent='An account with this phone number already exists.'; errEl.classList.remove('hidden'); }
      return;
    }
    pvGoStep(2);
  }

  /* Step 2 — Experience + Service */
  else if (step === 2) {
    const svc = document.querySelector('input[name="pv-category"]:checked');
    if (!svc) {
      if (errEl) { errEl.textContent='Please select your service category.'; errEl.classList.remove('hidden'); }
      return;
    }
    pvGoStep(3);
  }

  /* Step 3 — Password */
  else if (step === 3) {
    const pw  = document.getElementById('pv-password')?.value ?? '';
    const pw2 = document.getElementById('pv-confirm-password')?.value ?? '';
    if (pw.length < 8) {
      if (errEl) { errEl.textContent='Password must be at least 8 characters.'; errEl.classList.remove('hidden'); }
      return;
    }
    if (pw !== pw2) {
      if (errEl) { errEl.textContent='Passwords do not match.'; errEl.classList.remove('hidden'); }
      return;
    }
    pvGoStep(4);
  }
}

/* ── Document upload handler ── */
function pvHandleDoc(key, input) {
  if (!input.files.length) return;
  const fname = input.files[0].name;
  const zone  = document.getElementById(`pv-zone-${key}`);
  const label = document.getElementById(`pv-label-${key}`);
  if (zone) {
    zone.style.borderColor = '#1a7a4a';
    zone.style.borderStyle = 'solid';
    zone.style.background  = 'rgba(26,122,74,0.05)';
    zone.querySelector('.material-symbols-outlined').textContent = 'check_circle';
    zone.querySelector('.material-symbols-outlined').style.color = '#1a7a4a';
  }
  if (label) label.textContent = fname.length > 22 ? fname.slice(0,20)+'…' : fname;
  _pvDocs[key] = true;
  pvUpdateDocBtn();
}

function pvUpdateDocBtn() {
  const consent = document.getElementById('pv-consent')?.checked;
  const idOk    = _pvDocs['id-front'] && _pvDocs['id-back'];
  const btn     = document.getElementById('pv-submit-btn');
  if (btn) btn.disabled = !(consent && idOk);
}

/* ── Password strength ── */
function pvCheckStrength(value) {
  const bar  = document.getElementById('pv-pw-bar');
  const text = document.getElementById('pv-pw-text');
  if (!bar||!text) return;
  let score = 0;
  if (value.length>=8)  score++;
  if (value.length>=12) score++;
  if (/[A-Z]/.test(value)) score++;
  if (/[0-9]/.test(value)) score++;
  if (/[^A-Za-z0-9]/.test(value)) score++;
  const levels=[
    {w:'0%',c:'#e0e0e0',t:'Enter a password'},
    {w:'20%',c:'#ef5350',t:'Too weak'},
    {w:'40%',c:'#ff7043',t:'Weak'},
    {w:'65%',c:'#ffa726',t:'Fair'},
    {w:'85%',c:'#66bb6a',t:'Good'},
    {w:'100%',c:'#2e7d32',t:'Strong ✓'},
  ];
  const lvl=levels[Math.min(score,5)];
  bar.style.width=lvl.w; bar.style.background=lvl.c;
  text.textContent=lvl.t; text.style.color=lvl.c;
}

function pvTogglePw(id, btn) {
  const input = document.getElementById(id);
  if (!input) return;
  const show = input.type==='password';
  input.type = show ? 'text' : 'password';
  const icon = btn.querySelector('.material-symbols-outlined');
  if (icon) icon.textContent = show ? 'visibility_off' : 'visibility';
}

/* ── Final submit ── */
function pvSubmit() {
  const name     = document.getElementById('pv-name')?.value.trim();
  const phone    = document.getElementById('pv-phone')?.value.trim().replace(/\s/g,'');
  const email    = document.getElementById('pv-email')?.value.trim();
  const svc      = document.querySelector('input[name="pv-category"]:checked')?.value;
  const password = document.getElementById('pv-password')?.value;
  const exp      = document.getElementById('pv-exp-slider')?.value ?? '5';
  const bio      = document.getElementById('pv-bio')?.value.trim();

  const btn = document.getElementById('pv-submit-btn');
  if (btn) { btn.disabled=true; btn.innerHTML='<span class="material-symbols-outlined" style="animation:spin 1s linear infinite">progress_activity</span> Creating…'; }

  const account = {
    phone, password, name, email,
    role: 'provider',
    providerType: _pvType,
    service:   svc,
    experience: parseInt(exp),
    bio,
    status:   'pending_review',
    createdAt: new Date().toISOString(),
    companyName:    _pvType==='company' ? document.getElementById('pv-company-name')?.value.trim()    : null,
    commercialReg:  _pvType==='company' ? document.getElementById('pv-commercial-reg')?.value.trim()  : null,
    companyAddress: _pvType==='company' ? document.getElementById('pv-company-address')?.value.trim() : null,
  };

  bt_saveAccount(account);

  try {
    const adminProviders = JSON.parse(localStorage.getItem('ek_admin_providers')||'[]');
    adminProviders.unshift({
      id:           `PRV-${Date.now().toString().slice(-5)}`,
      name, phone, email,
      category:     svc,
      providerType: _pvType,
      experience:   `${exp} yrs`,
      bio,
      status:       'pending',
      submittedAt:  new Date().toISOString(),
      companyName:  account.companyName,
      commercialReg:account.commercialReg,
      img: `https://ui-avatars.com/api/?name=${encodeURIComponent(name)}&background=0050d4&color=fff`,
    });
    localStorage.setItem('ek_admin_providers', JSON.stringify(adminProviders));
  } catch {}

  sessionStorage.setItem('ek_provider_signup', JSON.stringify(account));

  setTimeout(() => {
    if (btn) { btn.disabled=false; btn.innerHTML='Create My Account <span class="material-symbols-outlined">check_circle</span>'; }
    pvGoStep(5);
  }, 700);
}

function pvGoToDashboard() {
  BayTack.closeAll();
  window.location.href = 'provider/dashboard/index.html';
}

/* ── Reset + open ── */
const _origOpenProviderSign = BayTack.openProviderSign.bind(BayTack);
BayTack.openProviderSign = function() {
  _pvCurrentStep = 0;
  _pvType = '';
  _pvDocs = { 'id-front':false, 'id-back':false, cert:false };

  ['pv-name','pv-phone','pv-email','pv-password','pv-confirm-password',
   'pv-bio','pv-company-name','pv-commercial-reg','pv-company-address'].forEach(id => {
    const el = document.getElementById(id); if (el) el.value='';
  });
  document.querySelectorAll('input[name="pv-category"]').forEach(r => r.checked=false);
  ['individual','company'].forEach(t => {
    const btn = document.getElementById(`pv-type-${t}`);
    if (btn) { btn.style.borderColor=''; btn.style.background=''; }
  });
  document.getElementById('pv-company-fields')?.classList.add('hidden');
  document.querySelectorAll('[id^="pv-err-"]').forEach(e => e.classList.add('hidden'));
  const pwBar  = document.getElementById('pv-pw-bar');
  const pwText = document.getElementById('pv-pw-text');
  if (pwBar)  { pwBar.style.width='0%'; pwBar.style.background='#e0e0e0'; }
  if (pwText) { pwText.textContent='Enter a password'; pwText.style.color=''; }
  const submitBtn = document.getElementById('pv-submit-btn');
  if (submitBtn) submitBtn.disabled = true;
  const consent = document.getElementById('pv-consent');
  if (consent) consent.checked = false;
  const expSlider = document.getElementById('pv-exp-slider');
  if (expSlider) { expSlider.value=5; pvUpdateExp(5); }

  // Reset doc zones
  ['id-front','id-back','cert'].forEach(key => {
    const zone  = document.getElementById(`pv-zone-${key}`);
    const label = document.getElementById(`pv-label-${key}`);
    if (zone) { zone.style.borderColor=''; zone.style.borderStyle='dashed'; zone.style.background=''; const icon=zone.querySelector('.material-symbols-outlined'); if(icon){icon.textContent=key==='cert'?'upload_file':'add_a_photo'; icon.style.color='';} }
    if (label) label.textContent = key==='id-front'?'Front Side': key==='id-back'?'Back Side':'Upload Certificate (PDF or image)';
  });

  pvGoStep(0);
  _origOpenProviderSign();
};

// Spin animation
const _pvStyle = document.createElement('style');
_pvStyle.textContent = '@keyframes spin{to{transform:rotate(360deg)}}';
document.head.appendChild(_pvStyle);
